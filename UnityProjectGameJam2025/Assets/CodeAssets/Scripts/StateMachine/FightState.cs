using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FightState : IState
{
    private PlayerController player;

    private float _locomotionDampingParameter = 0.15f;    
    private int _speedParameterHash;
    private int _directionParameterHash;
    private Vector3 _lookDirection;

    private float _elapsedTimeSincePunch = 0;

    public FightState(PlayerController player) 
    {
        this.player = player;
    }
    
    public void Enter()
    {
        _speedParameterHash = Animator.StringToHash("speed");
        _directionParameterHash = Animator.StringToHash("direction");

        player.PlayerAnimator.SetBool("isIdleState", false);
        player.PlayerAnimator.SetBool("isFightingState", true);
        player.PlayerAnimator.SetBool("isClimbingState", false);
    }

    // Update is called once per frame
    public void Update()
    {
        _elapsedTimeSincePunch += Time.deltaTime;

        if (_elapsedTimeSincePunch >= 3)
        {
            player.ChangeState(new IdleState(player));
        }

        ProcessDirectionalInput();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            LeftPunch();
            _elapsedTimeSincePunch = 0;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RightPunch();
            _elapsedTimeSincePunch = 0;
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Headbutt();
            _elapsedTimeSincePunch = 0;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            LeftKick();
            _elapsedTimeSincePunch = 0;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            RightKick();
            _elapsedTimeSincePunch = 0;
        }

    }

    // Sets Animator floats based on verical and horizontal Input, Invoked by CharacterStateMachine Class if State is Fighting
    // Rotates Rigidbody in Camera Direction
    public void ProcessDirectionalInput()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        bool shouldRun = Input.GetKey(KeyCode.LeftControl);

        float direction = horizontalInput;
        float speed = verticalInput;

        if (shouldRun)
        {
            speed *= 2;
        }

        GetLookDirection();

        player.PlayerAnimator.SetFloat(_speedParameterHash, speed, _locomotionDampingParameter, Time.deltaTime);
        player.PlayerAnimator.SetFloat(_directionParameterHash, direction, _locomotionDampingParameter, Time.deltaTime);

        Vector3 newDirection = Vector3.RotateTowards(player.PlayerTransform.forward, _lookDirection, 20f* Time.deltaTime, 0.0f);
        player.PlayerRigidbody.MoveRotation(Quaternion.LookRotation(newDirection));
    }

    // Gets normalized look Direction of Camera, so this Gameobject can rotate in this Direction.
    void GetLookDirection()
    {
        _lookDirection = (player.PlayerTransform.position - new Vector3(player.PlayerCamera.transform.position.x, player.PlayerTransform.position.y, player.PlayerCamera.transform.position.z)).normalized;
        
    }

    public void LeftPunch()
    {
        player.PlayerAnimator.SetTrigger("Punch Left");
    }

    public void RightPunch()
    {
        player.PlayerAnimator.SetTrigger("Punch Right");
    }

    public void LeftKick()
    {
        player.PlayerAnimator.SetTrigger("LeftKick");
    }

    public void RightKick()
    {
        player.PlayerAnimator.SetTrigger("RightKick");
    }
    public void Headbutt()
    {
        player.PlayerAnimator.SetTrigger("Headbutt");
    }
    
    // Removing eventListeners
    public void Exit()
    {
        _elapsedTimeSincePunch = 0;
    }
}
