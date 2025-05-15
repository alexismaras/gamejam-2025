using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterStateMachine))]
public class IdleController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private CharacterStateMachine _characterStateMachine;

    private float _locomotionDampingParameter = 0.15f;

    private int _speedParameterHash;
    private int _directionParameterHash;
    private Vector3 _lookDirection;

    


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _characterStateMachine = GetComponent<CharacterStateMachine>();

        _speedParameterHash = Animator.StringToHash("speed");
        _directionParameterHash = Animator.StringToHash("direction");
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }
    
    // Sets Animator floats based on verical and horizontal Input, Invoked by CharacterStateMachine Class if State is Idle
    // Rotates Rigidbody in Camera Direction (only when vertical or horizonatl Inputs are not 0, so the Player can circle around the Character when idling)
    public void ProcessDirectionalInput(float verticalInput, float horizontalInput, bool shouldRun)
    {
        float direction = horizontalInput;
        float speed = Mathf.Max(Mathf.Abs(verticalInput), Mathf.Abs(horizontalInput));

        if (shouldRun)
        {
            speed *= 2;
        }

        if (speed != 0)
        {
            GetLookDirection();
        }

        _animator.SetFloat(_speedParameterHash, speed, _locomotionDampingParameter, Time.deltaTime);
        _animator.SetFloat(_directionParameterHash, direction, _locomotionDampingParameter, Time.deltaTime);

        Vector3 crossLookDirection = Quaternion.Euler(0, 90, 0) * _lookDirection;
        Vector3 movementDirection = _lookDirection * verticalInput + crossLookDirection * horizontalInput;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, movementDirection, 20f* Time.deltaTime, 0.0f);
        _rigidbody.MoveRotation(Quaternion.LookRotation(newDirection));

    }

    // Gets Called by CharacterStateMachine
    // Determines wheter running-jump-animation or standing-jump-animation is played, based on animators speed param
    // NOTE: I should probably outsource that logic to the animator
    // NOTE: Currently there is only a standing-jump and running-jump animation but no walking-jump animation... 
    //       additionally, the jump animations root motion in y direction are baked into pose so jumping does nothing than looking like jumping
    public void Jump()
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

    // Gets normalized look Direction of Camera, so this Gameobject can rotate in this Direction.
    void GetLookDirection()
    {
        _lookDirection = (transform.position - new Vector3(_camera.transform.position.x, transform.position.y, _camera.transform.position.z)).normalized;
        
    }

    // Removing eventListeners
    void OnDestroy()
    {
    }

}
