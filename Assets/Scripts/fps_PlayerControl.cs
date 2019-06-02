using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    None,
    Idle,
    Walk,
    Crouch,
    Run,
}

public class fps_PlayerControl : MonoBehaviour
{
    private PlayerState state = PlayerState.None;
    public PlayerState State
    {
        get
        {
            if (running)
                state = PlayerState.Run;
            else if (walking)
                state = PlayerState.Walk;
            else if (crouching)
                state = PlayerState.Crouch;
            else
                state = PlayerState.Idle;
            return state;
        }
    }


    public float sprintSpeed = 10.0f;
    public float sprintJumpSpeed = 8.0f;
    public float normalSpeed = 6.0f;
    public float normalJumpSpeed = 7.0f;
    public float crouchSpeed = 2.0f;
    public float crouchJumpSpeed = 5.0f;
    public float crouchDeltaHeight = 0.5f;

    public float gravity = 20.0f;
    public float cameraMoveSpeed = 8.0f;
    public AudioClip JumpAudio;

    private float speed;
    private float JumpSpeed;
    private Transform mainCamera;

    private float standardCamHeight;
    private float crouchingCamHeight;
    private bool grounded = false;
    private bool walking = false;
    private bool crouching = false;
    private bool stopCrouching = false;
    private bool running = false;
    private Vector3 normalControllerCenter = Vector3.zero;
    private float normalControlerHeight = 0.0f;
    private float timer = 0;
    private CharacterController controller;
    private AudioSource audioSource;
    private fps_PlayerParameter parameter;

    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        crouching = false;
        walking = false;
        running = false;
        speed = normalSpeed;
        JumpSpeed = normalJumpSpeed;
        mainCamera = GameObject.FindGameObjectWithTag(Tags.mainCamera).transform;
        standardCamHeight = mainCamera.localPosition.y;
        crouchingCamHeight = standardCamHeight - crouchDeltaHeight;
        audioSource = this.GetComponent<AudioSource>();
        controller = this.GetComponent<CharacterController>();
        parameter = this.GetComponent<fps_PlayerParameter>();
        normalControllerCenter = controller.center;
        normalControlerHeight = controller.height;
           
    }

    private void FixedUpdate()
    {
        UpdateMove();
        AudioManagement();
    }

    private void UpdateMove()
    {
        //着地
        if(grounded)
        {
            
            moveDirection = new Vector3(parameter.inputMoveVector.x, 0, parameter.inputMoveVector.y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            //Debug.Log(parameter.inputJump);
            if(parameter.inputJump)
            {
                moveDirection.y = JumpSpeed;
                AudioSource.PlayClipAtPoint(JumpAudio, transform.position);
                CurrentSpeed();
            }
        }

        //受重力影响
        moveDirection.y -= gravity * Time.deltaTime;

        //判断是否落地
        CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime);
        grounded = (flags & CollisionFlags.CollidedBelow) != 0;

        if(Mathf.Abs(moveDirection.x) > 0 && grounded || Mathf.Abs(moveDirection.z) > 0 && grounded)
        {
            if(parameter.inputSprint)
            {
                walking = false;
                running = true;
                crouching = false;
            }
            else if(parameter.inputCrouch)
            {
                crouching = true;
                walking = false;
                running = false;
            }
            else
            {
                walking = true;
                crouching = false;
                running = false;
            }
        }
        else
        {
            walking = false;
            running = false;
            if (parameter.inputCrouch)
                crouching = true;
            else
                crouching = false;
        }

        if(crouching)
        {
            controller.height = normalControlerHeight - crouchDeltaHeight;
            controller.center = normalControllerCenter - new Vector3(0, crouchDeltaHeight / 2, 0);
        }
        else
        {
            controller.height = normalControlerHeight;
            controller.center = normalControllerCenter;
        }
        UpdateCrouch();
        CurrentSpeed();

    }

    //对状态和速度的管理
    private void CurrentSpeed()
    {
        switch(State)
        {
            case PlayerState.Idle:
                speed = normalSpeed;
                JumpSpeed = normalJumpSpeed;
                break;
            case PlayerState.Walk:
                speed = normalSpeed;
                JumpSpeed = normalJumpSpeed;
                break;
            case PlayerState.Crouch:
                speed = crouchSpeed;
                JumpSpeed = crouchJumpSpeed;
                break;
            case PlayerState.Run:
                speed = sprintSpeed;
                JumpSpeed = sprintJumpSpeed;
                break;
        }
    }

    private void AudioManagement()
    {
        if (State == PlayerState.Walk)
        {
            audioSource.pitch = 1.0f;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else if (State == PlayerState.Run)
        {
            audioSource.pitch = 1.3f;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
            audioSource.Stop();
    }

    private void UpdateCrouch()
    {
        if(crouching)
        {
            if(mainCamera.localPosition.y > crouchingCamHeight)
            {
                if (mainCamera.localPosition.y - crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed < crouchingCamHeight)
                {
                    mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, crouchingCamHeight, mainCamera.localPosition.z);
                }
                else
                    mainCamera.localPosition -= new Vector3(0, crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed, 0);
            }
            else
                mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, crouchingCamHeight, mainCamera.localPosition.z);
        }
        else
        {
            if(mainCamera.localPosition.y < standardCamHeight)
            {
                if (mainCamera.localPosition.y + (crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed) > standardCamHeight)
                    mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, standardCamHeight, mainCamera.localPosition.z);
                else
                    mainCamera.localPosition += new Vector3(0, crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed, 0);
            }
            else
                mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, standardCamHeight, mainCamera.localPosition.z);
        }
    }
}
