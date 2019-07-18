using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameProcessManager : MonoBehaviour
{
    public Color leftColor = Color.white;//第一队，我现在设的是序号前三个人
    public Color rightColor = Color.red;//第二队
    public Color notOccupiedColor = Color.black;
    public Color notOccupiedColor2 = Color.red;
    public Material fhzMat;
    public GameObject leftBirth = null;
    public GameObject rightBirth = null;
    public GameObject birthControl = null;
    public GameObject fhzObject = null;
    public float winTime = 10.0f;//占点直到获得胜利的时间
    public float EndTime = 900.0f;//游戏结束的时间
    public float SearchArea = 1.5f;//感应区的大小
    public Text totalTime = null;
    public Text occupyTime = null;

    private playerManagement playerManager = null;
    public GameObject[] leftPlayer = null;
    private GameObject[] rightPlayer = null;
    private int playerNow = 0;
    private fhz fhz = null;
    private Projector projector = null;

    //在占领区域的两方人数
    private int OccupiedLeft = 0;
    private int OccupiedRight = 0;
    //当前的优势方 -1为平，0为左，1为右
    private int superiority = -1;
    private float TimeRecord = 0;
    private int controlID = 1;//本机玩家编号

    private float gameTimeAll = 0;
    private int controlSide;//本机控制角色对应的一方

    // Start is called before the first frame update
    void Start()
    {
        //初始化两方角色
        if (birthControl)
        {
            //初始化
            playerManager = birthControl.GetComponent<playerManagement>();

            playerManager.a = new Vector3[6] {leftBirth.transform.position+new Vector3(1,0,-0.5f), leftBirth.transform.position,leftBirth.transform.position+new Vector3(-1,0,-0.5f),
            rightBirth.transform.position+new Vector3(1,0,-0.5f),rightBirth.transform.position,rightBirth.transform.position+new Vector3(-1,0,-0.5f)};

            playerManager.b = new Quaternion[6] { leftBirth.transform.rotation, leftBirth.transform.rotation, leftBirth.transform.rotation,
            rightBirth.transform.rotation, rightBirth.transform.rotation , rightBirth.transform.rotation };

            //playerManager.ra = playerManager.a[0];
            //playerManager.rb 
            playerManager.characterID = new int[6] { 2, 0, 1, 2, 0, 1 };//选角色在这里，012三种角色

            playerManager.controlId = controlID;//本机玩家编号

            if (controlID < 3)
            {
                controlSide = 0;
            }
            else
            {
                controlSide = 1;
            }

            playerManager.enabled = true;
        }

        if (fhzObject)
        {
            fhz = fhzObject.GetComponent<fhz>();
        }
        projector = GetComponent<Projector>();

        if (occupyTime)
        {
            occupyTime.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameTimeAll += Time.deltaTime;
        if (gameTimeAll > EndTime)
        {
            Debug.Log("游戏结束");
            int leftScore = 0;
            foreach(GameObject player in leftPlayer)
            {
                leftScore += player.GetComponent<PlayerMovement>().myScore;
            }

            int rightScore = 0;
            foreach (GameObject player in rightPlayer)
            {
                rightScore += player.GetComponent<PlayerMovement>().myScore;
            }
            //if(playerManager.myplayer[playerManager.controlId].)
            int winSide;
            if (leftScore > rightScore)
            {
                winSide = 0;
            }
            else if(leftScore < rightScore)
            {
                winSide = 1;
            }
            else
            {
                winSide = controlSide;
            }
            if (winSide == controlSide)
            {
                SceneManager.LoadScene("end");
            }
            else
            {
                SceneManager.LoadScene("end2");
            }
        }
        //if(playerNow==0&& playerManager.myplayer.Length == 6)
        //{
        //    playerNow = 6;
        //    for(int i=0;i<6;i++)
        //    {
        //        if (i != playerManager.controlId)
        //        {
        ////可以在这里给角色附上网络脚本
        ////PlayerManager.myplayer[i].AddComponent<...>()移动接口在PlayerMovement组件中 射击接口在PlayerShooting组件中，可以直接看prefab
        //        }
        //    }
        //}
        OccupiedLeft = 0;
        OccupiedRight = 0;
        leftPlayer = new GameObject[] { playerManager.myplayer[0], playerManager.myplayer[1], playerManager.myplayer[2] };
        foreach (GameObject item in leftPlayer)
        {
            if (!item.GetComponent<PlayerSide>())
            {
                item.AddComponent<PlayerSide>();
                item.GetComponent<PlayerSide>().side = 0;
                item.GetComponent<PlayerSide>().color1 = leftColor;
            }
        }
        rightPlayer = new GameObject[] { playerManager.myplayer[3], playerManager.myplayer[4], playerManager.myplayer[5] };
        foreach (GameObject item in rightPlayer)
        {
            if (!item.GetComponent<PlayerSide>())
            {
                item.AddComponent<PlayerSide>();
                item.GetComponent<PlayerSide>().side = 1;
                item.GetComponent<PlayerSide>().color2 = rightColor;
            }
        }

        //Physics.OverlapSphere(Vector3 position, float radius, int layerMask)

        Collider[] colliders =  Physics.OverlapSphere(transform.position, SearchArea, 1<<LayerMask.NameToLayer("Player"));
        foreach(Collider co in colliders)
        {
            if (co.gameObject.GetComponent<PlayerSide>().side == 0)
            {
                OccupiedLeft++;
            }
            else if(co.gameObject.GetComponent<PlayerSide>().side==1)
            {
                OccupiedRight++;
            }

        }
        if (OccupiedLeft + OccupiedRight > 0&&OccupiedLeft!=OccupiedRight)//有人在里面
        {
            if (OccupiedLeft > OccupiedRight)
            {
                if (superiority != 0)
                {
                    superiority = 0;
                    fhz.ActiveFhz();
                    projector.material.SetColor("_Color", leftColor);
                    fhzMat.SetColor("_Emission", leftColor);
                    occupyTime.GetComponent<Shadow>().effectColor = leftColor;
                    occupyTime.gameObject.SetActive(true);
                    TimeRecord = 0;
                }
                else
                {
                    TimeRecord += Time.deltaTime;
                }
            }
            else
            {
                if (superiority != 1)
                {
                    superiority = 1;
                    fhz.ActiveFhz();
                    
                    projector.material.SetColor( "_Color",rightColor);
                    fhzMat.SetColor("_Emission", rightColor);
                    occupyTime.GetComponent<Shadow>().effectColor = rightColor;
                    
                    occupyTime.gameObject.SetActive(true);

                }
                else
                {
                    TimeRecord += Time.deltaTime;
                }
            }
        }
        else
        {
            superiority = -1;
            projector.material.SetColor("_Color", notOccupiedColor);
            fhzMat.SetColor("_Emission", notOccupiedColor2);
            TimeRecord = 0;
            occupyTime.gameObject.SetActive(false);
        }
        //Debug.Log("被占领了！" + superiority);
        occupyTime.text = ((int)(TimeRecord / 60)).ToString()+":"+((int)TimeRecord%60).ToString();
        totalTime.text = ((int)(gameTimeAll / 60)).ToString() + ":" + ((int)gameTimeAll % 60).ToString();
        if (TimeRecord > winTime)
        {
            Debug.Log("胜利了！" + superiority);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (playerManager.myplayer[playerManager.controlId].GetComponent<PlayerSide>().side == superiority)
            {
                SceneManager.LoadScene("end");
            }
            else
            {
                SceneManager.LoadScene("end2");
            }
        }
    }
}
