using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerManagement : MonoBehaviour
{

   
    public GameObject player;
    public Vector3 a = new Vector3(0, 0, 0); //实例化预制体的position，可自定义
    public Quaternion b = new Quaternion(0, 0, 0, 0);//实例化预制体的rotation，可自定义
    PlayerHealth ph;
    public Slider hSlider;
    public Image dImage;
    public float rebirthTime=3;
    float curTime=0f;
    public Vector3 ra = new Vector3(0, 0, 0); //实例化预制体的position，可自定义
    public Quaternion rb = new Quaternion(0, 0, 0, 0);//实例化预制体的rotation，可自定义
    bool isdead=false;
    public Mesh characterMesh;
    void Start()
    {
       
        
        GameObject myplayer = GameObject.Instantiate(player, a, b);
        ph = myplayer.GetComponent<PlayerHealth>();
        ph.healthSlider = hSlider;
        ph.damageImage = dImage;
       
        

    }
   
    public float getDeathtime()
    {
        return curTime;
    }
    void FixedUpdate()
    {
        if (ph.isDead == true)
        {
            Debug.Log("rebirth");
            isdead = true;
        }
        if (isdead) {
            curTime = curTime+Time.deltaTime;
            Debug.Log(curTime);
            
            if (curTime>rebirthTime)
                rebirth(ra, rb);
                
        }
    }

    public void rebirth(Vector3 ra,Quaternion rb)
    {
        
        
        GameObject myplayer = GameObject.Instantiate(player, ra, rb);
        ph = myplayer.GetComponent<PlayerHealth>();
        ph.healthSlider = hSlider;
        ph.damageImage = dImage;
        isdead = false;
        curTime = 0;
        ph.healthSlider.value = ph.startingHealth;
    }


    

}
