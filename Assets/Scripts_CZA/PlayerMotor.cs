using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    private Vector3 velocity_Jump = Vector3.zero;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Get a velocity vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    //Get a rotational vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    //Get a camera rotational vector
    public void cameraRotate(Vector3 _cameratation)
    {
        cameraRotation = _cameratation;
    }

    public void Jump()
    {
        rb.velocity += new Vector3(0, 3, 0);
    }
    //Run every physics iterations
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //Perform movement based on velocity variable
    void PerformMovement()
    {
        if(Vector3.zero != velocity)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    //Perform rotation
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (null != cam)
        {
            cam.transform.Rotate(-cameraRotation);
        }
    }
    

}
