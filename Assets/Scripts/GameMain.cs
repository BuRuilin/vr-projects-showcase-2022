using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class GameMain : MonoSingleton<GameMain>
{
    public enum States { idle, firstPoint, secondPoint, thirdPoint,fouthPoint,fithPoint,sixthPoint } 
    ///枚举类型 枚举可以使代码更易于维护 更清晰
    public GameObject firstPoint;
    public GameObject SecondPoint;
    public GameObject thirdPoint;
    public GameObject fouthPoint;
    public GameObject fithPoint;
   // public GameObject sixthPoint;  
    public GameObject pintuObj;
    public States currentState;
    public int pintuCount;
    public Transform pintu1Point;
    public Transform pintu2Point;
    public Transform pintu3Point;
    public Transform pintu4Point;
    public GameObject yugan;
    public GameObject yu;
    
    // Start is called before the first frame update
    void Start()
    {
        pintuCount = 0; 
        UIManager.Instance.GetPanel<TipPanel>().OnEnter();
        currentState = States.idle; 
        //简单的状态机 currentState意思是当前的状态，然后通过调整枚举，来切换状态
        ///只需要将页面的currentState属性设置为其中某个状态，就会让当前页面变成这个状态时的样式
        firstPoint.SetActive(false);
        SecondPoint.SetActive(false);  
        pintuObj.SetActive(false);
        thirdPoint.SetActive(false);
        fithPoint.SetActive(false);
        fouthPoint.SetActive(false);
        yugan.SetActive(false); 
       // sixthPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetOnePoint()
    {
        firstPoint.SetActive(false);
        currentState = States.firstPoint;
        UIManager.Instance.GetPanel<MenPanel>().OnEnter(); 
    }
    public void SetTwoPoint()
    {
        SecondPoint.SetActive(false);
        currentState = States.secondPoint;
        pintuObj.SetActive(true);
        UIManager.Instance.GetPanel<PintTuPanel>().OnEnter(); 
    }
    public void SetThirdPoint()
    {
        thirdPoint.SetActive(false);
        currentState = States.thirdPoint;
        UIManager.Instance.GetPanel<PintTuPanel>().OnExit();
        UIManager.Instance.GetPanel<ZoulangPanel>().OnEnter();
        fouthPoint.SetActive(true); 
    }
    public void SetFouthPoint()
    {
        currentState = States.fouthPoint; 
        fouthPoint.SetActive(false);
        UIManager.Instance.GetPanel<ZoulangPanel>().OnExit();
        UIManager.Instance.GetPanel<ZhaiPanel>().OnEnter();
        fithPoint.SetActive(true);
    }
    public void SetFithPoint()
    {
        currentState = States.fithPoint;
        fithPoint.SetActive(false);
        UIManager.Instance.GetPanel<ZhaiPanel>().OnEnter();
        yugan.SetActive(true);
        UIManager.Instance.GetPanel<QiaoPanel>().OnEnter();
        yu.SetActive(false); 

    }
}
