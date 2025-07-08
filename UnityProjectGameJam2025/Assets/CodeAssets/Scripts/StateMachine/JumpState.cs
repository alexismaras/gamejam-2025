using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;


public class JumpState : IState
{
    private PlayerController player;

    // private float _elapsedTimeSinceJump;
    private float _airTime;

    private float _startHeight;

    
    public JumpState(PlayerController player)
    {
        this.player = player;
    }


    public void Enter()
    {
        Debug.Log("JUMP STATE ENTERED");
        // _elapsedTimeSinceJump = 0;
        _airTime = 0;
        _startHeight = player.PlayerRigidbody.position.y;
        player.PlayerRigidbody.AddForce(Vector3.up * player.JumpForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    public void Update()
    {
        bool freezeJump = Input.GetKey(KeyCode.Space);
        // _elapsedTimeSinceJump += Time.deltaTime;
        if (player.PlayerRigidbody.position.y >= _startHeight + player.JumpMaxHeight)
        {
            player.PlayerRigidbody.linearVelocity = Vector3.zero;
            player.PlayerRigidbody.angularVelocity = Vector3.zero;
            
            if (freezeJump)
            {
                player.PlayerRigidbody.useGravity = false;

                _airTime += Time.deltaTime;
            }
            else
            {
                player.ChangeState(new IdleState(player));
            }

        }
        
        if (_airTime >= player.JumpMaxAirTime)
        {
            player.ChangeState(new IdleState(player));
        }
        

    }

    void JumpUp()
    {
        player.PlayerRigidbody.MovePosition(player.PlayerRigidbody.position + new Vector3(0, 1, 0) * player.JumpForce * Time.deltaTime);
        Debug.Log("JUMPING NOW");
    }

    // Removing eventListeners
    public void Exit()
    {
    }

}
