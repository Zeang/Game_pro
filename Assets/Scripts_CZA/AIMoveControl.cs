using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIMoveEncounter))]
public class AIMoveControl : MonoBehaviour
{
    [SerializeField]
    private string AICamp = "Blue";
    private string EnemyCamp;

    enum State {Usual, Alert, Encounter };
    //AI state
    private int state;
    private bool StateChanged;
    //view Angle
    private float ViewAngle;

    //view distance
    private float ViewDistance;
    private bool IsFind;

    private RaycastHit hit;


    //AIMove
    private AIMoveEncounter AIChase;
    private AIShooting AIShoot;

    // Start is called before the first frame update
    void Start()
    {
        //AIChase
        AIChase = GetComponent<AIMoveEncounter>();
        //AI Shooting
        AIShoot = GetComponent<AIShooting>();
        //init state is usual
        GetComponent<AIMoveUsual>().enabled = true;
        GetComponent<AIMoveAlert>().enabled = false;
        GetComponent<AIMoveEncounter>().enabled = false;

        state = (int)State.Usual;
        IsFind = true;
        StateChanged = false;

        if(transform.tag == "AIRed")
        {
            AICamp = "Red";
            EnemyCamp = "Blue";
        }
        else if(transform.tag == "AIBlue")
        {
            AICamp = "Blue";
            EnemyCamp = "Red";
        }
    }

    void FixedUpdate()
    {
        PerformFindAttribute();
        PerformFindEnemy();
        PerformFindAlly();
        PerformState();
    }

    private void PerformFindAttribute()
    {
        switch (state)
        {
            case (int)State.Usual:
                {
                    //120度视角
                    ViewAngle = 60.0f;
                    ViewDistance = 10.0f;
                    IsFind = true;
                    break;
                }
            case (int)State.Alert:
                {
                    //210度视角
                    ViewAngle = 105.0f;
                    ViewDistance = 15.0f;
                    IsFind = true;
                    break;
                }
            case (int)State.Encounter:
                {
                    IsFind = false;
                    ViewAngle = 0.0f;
                    break;
                }
        }
    }

    //遍历视野内的敌人
    private void PerformFindEnemy()
    {
        if (IsFind == false)
            return;

        //遍历敌人
        GameObject[] PlayerEnemy = GameObject.FindGameObjectsWithTag("Player" + EnemyCamp);
        //GameObject[] AIEnemy = GameObject.FindGameObjectsWithTag("AI" + EnemyCamp);
        
        foreach (GameObject EnemyP in PlayerEnemy)
        {
            if (Is_SuitView(EnemyP) == true)
            {
                //确定敌人
                state = (int)State.Encounter;
                AIChase.SetEnemy(EnemyP);
                AIShoot.SetEnemy(EnemyP);
                StateChanged = true;
                return;
            }
        }

        //foreach (GameObject EnemyA in AIEnemy)
        //{
        //    if (Is_SuitView(EnemyA) == true)
        //    {
        //        //确定敌人
        //        state = (int)State.Encounter;
        //        AIChase.SetEnemy(EnemyA);
        //        StateChanged = true;
        //        return;

        //    }
        //}

    }

    private void PerformFindAlly()
    {
        if (IsFind == false)
            return;
        //遍历盟友AI
        GameObject[] AIAlly = GameObject.FindGameObjectsWithTag("AI" + AICamp);

        foreach (GameObject Ally in AIAlly)
        {
            if (Ally.transform.position == transform.position)
                continue;
            if(Is_SuitView(Ally) == true)
            {
                int temp = Ally.GetComponent<AIMoveControl>().getState();
                //如果见到盟友AI的状态是 警戒或遭遇 则变为警戒
                if(temp == (int)State.Alert || temp == (int)State.Encounter)
                {
                    state = (int)State.Alert;
                    StateChanged = true;
                }
            }
        }

    }

    private bool Is_SuitView(GameObject obj)
    {
        Vector3 AI2Enemy;
        float distance;
        float angle;

        AI2Enemy = obj.transform.position - transform.position;
        //计算是否在视线距离内
        distance = AI2Enemy.magnitude;
        if (distance > ViewDistance)
            return false;

        //视角
        angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(AI2Enemy));
        if (angle > ViewAngle)
            return false;

        //遮挡
        bool IsAim = Physics.Raycast(transform.position, AI2Enemy, out hit);
        if (true == IsAim && hit.transform == obj.transform)
        {
            return true;
        }
        else
            return false;
    }

    
    //根据state 改变脚本的调用 
    private void PerformState()
    {
        if(true == StateChanged)
        {
            switch(state)
            {
                case (int)State.Usual:
                    {
                        GetComponent<AIMoveUsual>().enabled = true;
                        GetComponent<AIMoveAlert>().enabled = false;
                        GetComponent<AIMoveEncounter>().enabled = false;
                        break;
                    }
                case (int)State.Alert:
                    {
                        GetComponent<AIMoveUsual>().enabled = false;
                        GetComponent<AIMoveAlert>().enabled = true;
                        GetComponent<AIMoveEncounter>().enabled = false;
                        break;
                    }
                case (int)State.Encounter:
                    {
                        GetComponent<AIMoveUsual>().enabled = false;
                        GetComponent<AIMoveAlert>().enabled = false;
                        GetComponent<AIMoveEncounter>().enabled = true;
                        break;
                    }
            }
            StateChanged = false;
        }
    }


    public int getState()
    {
        return state;
    }

}
