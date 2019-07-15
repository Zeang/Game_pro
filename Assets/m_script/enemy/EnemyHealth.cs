using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float per;//slider的数值
    public float sinkSpeed = 2.5f;//敌人死了以后沉入地下的速度
    public int scoreValue = 10;//每个敌人死掉以后可以增加多少分数
    public AudioClip deathClip;
    PlayerHealth playerHealth;
    // public Slider sl;
    GameObject player;
    GameObject emptyplayer;
    playerManagement pManagement;
    int myscore=0;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead=false;
    bool isSinking=false;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();//在子对象中寻找组件
        capsuleCollider = GetComponent <CapsuleCollider> ();
        currentHealth = startingHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        emptyplayer = GameObject.FindGameObjectWithTag("playerManage");
        pManagement = emptyplayer.GetComponent<playerManagement>();
        per = 1;
        
       // sl.value = 1;
    }


    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);//每秒移动而不是每帧
        }
        if (playerHealth.isDead)
        {
            Destroy(gameObject, 2f);
        }

        //sl.value = per;
    }


    public void TakeDamage (int amount, Vector3 hitPoint,int playerID)
    {
        Debug.Log(currentHealth);
        if(isDead)
            return;

        //enemyAudio.Play ();

        currentHealth -= amount;
        per = (float)currentHealth / (float)startingHealth;
            
        //hitParticles.transform.position = hitPoint;
        //hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death (playerID);
            //StartSinking();
        }
    }


    void Death (int playerID)
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");
        GameObject shootingPlayer = pManagement.myplayer[playerID];
        PlayerMovement shootingmove = shootingPlayer.GetComponent<PlayerMovement>();
        myscore=shootingmove.myScore += scoreValue;

        //enemyAudio.clip = deathClip;
        //enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        //GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;//禁用一个组件
        //GetComponent <Rigidbody> ().isKinematic = true;//不受物理力控制
        //isSinking = true;
        // ScoreManager.score += scoreValue;
        ScoreManager.score = myscore;
        Destroy (gameObject, 2f);
    }
}
