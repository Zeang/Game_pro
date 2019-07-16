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
        if(true == isAim && hit.collider.tag != "PlayerRed")
        {
            isAim = false;
        }
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
        
    }

    public void SetEnemy(GameObject obj)
    {
        enemy = obj;
    }
}
