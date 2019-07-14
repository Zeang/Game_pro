using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private PlayerMotor motor;

    private bool state = true;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        //Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");
        Vector3 _movHorizontal = transform.right * _xMov;   //transform.right = (1, 0, 0)
        Vector3 _movVertical = transform.forward * _zMov;   //transform.forward = (0, 0, 1)

        //Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

        //Apply movement
        motor.Move(_velocity);

        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
        
        //Apply Rotation
        motor.Rotate(_rotation);

        //Calculate camera rotation as a 3D vector (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;

        motor.cameraRotate(_cameraRotation);

        //Jump
        float _yMov = Input.GetAxisRaw("Jump");
        if (true == state && 1 == _yMov)
        {
            state = false;
            motor.Jump();
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if( "Floor" == collision.gameObject.tag)
        {
            state = true;
        }
    }
}
