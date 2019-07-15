using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player;//敌人在游戏的过程中不断的被创建，因此需要它自己去寻找玩家
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;
    GameObject emptyplayer;
    playerManagement pManagement;
    int playerID;
    GameObject[] myplayer;


    void Start ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        Debug.Log("fef"+player);
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
        emptyplayer = GameObject.FindGameObjectWithTag("playerManage");
        pManagement = emptyplayer.GetComponent<playerManagement>();
        myplayer = pManagement.myplayer;
    }


    void Update ()
    {
        int controlID = pManagement.controlId;
        if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            nav.SetDestination (myplayer[controlID].transform.position);//保持追踪敌人
        }
        else
        {
            nav.enabled = false;
        }
    }
}
