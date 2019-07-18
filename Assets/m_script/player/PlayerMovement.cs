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
    public GameObject gunHead;
    PlayerShooting shot;
    public int myScore=0;
    public changeMesh changerole;
    int run = 0;
    int jump = 0;
    int count = 0;
    public Vector3 startpoint;
    public float jumpSpeed=5f;

    //rotation Sensitivity
    private float lookSensitivity = 5.0f;
    //camera
    [SerializeField]
    private Camera Cam;
    
    void Awake()//无论脚本是否可运行都会执行，适合用于设置初始值
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        floorMask = LayerMask.GetMask("Floor");//传入我们要获取的layer对应的字符串
        anim = GetComponent<Animator>();
        shot = gunhead.GetComponent<PlayerShooting>();
        playerRigidbody = GetComponent<Rigidbody>();
        ph = GetComponent<PlayerHealth>();
        changerole = akai.GetComponent<changeMesh>();
        
        /* Character maoxianzhe = (Character)AssetDatabase.LoadAssetAtPath<Character>("Assets/CharacterAsset/Shengtu1.asset");
         Mesh newMesh =maoxianzhe.fbx.GetComponent<Mesh>();
         SkinnedMeshRenderer meshrender = GetComponent<SkinnedMeshRenderer>();
         meshrender.sharedMesh=newMesh;*/
        Cursor.visible = false;//隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;//把鼠标锁定到屏幕中间


    }
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "addHP")
        {
            other.gameObject.SetActive(false);
            if (ph.startingHealth - ph.currentHealth > 30)
            {
                ph.currentHealth = ph.currentHealth + 30;
            }
            else
            {
                ph.currentHealth = ph.startingHealth;
            }
            
        }
       
        else if (other.tag == "addDamage"&&shot.addDamage==false)
        {
            other.gameObject.SetActive(false);
            shot.damagePerShot = 50;
            shot.addDamage=true;
        }
    }
    public int getID()
    {
        return playerID;
    }
    void Update()//固定时间间隔被调用，跟物理引擎一起被更新
    {
        float h = Input.GetAxisRaw("Horizontal");//从横轴获取输入,只有0，-1，1，可以认为是一个方向
        float v = Input.GetAxisRaw("Vertical");//从纵轴获取输入
        Move(h*5, v*5);
        //Turning();//要删
        //transform.forward = cameratranform.forward;
        animating(h, v);
        positionRest();
        //Rotate
        float _yRot = Input.GetAxis("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        //Apply Rotation
        PerformRotate(_rotation);
        //Calculate camera rotation as a 3D vector (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;

        PerformCamRotate(_cameraRotation);

    }

    private void PerformRotate(Vector3 _rotation)
    {
        transform.rotation = transform.rotation * Quaternion.Euler(_rotation);
        
    }

    private void PerformCamRotate(Vector3 _rotation)
    {
        if(null != Cam)
        {
            Cam.transform.Rotate(-_rotation);
        }
        Vector3 offset_Y = new Vector3(0.0f, 3.0f, 0.0f);
        Cam.transform.position = akai.transform.position - 3.0f * Cam.transform.forward.normalized + offset_Y;

    }
    

    //void FixedUpdate()//固定时间间隔被调用，跟物理引擎一起被更新
    //{
    //    float h = Input.GetAxisRaw("Horizontal");//从横轴获取输入,只有0，-1，1，可以认为是一个方向
    //    float v = Input.GetAxisRaw("Vertical");//从纵轴获取输入
    //    Move(h*5, v*5);
    //    //Turning();//要删
    //    //transform.forward = cameratranform.forward;
    //    animating(h, v);
    //}
    public void Move(float h,float v)
    {
        animating(h, v);
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
        //Debug.Log(Input.mousePosition);
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
    void positionRest()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
           transform.position = startpoint;

        }
    }
    void animating(float h, float v)//玩家的状态取决于输入
    {
        //bool walking=h!=0f || v != 0f;
        //anim.SetBool("IsWalking",walking);
        
        if (Input.GetKeyUp(KeyCode.E))
        {

            count++;
            Debug.Log("ccc" + count);
            if (count % 2 == 1)
            {
                run = 1;
                speed = 5;
            }
            else if (count % 2 == 0)
            {
                run = 0;
                speed = 2;
            }
            

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("jump");
            
            playerRigidbody.velocity += new Vector3(0, 20, 0);
            playerRigidbody.AddForce(Vector3.up * jumpSpeed);
            jump = 1;
            
            
        }
        

            if (h > 0 && v == 0)
        {
            if(run==0)
                anim.SetInteger("state", 3);
            if (run == 1)
                anim.SetInteger("state", 5);
            if (jump == 1)

            {
                anim.SetTrigger("jump");
                jump = 0;
            }
             
           
        }
        else if (h < 0 && v == 0)
        {
            if (run == 0 )
                anim.SetInteger("state", 2);
            if (run == 1)
                anim.SetInteger("state", 5);
            if (jump == 1)
            {
                anim.SetTrigger("jump");
                jump = 0;
            }

        }
        else if (h == 0 && v > 0)
        {
            if (run == 0 && jump == 0)
                anim.SetInteger("state", 1);
            if (run == 1)
                anim.SetInteger("state", 5);
            if (jump == 1)
            {
                anim.SetTrigger("jump");
                jump = 0;
            }

        }
        else if (h == 0 && v < 0)
        {
            if (run == 0 && jump == 0)
                anim.SetInteger("state", 4);
            if (run == 1)
                anim.SetInteger("state", 5);
            if (jump == 1)
            {
                anim.SetTrigger("jump");
                jump = 0;
            }

        }
        else if (h == 0 && v == 0 && ph.currentHealth > 0)
        {
           
           anim.SetInteger("state", 0);
            
            if (jump == 1)
            {
                anim.SetTrigger("jump");
                jump = 0;
            }
        }
        

    }


} 
 