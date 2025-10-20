using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateModel : MonoBehaviour {

    public Transform modelTransform;
    public Vector3 startPoint;
    public Vector3 startAngel;
    public Vector3 initAngel;
    [Range(0.1f,1f)]
    public float rotateScale=1f;
    public bool isRotate;
    public Camera cam; 
	// Use this for initialization
	void Awake () {
        initAngel = modelTransform.eulerAngles; 
	}
	public void ResetPosAndScale()
    {
        cam.orthographicSize = 5; //That magic number is the number of units from the center half of the screen to the top, or bottom.
        modelTransform.eulerAngles = initAngel; //刷新初始角度
    }
	// Update is called once per frame
	//void Update () {
		
 //       if(Input.GetMouseButtonDown(0)&&!isRotate)
 //       {
 //           isRotate = true;
 //           startPoint = Input.mousePosition;
 //           startAngel = modelTransform.eulerAngles;
 //       }
 //       if(Input.GetMouseButtonUp(0))
 //       {
 //           isRotate = false;
 //       }
 //       if(isRotate)
 //       {
 //           var currentPoint = Input.mousePosition;
 //           var x = startPoint.x - currentPoint.x;
 //           modelTransform.eulerAngles = startAngel + new Vector3(0, x * rotateScale, 0);
 //       }
	//}
    public void BeginDrag()
    {
        isRotate = true;
        startPoint = Input.mousePosition; //输入目前像素坐标下的鼠标位置
        startAngel = modelTransform.eulerAngles; // 表示模型在世界空间中的旋转
    }
    public void Drag()
    {
        var currentPoint = Input.mousePosition; //获取现在的鼠标位置
        var x = startPoint.x - currentPoint.x;  //计算得到鼠标位置在x轴上的偏移
        modelTransform.eulerAngles = startAngel + new Vector3(0, x * rotateScale, 0); //根据鼠标移动的距离计算模型转动的欧拉角（初始位置角度加上计算得知的偏移角度）
        
    }
    public void EndDrag() 
    {
        isRotate = false;
    }
    public void Scorlle()
    {
     float m = cam.orthographicSize -= Input.mouseScrollDelta.y * 1f;
        cam.orthographicSize = Mathf.Clamp(m, 1f, 10f); //鼠标滚轮与摄像机的视体之间的转换，用于操控用户能看到的视角远近
    }
}
