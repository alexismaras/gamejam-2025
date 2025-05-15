using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ClimbState : IState
{
    

    private PlayerController player;


    private bool _performingClimb;

    private float _climbWallDistance = 0.5f;
    private float _locomotionDampingParameter = 0.15f;
    private int _climbDirXParameterHash;
    private int _climbDirYParameterHash;

    public ClimbState(PlayerController player) 
    {
        this.player = player;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Enter()
    {
        _climbDirXParameterHash = Animator.StringToHash("climbDirX");
        _climbDirYParameterHash = Animator.StringToHash("climbDirY");

        player.PlayerAnimator.SetBool("isIdleState", false);
        player.PlayerAnimator.SetBool("isFightingState", false);
        player.PlayerAnimator.SetBool("isClimbingState", true);

        StartClimb();
    }

    // Constantly detect if a wall is in front of the gameObject and how tall it is.
    public void Update()
    { 
        if (_performingClimb)
        {
            ProcessDirectionalInput();

            if(player.PlayerWallHit.collider == null)
            {
                player.PlayerAnimator.SetBool("wallEnd", true);
                EndClimb();
            }
        }
        
    }

    // Sets Animator floats based on verical and horizontal Input, Invoked by CharacterStateMachine Class if State is Climbing
    void ProcessDirectionalInput()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        bool shouldRun = Input.GetKey(KeyCode.LeftControl);

        player.PlayerAnimator.SetFloat(_climbDirYParameterHash, verticalInput, _locomotionDampingParameter, Time.deltaTime);
        player.PlayerAnimator.SetFloat(_climbDirXParameterHash, horizontalInput, _locomotionDampingParameter, Time.deltaTime);

    }

    // Gets a detected wall's normal direction and flips it -> (apparently hit.normal works like a laserbeam being reflected on the wall so it always points away from the wall, therefore we flip it)
    // Return gets casted to Quaternion because its need for rotating the rigidbody
    Quaternion GetWallRotation(RaycastHit hit)
    {
        Vector3 wallNormal = hit.normal;

        wallNormal *= -1;

        return Quaternion.LookRotation(wallNormal);
    }
    

    // Called by CharacterStateMachine when pressing Space
    // If Detected Wall is not minimum _minClimbHeight, the Method returns false so CharacterStateMachine can turn the Space Input into a regular Jump
    // If wall is high enough:
    // Prepare Rigidboyd Constraints
    // Rotate gameObjects rigidbody in wall direction and adjust the position so the character does not glitch through the wall
    // Tell Animator that we are climbing
    // Set global _performingClimb bool true
    void StartClimb()
    {
        player.PlayerRigidbody.useGravity = false;
        player.PlayerRigidbody.excludeLayers = player.ClimbableLayer.value;
        player.PlayerRigidbody.MoveRotation(GetWallRotation(player.PlayerWallHit));

        Vector3 climbPos = new Vector3
        (
            player.PlayerWallHit.point.x + player.PlayerWallHit.normal.x * _climbWallDistance,
            player.PlayerTransform.position.y,
            player.PlayerWallHit.point.z + player.PlayerWallHit.normal.z * _climbWallDistance
        );

        player.PlayerRigidbody.MovePosition(climbPos); // This does not work properly yet
        _performingClimb = true;
        player.PlayerAnimator.SetBool("isClimbing", _performingClimb);
    }

    void EndClimb()
    {
        _performingClimb = false;
        Debug.Log("xxx");
        player.StartCoroutine(HandlePullUpFinished());
    }
    


    // Gets Invoked when "Pull Up Wall" Animation finished => That should be changed seems dirty idk, maybe save the wall height at the start of the climb end when the gameObject y-Coordinate is there then invoke?
    // Usual Rigidbody Constraints get reactivated
    // global bool _performingClimb is set to false again as we finished climbing
    IEnumerator HandlePullUpFinished()
    {
        yield return new WaitUntil(() => player.PlayerWallHeight <= 0.01f);
        
        player.PlayerRigidbody.useGravity = true;
        player.PlayerRigidbody.excludeLayers = 0;
        player.PlayerAnimator.SetBool("isClimbing", _performingClimb);

        yield return new WaitUntil(() => player.IsGrounded);
        player.ChangeState(new IdleState(player));
    }

    // Removing eventListeners
    public void Exit()
    {
        player.PlayerAnimator.SetBool("wallEnd", false);
    }

}
