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


        int randomNumber = UnityEngine.Random.Range(0, 4);

        switch (randomNumber)
        {
            case 0:
                LeftPunch();
                break;


            case 1:
                RightPunch();
                break;

            case 2:
                LeftKick();
                break;
                
            case 3:
                RightKick();
                break;
        }

        

        player.ChangeState(new IdleState(player));
    }

    // Update is called once per frame
    public void Update()
    {

    }

    // Sets Animator floats based on verical and horizontal Input, Invoked by CharacterStateMachine Class if State is Fighting
    // Rotates Rigidbody in Camera Direction

    // Gets normalized look Direction of Camera, so this Gameobject can rotate in this Direction.
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
