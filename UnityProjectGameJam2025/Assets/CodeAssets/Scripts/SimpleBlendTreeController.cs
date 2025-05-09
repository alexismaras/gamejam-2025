using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Controll a blend tree
/// </summary>
public class SimpleBlendTreeController : MonoBehaviour
{
    // Defines how fast the character will move

    public float LocomotionDampingParameter = 0.15f;

    // Animator playing animations
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Collider _bodyCollider;

    // Hash speed parameter
    private int _speedParameterHash;
    private int _directionParameterHash;


    private Vector3 _lookDirection;

    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerStateMachine _playerStateMachine;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _bodyCollider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        _speedParameterHash = Animator.StringToHash("speed");
        _directionParameterHash = Animator.StringToHash("direction");

        PlayerStateMachine.OnLeftPunch += HandleLeftPunch;
        PlayerStateMachine.OnRightPunch += HandleRightPunch;
        PlayerStateMachine.OnHeadbutt += HandleHeadbutt;
        PlayerStateMachine.OnJump += HandleJump;
        PlayerStateMachine.OnLeftKick += HandleLeftKick;
        PlayerStateMachine.OnRightKick += HandleRightKick;
    }

    // Update is called once per frame
    void Update()
    {

        // Stores inputs
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        bool shouldRun = Input.GetKey(KeyCode.LeftControl);

        float direction = horizontalInput;

        

        

        if (_playerStateMachine.CurrentPlayerState == PlayerStateMachine.PlayerState.Idle)
        {
            float speed = Mathf.Max(Mathf.Abs(verticalInput), Mathf.Abs(horizontalInput));

            if (shouldRun)
            {
                speed *= 2;
            }

            if (speed != 0)
            {
                GetLookDirection();
            }

            _animator.SetFloat(_speedParameterHash, speed, LocomotionDampingParameter, Time.deltaTime);
            _animator.SetFloat(_directionParameterHash, direction, LocomotionDampingParameter, Time.deltaTime);

            Vector3 crossLookDirection = Quaternion.Euler(0, 90, 0) * _lookDirection;
            Vector3 movementDirection = _lookDirection * verticalInput + crossLookDirection * horizontalInput;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, movementDirection, 20f* Time.deltaTime, 0.0f);
            _rigidbody.MoveRotation(Quaternion.LookRotation(newDirection));
        }

        if (_playerStateMachine.CurrentPlayerState == PlayerStateMachine.PlayerState.Fighting)
        {
            float speed = verticalInput;

            if (shouldRun)
            {
                speed *= 2;
            }

            GetLookDirection();

            _animator.SetFloat(_speedParameterHash, speed, LocomotionDampingParameter, Time.deltaTime);
            _animator.SetFloat(_directionParameterHash, direction, LocomotionDampingParameter, Time.deltaTime);

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, _lookDirection, 20f* Time.deltaTime, 0.0f);
            _rigidbody.MoveRotation(Quaternion.LookRotation(newDirection));
        }

        

        
    }

    void HandleLeftPunch()
    {
        _animator.SetTrigger("Punch Left");
    }

    void HandleRightPunch()
    {
        _animator.SetTrigger("Punch Right");
    }

    void HandleLeftKick()
    {
        _animator.SetTrigger("LeftKick");
    }

    void HandleRightKick()
    {
        _animator.SetTrigger("RightKick");
    }
    void HandleHeadbutt()
    {
        _animator.SetTrigger("Headbutt");
    }

    void HandleJump()
    {
        if (_animator.GetFloat("speed") <= 1)
        {
            _animator.SetTrigger("Idle Jump");
        }
        else 
        {
            _animator.SetTrigger("Running Jump");
        }
    }

    void GetLookDirection()
    {
        _lookDirection = (transform.position - new Vector3(_camera.transform.position.x, transform.position.y, _camera.transform.position.z)).normalized;
        
    }

    void OnAnimatorMove()
    {
        _rigidbody.MovePosition(_rigidbody.position + _animator.deltaPosition);
        _rigidbody.MoveRotation(_rigidbody.rotation * _animator.deltaRotation);
    }

}
