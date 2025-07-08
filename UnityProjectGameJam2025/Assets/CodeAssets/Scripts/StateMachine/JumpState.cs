using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;


public class JumpState : IState
{
    private PlayerController player;
    private float speed;

    // private float _elapsedTimeSinceJump;
    private float _airTime;

    private float _startHeight;

    private bool _hasReachedMaxHeight;
    private bool _hasMadeGroundcheck;
    private bool _addedDownForce;


    public JumpState(PlayerController player, float speed)
    {
        this.player = player;
        this.speed = speed;
    }


    public void Enter()
    {
        Debug.Log("JUMP STATE ENTERED");
        // _elapsedTimeSinceJump = 0;
        _airTime = 0;
        _startHeight = player.PlayerRigidbody.position.y;
        player.PlayerRigidbody.AddForce(Vector3.up * player.JumpForce + player.PlayerRigidbody.transform.forward * player.JumpForce * speed * 0.5f, ForceMode.Impulse);
    }

    // Update is called once per frame
    public void Update()
    {
        Debug.Log("A");
        bool freezeJump = Input.GetKey(KeyCode.Space);
        // _elapsedTimeSinceJump += Time.deltaTime;
        if (player.PlayerRigidbody.position.y >= _startHeight + player.JumpMaxHeight)
        {
            _hasReachedMaxHeight = true;
            if (!_hasMadeGroundcheck)
            {
                player.AsyncGroundCheck();
                _hasMadeGroundcheck = true;
            }
            player.PlayerRigidbody.linearVelocity = Vector3.zero;
            player.PlayerRigidbody.angularVelocity = Vector3.zero;

            if (freezeJump && _airTime < player.JumpMaxAirTime)
            {
                Debug.Log("C");
                _airTime += Time.deltaTime;
                player.PlayerRigidbody.useGravity = false;



            }
            else
            {
                player.PlayerRigidbody.useGravity = true;
                if (!_addedDownForce)
                {
                    player.PlayerRigidbody.AddForce(Vector3.down * player.JumpForce *0.75f, ForceMode.Impulse);
                    _addedDownForce = true;

                }

            }

        }

    }


    // Removing eventListeners
    public void Exit()
    {
    }

}
