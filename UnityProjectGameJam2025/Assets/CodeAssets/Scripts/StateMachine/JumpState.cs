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
        _airTime = 0;
        _startHeight = player.PlayerRigidbody.position.y;
        player.PlayerRigidbody.AddForce(Vector3.up * player.JumpForce + player.PlayerRigidbody.transform.forward * player.JumpForce * speed * 0.5f, ForceMode.Impulse);
        player.ChangeState(new IdleState(player));
    }

    // Update is called once per frame
    public void Update()
    {
        if (player.PlayerRigidbody.position.y >= _startHeight + player.JumpMaxHeight)
        {
            player.PlayerRigidbody.linearVelocity = Vector3.zero;
            player.PlayerRigidbody.angularVelocity = Vector3.zero;
            player.ChangeState(new IdleState(player));

        }

    }


    // Removing eventListeners
    public void Exit()
    {
    }

}
