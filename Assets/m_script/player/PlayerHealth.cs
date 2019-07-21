using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using network;

public class PlayerHealth : MonoBehaviour, ISerializable
{
    public int startingHealth = 100;
    public int currentHealth=100;
    public Slider healthSlider;

    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);//红色，透明度0.1
    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    public int isDead=0;
    private int damaged;

    public int getHP()
    {
        return currentHealth;

    }
    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
      

    }
    public void Serialize(ByteBuffer buffer)
    {
        buffer.WriteInt(currentHealth);
        buffer.WriteInt(isDead);
        buffer.WriteInt(damaged);
    }

    public void Deserialize(ByteBuffer buffer)
    {
        currentHealth = buffer.ReadInt();
        isDead = buffer.ReadInt();
        damaged = buffer.ReadInt();
    }

    void Update ()
    {
        healthSlider.value = currentHealth;
        if (damaged == 1)
        {
            damageImage.color = flashColour;
            Debug.Log("flash");
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);//闪过红色，然后淡出成全透明
        }
        damaged = 0;
    }


    public void TakeDamage (int amount)
    {
        damaged = 1;
        Debug.Log("i'm damaged1");

        currentHealth -= amount;

        //healthSlider.value = currentHealth;

        //playerAudio.Play ();

        if(currentHealth <= 0 && isDead == 0)
        {
            Death ();
            Debug.Log("enter die");
        }
    }


    void Death ()
    {
        isDead = 1;

        //playerShooting.DisableEffects ();
        Debug.Log("player die");
        anim.SetInteger("state", -1);
        anim.SetTrigger ("Die");
        //playerAudio.clip = deathClip;
        //playerAudio.Play ();
        healthSlider.enabled = false;
        damageImage.enabled = false;
        Destroy(gameObject, 2f);
        playerMovement.enabled = false;
        playerShooting.enabled = false;
       
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
