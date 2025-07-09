using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class IdleState : IState
{
    private PlayerController player;

    private int _speedParameterHash;
    private int _directionParameterHash;
    private float _currentSpeed;
    private Vector3 _lookDirection;

    private bool _isJumping;
    private float _jumpStartHeight;

    
    public IdleState(PlayerController player)
    {
        this.player = player;
    }


    public void Enter()
    {
        _speedParameterHash = Animator.StringToHash("speed");
        _directionParameterHash = Animator.StringToHash("direction");

    }

    // Update is called once per frame
    public void Update()
    {
        if (player.GroundCheck())
        {
            ProcessDirectionalInput();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.PlayerRigidbody.AddForce(Vector3.up * player.JumpForce + player.PlayerRigidbody.transform.forward * player.JumpForce * _currentSpeed * 0.3f, ForceMode.Impulse);
                _jumpStartHeight = player.PlayerRigidbody.position.y;
                _isJumping = true;
                
            }
        }
        else 
        {
            player.PlayerAnimator.SetFloat(_speedParameterHash, 0, player.WalkingAcceleration, Time.deltaTime);
            player.PlayerAnimator.SetFloat(_directionParameterHash, 0, player.WalkingAcceleration, Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                player.PlayerRigidbody.AddForce(Vector3.down * player.JumpForce * 0.5f, ForceMode.Impulse);
            }
        }
    }
    
    // Sets Animator floats based on verical and horizontal Input, Invoked by CharacterStateMachine Class if State is Idle
    // Rotates Rigidbody in Camera Direction (only when vertical or horizonatl Inputs are not 0, so the Player can circle around the Character when idling)
    void ProcessDirectionalInput()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        bool shouldRun = Input.GetKey(KeyCode.LeftControl);
        
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

        _currentSpeed = speed;

        player.PlayerAnimator.SetFloat(_speedParameterHash, (player.SpeedPowerUp && speed != 0) ? speed + 1 : speed, player.WalkingAcceleration, Time.deltaTime);
        player.PlayerAnimator.SetFloat(_directionParameterHash, direction, player.WalkingAcceleration, Time.deltaTime);

        Vector3 crossLookDirection = Quaternion.Euler(0, 90, 0) * _lookDirection;
        Vector3 movementDirection = _lookDirection * verticalInput + crossLookDirection * horizontalInput;

        Vector3 newDirection = Vector3.RotateTowards(player.PlayerTransform.forward, movementDirection, player.RotationSpeed * Time.deltaTime, 0.0f);
        player.PlayerRigidbody.MoveRotation(Quaternion.LookRotation(newDirection));

    }


    // Gets normalized look Direction of Camera, so this Gameobject can rotate in this Direction.
    void GetLookDirection()
    {
        _lookDirection = (player.PlayerTransform.position - new Vector3(player.PlayerCamera.transform.position.x, player.PlayerTransform.transform.position.y, player.PlayerCamera.transform.position.z)).normalized;
        
    }

    // Removing eventListeners
    public void Exit()
    {
        player.PlayerAnimator.SetFloat(_speedParameterHash, 0, player.WalkingAcceleration, Time.deltaTime);
        player.PlayerAnimator.SetFloat(_directionParameterHash, 0, player.WalkingAcceleration, Time.deltaTime);
    }

}
