using UnityEngine;
using System.Collections;
using Common;
using System.Collections.Generic;

public class BasePanel : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    protected Transform mRoot;
    private Dictionary<string, UIEventListener> uiEventListenerCache;
    public bool isShow;

    protected virtual void Awake() 
    {
        uiEventListenerCache = new Dictionary<string, UIEventListener>();
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>(); 
        }
        mRoot = transform;
    }
    /// <summary>
    /// 显示
    /// </summary>
    public void Show()  //显示
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1; //将画布设置为不透明
        canvasGroup.blocksRaycasts = true; //可以被射线检测到
        isShow = true;
    }
    /// <summary>
    /// 隐藏
    /// </summary>
    public void Hide() //隐藏
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0; //将画布设置为透明 
        canvasGroup.blocksRaycasts = false;  //将画布设置为不可以被射线检测到 
        isShow = false;
    }
    /// <summary>
    /// 可点击
    /// </summary>
    public void Clickable() //可以被点击
    {
        canvasGroup.blocksRaycasts = true;  //画布可以被检测到
    } 
    /// <summary>
    /// 不可点击
    /// </summary>
    public void Unclickable() //不可被点击
    {
        canvasGroup.blocksRaycasts = false; //画布不可以被检测到
    }

    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public virtual void OnEnter() //虚方法，方便之后对其进行重写
    {
        Show(); //调用show函数，使其显示出来
        Clickable(); //可以点击
    }

    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual void OnPause() { }

    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResume() { }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关   
    /// </summary>
    public virtual void OnExit()
    {
        Hide();
        Unclickable();
    }
    /// <summary>
    /// 根据名称查找物体注册事件
    /// 根据子物体名称获取监听组件
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UIEventListener GetUIEventListener(string name)
    {
        if (!uiEventListenerCache.ContainsKey(name))          
        {
            var listener = UIEventListener.GetListener(transform.FindChildByName(name));
            uiEventListenerCache.Add(name, listener);
        }
        return uiEventListenerCache[name];
    }



}
