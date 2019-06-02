using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;//敌人死了以后沉入地下的速度
    public int scoreValue = 10;//每个敌人死掉以后可以增加多少分数
    public AudioClip deathClip;


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
    }


    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);//每秒移动而不是每帧
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        Debug.Log(currentHealth);
        if(isDead)
            return;

        //enemyAudio.Play ();

        currentHealth -= amount;
            
        //hitParticles.transform.position = hitPoint;
        //hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
            //StartSinking();
        }
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        //enemyAudio.clip = deathClip;
        //enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;//禁用一个组件
        GetComponent <Rigidbody> ().isKinematic = true;//不受物理力控制
        isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}
