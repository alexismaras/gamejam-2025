using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class IdleState : IState
{
    private PlayerController player;
    private float _locomotionDampingParameter = 0.15f;

    private int _speedParameterHash;
    private int _directionParameterHash;
    private Vector3 _lookDirection;

    
    public IdleState(PlayerController player) 
    {
        this.player = player;
    }


    public void Enter()
    {
        _speedParameterHash = Animator.StringToHash("speed");
        _directionParameterHash = Animator.StringToHash("direction");

        player.PlayerAnimator.SetBool("isIdleState", true);
        player.PlayerAnimator.SetBool("isFightingState", false);
        player.PlayerAnimator.SetBool("isClimbingState", false);
    }

    // Update is called once per frame
    public void Update()
    {
        
        ProcessDirectionalInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            player.ChangeState(new FightState(player));
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

        player.PlayerAnimator.SetFloat(_speedParameterHash, speed, _locomotionDampingParameter, Time.deltaTime);
        player.PlayerAnimator.SetFloat(_directionParameterHash, direction, _locomotionDampingParameter, Time.deltaTime);

        Vector3 crossLookDirection = Quaternion.Euler(0, 90, 0) * _lookDirection;
        Vector3 movementDirection = _lookDirection * verticalInput + crossLookDirection * horizontalInput;

        Vector3 newDirection = Vector3.RotateTowards(player.PlayerTransform.forward, movementDirection, 20f* Time.deltaTime, 0.0f);
        player.PlayerRigidbody.MoveRotation(Quaternion.LookRotation(newDirection));

    }

    // Gets Called by CharacterStateMachine
    // Determines wheter running-jump-animation or standing-jump-animation is played, based on animators speed param
    // NOTE: I should probably outsource that logic to the animator
    // NOTE: Currently there is only a standing-jump and running-jump animation but no walking-jump animation... 
    //       additionally, the jump animations root motion in y direction are baked into pose so jumping does nothing than looking like jumping
    void Jump()
    {
        if (player.PlayerWallHit.collider == null)
        {
            if (player.PlayerAnimator.GetFloat(_speedParameterHash) <= 1)
            {
                player.PlayerRigidbody.useGravity = false;
                player.PlayerAnimator.Play("Idle Jump");
            }
            else 
            {
                player.PlayerRigidbody.useGravity = false;
                player.PlayerAnimator.Play("Running Jump");
            }
        }
        else
        {
            player.ChangeState(new ClimbState(player));
        }
    }

    // Gets normalized look Direction of Camera, so this Gameobject can rotate in this Direction.
    void GetLookDirection()
    {
        _lookDirection = (player.PlayerTransform.position - new Vector3(player.PlayerCamera.transform.position.x, player.PlayerTransform.transform.position.y, player.PlayerCamera.transform.position.z)).normalized;
        
    }

    // Removing eventListeners
    public void Exit()
    {
    }

}
