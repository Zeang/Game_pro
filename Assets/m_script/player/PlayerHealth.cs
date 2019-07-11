using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
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
    public bool isDead=false;
    bool damaged;

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


    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
            Debug.Log("flash");
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);//闪过红色，然后淡出成全透明
        }
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;
        Debug.Log("i'm damaged1");

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        //playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
            Debug.Log("enter die");
        }
    }


    void Death ()
    {
        isDead = true;

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
