using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithCamera : MonoBehaviour
{
    [SerializeField]
    private Camera Cam;

    private void FixedUpdate()
    {
        transform.rotation = Cam.transform.rotation;
    }
}
