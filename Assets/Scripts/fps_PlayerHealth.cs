using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_PlayerHealth : MonoBehaviour
{

    public bool isDead;
    public float resetAfterDeathTime = 5;
    public AudioClip deathClip;
    public AudioClip damageClip;

    public float maxHp = 100;
    public float hp = 100;
    public float recoverSpeed = 1;


    private float timer = 0;
    private FadeInOut fader;


}
