using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveAlert : MonoBehaviour
{
    //AI 徘徊移动
    [SerializeField]
    private float max_velocity = 1.0f;
    [SerializeField]
    private float max_accelerate = 2.0f;
    [SerializeField]
    private float LookSensitivity = 4.0f;

    private Vector3 velocity = Vector3.zero;

    private Vector3 accelerate = Vector3.zero;

    private float MoveDuration;

    private Vector3 LookForward = Vector3.zero;

    private bool RotationDone;

    //Initial
    private void Start()
    {
        MoveDuration = Random.Range(1.2f, 3.6f);
        LookForward.x = Random.Range(-1.0f, 1.0f);
        LookForward.z = Random.Range(-1.0f, 1.0f);
        accelerate = LookForward.normalized * max_accelerate;
        RotationDone = false;
    }
    // update
    private void FixedUpdate()
    {
        PerformAttribute();
        PerformRotate();
        PerformMove();

    }

    //加速度 速度的变化 更新状态等
    private void PerformAttribute()
    {

        if (MoveDuration > 0)
        {
            if (RotationDone == true)
            {
                MoveDuration -= Time.deltaTime;
                velocity += accelerate;
                if (velocity.magnitude > max_velocity)
                {
                    velocity = velocity.normalized * max_velocity;
                }
            }
        }
        else
        {
            MoveDuration = Random.Range(0.8f, 3.6f);
            LookForward.x = Random.Range(-1.0f, 1.0f);
            LookForward.z = Random.Range(-1.0f, 1.0f);
            accelerate = LookForward.normalized * max_accelerate;
            RotationDone = false;
            velocity = Vector3.zero;
        }

    }

    //
    private void PerformMove()
    {
        if (false == RotationDone)
            return;

        transform.position += velocity * Time.deltaTime;

    }

    //旋转
    private void PerformRotate()
    {
        float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(LookForward));
        if (angle < 0.1)
        {
            RotationDone = true;
        }
        float step = LookSensitivity * 60.0f * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(LookForward), step);
    }

    //通知友军警戒
    private void Inform()
    {

    }
}
