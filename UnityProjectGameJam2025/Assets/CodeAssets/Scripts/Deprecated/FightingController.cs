using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterStateMachine))]
public class FightingController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private CharacterStateMachine _characterStateMachine;

    private float _locomotionDampingParameter = 0.15f;    
    private int _speedParameterHash;
    private int _directionParameterHash;
    private Vector3 _lookDirection;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Sets Animator floats based on verical and horizontal Input, Invoked by CharacterStateMachine Class if State is Fighting
    // Rotates Rigidbody in Camera Direction
    public void ProcessDirectionalInput(float verticalInput, float horizontalInput, bool shouldRun)
    {
        float direction = horizontalInput;
        float speed = verticalInput;

        if (shouldRun)
        {
            speed *= 2;
        }

        GetLookDirection();

        _animator.SetFloat(_speedParameterHash, speed, _locomotionDampingParameter, Time.deltaTime);
        _animator.SetFloat(_directionParameterHash, direction, _locomotionDampingParameter, Time.deltaTime);

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, _lookDirection, 20f* Time.deltaTime, 0.0f);
        _rigidbody.MoveRotation(Quaternion.LookRotation(newDirection));
    }

    // Gets normalized look Direction of Camera, so this Gameobject can rotate in this Direction.
    void GetLookDirection()
    {
        _lookDirection = (transform.position - new Vector3(_camera.transform.position.x, transform.position.y, _camera.transform.position.z)).normalized;
        
    }

    public void LeftPunch()
    {
        _animator.SetTrigger("Punch Left");
    }

    public void RightPunch()
    {
        _animator.SetTrigger("Punch Right");
    }

    public void LeftKick()
    {
        _animator.SetTrigger("LeftKick");
    }

    public void RightKick()
    {
        _animator.SetTrigger("RightKick");
    }
    public void Headbutt()
    {
        _animator.SetTrigger("Headbutt");
    }
    
    // Removing eventListeners
    void OnDestroy()
    {
    }
}
