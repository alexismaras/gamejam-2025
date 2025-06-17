using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
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
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        float direction = horizontalInput;
        float speed = Mathf.Max(Mathf.Abs(verticalInput), Mathf.Abs(horizontalInput));
        bool shouldRun = Input.GetKey(KeyCode.LeftControl);

        if (shouldRun)
        {
            speed *= 2;
        }

        if (speed != 0)
        {
            GetLookDirection();
        }

        Vector3 crossLookDirection = Quaternion.Euler(0, 90, 0) * _lookDirection;
        Vector3 movementDirection = _lookDirection * verticalInput + crossLookDirection * horizontalInput;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, movementDirection, 20f * Time.deltaTime, 0.0f);
        _playerRigidbody.MoveRotation(Quaternion.LookRotation(newDirection));
    }
    
    void GetLookDirection()
    {
        _lookDirection = (transform.position - new Vector3(_playerCamera.transform.position.x, transform.transform.position.y, _playerCamera.transform.position.z)).normalized;
        
    }
}
