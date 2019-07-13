using UnityEngine;
using UnityEditor;

public class PlayerMovement : MonoBehaviour
{
    public Transform cameratranform;
    public float speed = 6f;
    Vector3 movement;//存储移动信息并加在玩家身上
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;//存储地板，使ray只照射在地板上
    float camRayLength=100f;//存储摄像机射线的长度
    PlayerHealth ph;
    public Transform[] spawnPoints;
    public int playerID;
    Character maoxianzhe;
    public GameObject gunhead;
    public GameObject cm;
    public GameObject akai;
    public changeMesh changerole;
    void Awake()//无论脚本是否可运行都会执行，适合用于设置初始值
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        floorMask = LayerMask.GetMask("Floor");//传入我们要获取的layer对应的字符串
        anim = GetComponent<Animator>();
    
        playerRigidbody = GetComponent<Rigidbody>();
        ph = GetComponent<PlayerHealth>();
        changerole = akai.GetComponent<changeMesh>();
       /* Character maoxianzhe = (Character)AssetDatabase.LoadAssetAtPath<Character>("Assets/CharacterAsset/Shengtu1.asset");
        Mesh newMesh =maoxianzhe.fbx.GetComponent<Mesh>();
        SkinnedMeshRenderer meshrender = GetComponent<SkinnedMeshRenderer>();
        meshrender.sharedMesh=newMesh;*/
       

    }
  public int getID()
    {
        return 1;
    }
    void FixedUpdate()//固定时间间隔被调用，跟物理引擎一起被更新
    {
        float h = Input.GetAxisRaw("Horizontal");//从横轴获取输入,只有0，-1，1，可以认为是一个方向
        float v = Input.GetAxisRaw("Vertical");//从纵轴获取输入
        Move(h*5, v*5);
        //Turning();//要删
        //transform.forward = cameratranform.forward;
        animating(h, v);
    }
    public void Move(float h,float v)
    {
        //movement.Set(h, 0f, v);//x,y,z不需要y方向
        //movement = movement.normalized * speed * Time.deltaTime;//为了使每个方向跑的一样快，为了使每秒移动6个单元
        // playerRigidbody.MovePosition(transform.position+movement);//会将刚体移动到世界空间的某个坐标，所以需要把它与角色当前所在的坐标关连
        Vector3 targetDirection = new Vector3(h, 0, v);
        float y = Camera.main.transform.rotation.eulerAngles.y;
        targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;

        transform.Translate(targetDirection * Time.deltaTime * speed, Space.World);


    }
    void Turning()//不需要参数，因为角色的朝向是基于鼠标的位置，而不是我们保存的输入
    {
        Debug.Log(Input.mousePosition);
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);//从camera发射一条射线打在鼠标位置上(如果这行报错，可能是camera没有设置tag为main
        RaycastHit floorHit;
        if(Physics.Raycast(camRay,out floorHit, camRayLength, floorMask))//如果击中某物
        {
            Debug.Log("enter");
            Vector3 playerToMouse = floorHit.point - transform.position;//要取负
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }

    }
    void animating(float h,float v)//玩家的状态取决于输入
    {
        //bool walking=h!=0f || v != 0f;
        //anim.SetBool("IsWalking",walking);
        if (h > 0 && v == 0)
        {
            anim.SetInteger("state", 3);
        }
        else if (h < 0 && v == 0)
        {
            anim.SetInteger("state", 2);
        }
        else if (h == 0 && v > 0)
        {
            anim.SetInteger("state", 1);
        }
        else if (h == 0 && v < 0)
        {
            anim.SetInteger("state", 4);
        }
        else if(h==0&&v==0&&ph.currentHealth>0)
        {
            anim.SetInteger("state", 0);
        }

    }


} 
 