using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
///在脚本中，首先声明一个Gameobject的数组和一个ScrollRect的数组，
///里面分别存放五个模型和与之对应的ScrollRect，他们的索引从0开始。每次在点击“下一个”时会触发onclick事件，
///调用show manager脚本中的BtuListenr方法。
///两个数组的索引都将会加一，即将当前模型的下一个模型在窗口中展示出来。
///如果点击次数超出了数组长度，那么代码将会将点击次数对模型数组长度（5）取余，也就是说将会显示求模后所得的索引所代表的的模型。
public class ShowManager : MonoBehaviour
{
    public GameObject[] Models;
    public ScrollRect[] scrolls;
    public int index;
    public RotateModel RM;
    //private void Start()
    //{
    //    index = 0;
    //    SetShow(index); 
    //}
    public void RestShow()
    {
        index = 0; //初始索引为0
        SetShow(index); //调用setshow函数
    }
    public void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    index++;
        //    SetShow(index%Models.Length);
        //}
    }
    public void BtuListenr()
    {
        index++; //每按一次都加一
        SetShow(index % Models.Length);  //求模后查找索引
    }
    public void SetShow(int index)
    {
        for (int i = 0; i < Models.Length; i++) // for 循环遍历所有模型
        {
            if(i==index)
            {
                Models[i].SetActive(true); //将索引所代表的的模型激活
                RM.ResetPosAndScale(); //重置使其恢复初始位置
            }
            else
            {
                Models[i].SetActive(false); //其他的模型都使其消失
            }
           
        }
        for (int i = 0; i < scrolls.Length; i++) //遍历滚动条数组中的讲解ui
        {
            if(i==index) 
            {
                scrolls[i].gameObject.SetActive(true); //将讲解界面激活
                scrolls[i].verticalScrollbar.value = 1;  //使其滚动条复位到第一页所在的位置
            }
            else
            {
                scrolls[i].gameObject.SetActive(false); //使该模型对应的讲解界面消失
            }
            
        }
    }
}
