using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerManagement : MonoBehaviour
{
    public GameObject player;
    public Vector3 []a; //实例化预制体的position，可自定义
    public Quaternion []b ;//实例化预制体的rotation，可自定义
    PlayerHealth ph;
    PlayerMovement pm;
    public GameObject enemyAi;
    public Slider hSlider;
    public Image dImage;
    public float rebirthTime=3;
    float curTime=0f;
    EnemyAttack eattack;
    public int controlId;
    //public Vector3[] ra; //实例化预制体的position，可自定义
    //public Quaternion []rb ;//实例化预制体的rotation，可自定义
    bool isdead=false;
    public Mesh characterMesh;
    PlayerShooting ps;
    vThirdPersonCamera vc;
    GameObject cm;
    PlayerHealth phc;
    PlayerMovement pmc;
    PlayerShooting psc;
    vThirdPersonCamera vcc;//受控制的角色
    public GameObject[] myplayer;
    public int[]characterID;
    void Start()
    {
        myplayer = new GameObject[a.Length];
        eattack=enemyAi.GetComponent<EnemyAttack>();

        for (int i = 0; i < a.Length; i++)
        {
            Debug.Log(a.Length);
            myplayer[i] = GameObject.Instantiate(player, a[i], b[i]);
            ph = myplayer[i].GetComponent<PlayerHealth>();
            eattack.player = myplayer[controlId];
            pm = myplayer[i].GetComponent<PlayerMovement>();
            pm.playerID = i;
            pm.startpoint = a[i];
            ps = pm.gunhead.GetComponent<PlayerShooting>();
            vc = pm.cm.GetComponent<vThirdPersonCamera>();
            Debug.Log(i);
            cm = pm.cm;
            pm.changerole.setCount(characterID[i]);
            if (pm.playerID != controlId)
            {
                
                Debug.Log("vc" + vc.targ);
                pm.cm.SetActive(false);
                ph.enabled = false;
                pm.enabled = false;
                ps.enabled = false;

                myplayer[i].GetComponent<Invector.CharacterController.vThirdPersonInput>().enabled = false;
                myplayer[i].GetComponent<Invector.CharacterController.vThirdPersonController>().enabled = false;


            }
            else
            {
                vc.targ = myplayer[i];
                ph.enabled = true;
                pm.enabled = true;
                ps.enabled = true;
                pm.cm.SetActive(true);
                ph.healthSlider = hSlider;
                ph.damageImage = dImage;
                phc = ph;
               
                
                //

            }
        }
       
        

    }
   
    public float getDeathtime()
    {
        return curTime;
    }
    void FixedUpdate()
    {
        
        if (phc.isDead == true)
        {
            Debug.Log("rebirth");
            isdead = true;
        }
        if (isdead) {
            curTime = curTime+Time.deltaTime;
            Debug.Log(curTime);
            
            if (curTime>rebirthTime)
                rebirth(a[controlId], b[controlId]);
                
        }
    }

    public void rebirth(Vector3 ra,Quaternion rb)
    {
        Debug.Log("rebirth");
        
        myplayer[controlId] = GameObject.Instantiate(player, ra, rb);
        phc = myplayer[controlId].GetComponent<PlayerHealth>();
        pmc = myplayer[controlId].GetComponent<PlayerMovement>();
        psc = pmc.gunhead.GetComponent<PlayerShooting>();
        vcc = pmc.cm.GetComponent<vThirdPersonCamera>();
        vcc.targ = myplayer[controlId];
        phc.enabled = true;
        pmc.enabled = true;
        psc.enabled = true;
        pmc.cm.SetActive(true);
        pmc.playerID = controlId;
        phc.healthSlider = hSlider;  
        phc.damageImage = dImage;
        isdead = false;
        curTime = 0;
        phc.healthSlider.value = phc.startingHealth;
        eattack = enemyAi.GetComponent<EnemyAttack>();
        eattack.player = myplayer[controlId];
    }


    

}
