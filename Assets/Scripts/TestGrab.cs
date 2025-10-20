using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TestGrab : MonoBehaviour
{
    public VRTK_InteractGrab grab;
    public bool canCaulateTime;
    public bool isGrab;
    public float initTime;
    void Start()
    {
        grab = GetComponent<VRTK_InteractGrab>();
        grab.ControllerGrabInteractableObject += GrabListener; //当有效对象被抓取时发出事件
        grab.ControllerUngrabInteractableObject += RelesGrabListener;  //当有效对象从抓取到放开时发出事件
    }

    private void RelesGrabListener(object sender, ObjectInteractEventArgs e) //当没抓住鱼竿时
    {
       if(e.target.tag=="yugan") 
        {
            isGrab = false;
            canCaulateTime = false; 
            UIManager.Instance.GetPanel<YuPanel>().OnExit();
            GameMain.Instance.yu.SetActive(false); 
        }
    }
 
    private void GrabListener(object sender, ObjectInteractEventArgs e) //当抓住鱼竿时
    {
       if(e.target.tag=="yugan")  //将布尔值变为true
        {
            isGrab = true;
            initTime = 0;
            UIManager.Instance.GetPanel<YuPanel>().OnEnter();
            canCaulateTime = true;
            if(GameMain.Instance.currentState== GameMain.States.fithPoint)  //判断是否是在第五个任务点
            {
               
                UIManager.Instance.GetPanel<QiaoPanel>().text1.SetActive(false); //使“使用手柄抓取鱼竿，完成垂钓准备”提示消失
                UIManager.Instance.GetPanel<QiaoPanel>().text2.SetActive(true); //打开提示“钓鱼中”
                GameMain.Instance.currentState = GameMain.States.sixthPoint; 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrab)
        {
            if(canCaulateTime) //抓住鱼竿开始钓鱼
            {
                initTime += Time.deltaTime; //计时器
                if (initTime >= 10) //十秒后
                {
                    canCaulateTime = false; //关闭计时器
                    UIManager.Instance.GetPanel<YuPanel>().text1.SetActive(false); //显示钓鱼成功
                    UIManager.Instance.GetPanel<YuPanel>().text2.SetActive(true); //显示恭喜完成任务
                    GameMain.Instance.yu.SetActive(true); //显示鱼的模型
                }
            }
            else
            {
                initTime = 0; //刷新计时器
            }
           
        }
       
    }
}
