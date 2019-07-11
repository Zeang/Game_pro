using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;//每一次攻击间隔的时间
    public int attackDamage = 10;


    Animator anim;
    public playerManagement playermanagement;
    public GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;//保证敌人的速度不会太快
   

    void Awake ()
    {
       
        playerHealth = player.GetComponent <PlayerHealth> ();
        Debug.Log("playerhealth"+playerHealth);
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack ();
        }

        if(playerHealth.currentHealth <= 0)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            anim.SetTrigger ("PlayerDead");
            Destroy(gameObject,2f);
        }
    }


    void Attack ()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            Debug.Log("takedamage");
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
