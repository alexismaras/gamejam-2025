using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;

    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _jumpForceMultiplier;
    [SerializeField] private bool _canJump;
    [SerializeField] private bool _canRun;

    private bool _isGrounded;
    private Rigidbody _playerRigidbody;
    private Vector3 _lookDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        float direction = horizontalInput;
        float speed = Mathf.Max(Mathf.Abs(verticalInput), Mathf.Abs(horizontalInput));
        bool shouldRun = Input.GetKey(KeyCode.LeftControl);
        bool shouldJump = Input.GetKeyDown(KeyCode.Space);


        // RUNNING MULTIPLIER
        if (shouldRun && _canRun)
        {
            speed *= 2;
        }

        // JUMPING FORCE
        if (shouldJump && _canJump && _isGrounded)
        {

            Vector3 jumpDir = Vector3.up;
            Vector3 jumpForce = jumpDir * _jumpForceMultiplier;
            _playerRigidbody.AddForce(jumpForce);
        }

        if (speed != 0)
        {
            GetLookDirection();
        }

        _playerRigidbody.MovePosition(transform.position + transform.forward * speed * _speedMultiplier * Time.deltaTime);

        Vector3 crossLookDirection = Quaternion.Euler(0, 90, 0) * _lookDirection;
        Vector3 movementDirection = _lookDirection * verticalInput + crossLookDirection * horizontalInput;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, movementDirection, 20f * Time.deltaTime, 0.0f);
        _playerRigidbody.MoveRotation(Quaternion.LookRotation(newDirection));
    }

    void GetLookDirection()
    {
        _lookDirection = (transform.position - new Vector3(_playerCamera.transform.position.x, transform.transform.position.y, _playerCamera.transform.position.z)).normalized;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
       if (collision.gameObject.tag == "Floor")
        {
            _isGrounded = false;
        } 
    }
}
