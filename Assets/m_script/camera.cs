using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour {

    public Transform target;
    public float smoothing=5f;//相机平滑移动的程度
    Vector3 offset;
    void Start()
    {
        offset = transform.position - target.position;//从角色指向相机的向量
    }
    void FixedUpdate()//如果不用fixed，相机的时间与玩家的时间就会错开
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos,smoothing*Time.deltaTime);

    }
}
