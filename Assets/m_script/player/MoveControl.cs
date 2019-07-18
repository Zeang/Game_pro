using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    private PlayerMovement pm = null;
    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()//固定时间间隔被调用，跟物理引擎一起被更新
    {
        float h = Input.GetAxisRaw("Horizontal");//从横轴获取输入,只有0，-1，1，可以认为是一个方向
        float v = Input.GetAxisRaw("Vertical");//从纵轴获取输入
        pm.Move(h * 5, v * 5);
        //Turning();//要删
        //transform.forward = cameratranform.forward;
        //pm.animating(h, v);
    }
}
