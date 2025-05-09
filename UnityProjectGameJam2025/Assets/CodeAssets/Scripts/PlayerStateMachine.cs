using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public enum PlayerState { Idle, Fighting }

    public PlayerState CurrentPlayerState;

    public static event Action OnLeftPunch;
    public static event Action OnRightPunch;
    public static event Action OnHeadbutt;
    public static event Action OnJump;
    public static event Action OnLeftKick;
    public static event Action OnRightKick;

    private float _elapsedTimeSincePunch = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentPlayerState = PlayerState.Idle;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Idle:
                    CurrentPlayerState = PlayerState.Fighting;
                    HandleStateChange();
                    OnLeftPunch?.Invoke();
                    break;

                case PlayerState.Fighting:
                    OnLeftPunch?.Invoke();
                    _elapsedTimeSincePunch = 0;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Idle:
                    CurrentPlayerState = PlayerState.Fighting;
                    HandleStateChange();
                    OnRightPunch?.Invoke();
                    break;

                case PlayerState.Fighting:
                    OnRightPunch?.Invoke();
                    _elapsedTimeSincePunch = 0;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Idle:
                    CurrentPlayerState = PlayerState.Fighting;
                    HandleStateChange();
                    OnHeadbutt?.Invoke();
                    break;

                case PlayerState.Fighting:
                    OnHeadbutt?.Invoke();
                    _elapsedTimeSincePunch = 0;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Idle:
                    break;

                case PlayerState.Fighting:
                    OnLeftKick?.Invoke();
                    _elapsedTimeSincePunch = 0;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Idle:
                    break;

                case PlayerState.Fighting:
                    OnRightKick?.Invoke();
                    _elapsedTimeSincePunch = 0;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Idle:
                    OnJump?.Invoke();
                    break;

                case PlayerState.Fighting:
                    break;
            }
        }

        if (CurrentPlayerState == PlayerState.Fighting)
        {
            _elapsedTimeSincePunch += Time.deltaTime;
            if (_elapsedTimeSincePunch >= 3)
            {
                CurrentPlayerState = PlayerState.Idle;
                HandleStateChange();
                _elapsedTimeSincePunch = 0;
            }
        }
    }

    void HandleStateChange()
    {
        if(CurrentPlayerState == PlayerState.Idle)
        {
            _animator.SetBool("isIdleState", true);
            _animator.SetBool("isFightingState", false);
        }
        else if (CurrentPlayerState == PlayerState.Fighting)
        {
            _animator.SetBool("isIdleState", false);
            _animator.SetBool("isFightingState", true);
        }
    }
}
