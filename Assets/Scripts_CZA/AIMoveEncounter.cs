using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveEncounter : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private float max_velocity = 4.0f;
    [SerializeField]
    private float max_accelerate = 4.0f;
    [SerializeField]
    private float LookSensitivity = 2.0f;

    private Rigidbody rb;

    private Vector3 velocity = Vector3.zero;

    private Vector3 accelerate = Vector3.zero;

    private Vector3 AI2Enemy;

    private int JumpCount;
    private int LandScapeCount;
    private bool JumpState = true;

    private float HorizontalDuration = 0;
    private float HorizontalFrequency = 0;
    private float HorizontalCoefficient = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        JumpCount = Mathf.FloorToInt(200 * Random.value);
        LandScapeCount = Mathf.FloorToInt(100 * Random.value);
    }

    //Update
    private void FixedUpdate()
    {
        PerformAttribute();
        PerformMove();
        PerformRotate();
        PerformJump();
        PerformLandScape();
    }

    //内在物理性质变化 加速度 速度 面向角度
    private void PerformAttribute()
    {
        AI2Enemy = (enemy.transform.position - rb.position).normalized;
        if ((enemy.transform.position - rb.position).magnitude > 10)
        {
            accelerate = AI2Enemy * max_accelerate;
        }
        else if ((enemy.transform.position - rb.position).magnitude < 6)
        {
            accelerate = -1.0f * AI2Enemy * max_accelerate;
        }

        //Apply the accelerate to velocity
        velocity += accelerate * Time.deltaTime;
        if (velocity.magnitude > max_velocity)
        {
            velocity = (velocity.normalized) * max_velocity;
        }
    }

    private void PerformJump()
    {
        JumpCount--;
        if(JumpCount <= 0)
        {
            JumpCount = Mathf.FloorToInt(200 * Random.value);
            float rand = Random.value;
            if(rand > 0.7)
            {
                Jump();
            }
        }
    }

    //应用位置变化
    private void PerformMove()
    {
        //rb.MovePosition(rb.position + velocity * Time.deltaTime);
        transform.position += velocity * Time.deltaTime;
    }

    private void PerformRotate()
    {
        //当前方向和目标方向的角度
        float angle = Quaternion.Angle(rb.rotation, Quaternion.LookRotation(AI2Enemy));
        
        //每次Update 旋转步长
        float step = LookSensitivity * angle * Time.deltaTime;
        rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(AI2Enemy), step);
    }

    private void PerformLandScape()
    {
        LandScapeCount--;
        if(LandScapeCount <= 0)
        {
            LandScapeCount = Mathf.FloorToInt(120 * Random.value);
            float rand = Random.value;
            if(rand > 0.2)
            {
                HorizontalDuration = Random.Range(0.4f, 3.0f);
                HorizontalFrequency = Random.Range(0.2f, 5.0f);
                HorizontalCoefficient = Random.Range(2.0f, max_velocity);
            }
        }

        LandScapeMove();
    }
    //跳跃
    private void Jump()
    {
        if(true == JumpState)
        {
            rb.velocity += new Vector3(0, 3, 0);
            JumpState = false;
        }
    }

    private void LandScapeMove()
    {
        if(HorizontalDuration > 0)
        {
            //左右偏移的导数 * 时间（即微分）
            float HorizontalDisplacement = HorizontalCoefficient * Mathf.Cos(Time.unscaledTime) * Time.deltaTime;
            transform.position += transform.right * HorizontalDisplacement;
            HorizontalDuration -= Time.deltaTime;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if ("Floor" == collision.gameObject.tag)
        {
            JumpState = true;
        }
    }

    public void SetEnemy(GameObject obj)
    {
        enemy = obj;
    }
}
