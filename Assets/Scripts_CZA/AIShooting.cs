using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooting : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    //AI射击的精确性
    private float ShootAccurary;
    private float ShootingDuration;

    private bool isAim = false;
    private RaycastHit pastHit;
    private RaycastHit hit;
    private GameObject goal;

    private void Start()
    {
        ShootAccurary = Random.Range(0.5f, 1.0f);
        ShootingDuration = Random.Range(2.4f, 4.8f);
    }

    private void Update()
    {
        PerformAim();
        PerformShooting();
    }

    //判断是否瞄准
    private void PerformAim()
    {
        isAim = Physics.Raycast(transform.position, transform.forward, out hit);
        if (isAim && hit.collider.gameObject)
        {
            goal = hit.collider.gameObject;
            if (this.CompareTag("AIRed"))
            {
                if (goal.tag != "PlayerBlue")
                {
                    isAim = false;
                }
            }
            else if (this.CompareTag("AIBlue"))
            {
                if(goal.tag != "PlayerRed")
                {
                    isAim = false;
                }
            }
        }
        //if (isAim==false && g.CompareTag("PlayerRed"))
        //{
        //    isAim = true;
        //}
    }
    
    //是否执行射击
    private void PerformShooting()
    {
        ShootingDuration -= Time.deltaTime;
        if(ShootingDuration <= 0)
        {
            ShootingDuration = Random.Range(2.4f, 4.8f);
            if(isAim)
            {
                Shooting();
            }
        }
    }

    //射击
    private void Shooting()
    {
        
        Ray ray = new Ray(transform.position, hit.transform.position - transform.position);
        Debug.Log("Shooting");
        Debug.DrawLine(ray.origin, hit.point, Color.white);
        if(goal)
            goal.GetComponent<PlayerHealth>().TakeDamage(20);

    }

    public void SetEnemy(GameObject obj)
    {
        enemy = obj;
    }
}
