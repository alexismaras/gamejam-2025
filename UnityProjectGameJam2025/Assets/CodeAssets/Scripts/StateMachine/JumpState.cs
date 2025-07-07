using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class JumpState : IState
{
    private PlayerController player;

    private float _elapsedTimeSinceJump;
    private float _airTime;
    
    public JumpState(PlayerController player)
    {
        this.player = player;
    }


    public void Enter()
    {
        Debug.Log("JUMP STATE ENTERED");
        _elapsedTimeSinceJump = 0;
        _airTime = 0;
        player.PlayerRigidbody.useGravity = false;
    }

    // Update is called once per frame
    public void Update()
    {
        _elapsedTimeSinceJump += Time.deltaTime;

        if (player.PlayerRigidbody.position.y >= 2f)
        {
            _airTime += Time.deltaTime;

            if (_airTime >= 1f)
            {
                player.ChangeState(new IdleState(player));
            }
            
        }
        else
        {
            JumpUp();
        }
        

    }

    void JumpUp()
    {
        player.PlayerRigidbody.MovePosition(player.PlayerRigidbody.position + new Vector3(0, 1, 0) * 10f * Time.deltaTime);
        Debug.Log("JUMPING NOW");
    }

    // Removing eventListeners
    public void Exit()
    {
    }

}
