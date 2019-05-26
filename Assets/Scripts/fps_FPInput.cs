using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_FPInput : MonoBehaviour
{
    public bool LockCursor
    {
        get { return Cursor.lockState == CursorLockMode.Locked ? true : false; }
        set
        {
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;

        }
    }

    private fps_PlayerParameter parameter;
    private fps_Input input;


    void Start()
    {
        LockCursor = true;
        parameter = this.GetComponent<fps_PlayerParameter>();
        input = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<fps_Input>();

    }

    void Update()
    {
        InitialInput();
    }

    private void InitialInput()
    {
        parameter.inputMoveVector = new Vector2(input.GetAxis("Horizontal"), input.GetAxis("Vertical"));
        parameter.inputSmoothLook = new Vector2(input.GetAxisRaw("Mouse X"), input.GetAxisRaw("Mouse Y"));
        parameter.inputCrouch = input.GetButton("Crouch");
        parameter.inputJump = input.GetButton("Jump");
        parameter.inputSprint = input.GetButton("Sprint");
        parameter.inputFire = input.GetButton("Fire");
        parameter.inputReload = input.GetButtonDown("Reload");

    }
}
