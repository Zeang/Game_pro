using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class fps_PlayerParameter : MonoBehaviour
{
    [HideInInspector]
    public Vector2 inputSmoothLook;
    [HideInInspector]
    public Vector2 inputMoveVector;
    [HideInInspector]
    public bool inputCrouch;
    [HideInInspector]
    public bool inputJump;
    [HideInInspector]
    public bool inputSprint;
    [HideInInspector]
    public bool inputFire;
    [HideInInspector]
    public bool inputReload;

}
