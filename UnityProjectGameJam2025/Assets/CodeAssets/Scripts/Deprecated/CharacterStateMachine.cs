using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ClimbingController))]
[RequireComponent(typeof(FightingController))]
public class CharacterStateMachine : MonoBehaviour
{
    [SerializeField] ClimbingController _climbingController;
    [SerializeField] FightingController _fightingController;
    [SerializeField] IdleController _idleController;
    private Animator _animator;
    public enum PlayerState { Idle, Fighting, Climbing }

    public PlayerState CurrentPlayerState;

    public event Action OnJump;

    private float _elapsedTimeSincePunch = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _climbingController = GetComponent<ClimbingController>();
        _fightingController = GetComponent<FightingController>();
        _idleController = GetComponent<IdleController>();
        _animator = GetComponent<Animator>();

        CurrentPlayerState = PlayerState.Idle;
        HandleStateChange();
        
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        bool shouldRun = Input.GetKey(KeyCode.LeftControl);
        switch (CurrentPlayerState)
        {
            case PlayerState.Idle:
                _idleController.ProcessDirectionalInput(verticalInput, horizontalInput, shouldRun);
                break;

            case PlayerState.Fighting:
                _fightingController.ProcessDirectionalInput(verticalInput, horizontalInput, shouldRun);
                break;
            case PlayerState.Climbing:
                _climbingController.ProcessDirectionalInput(verticalInput, horizontalInput, shouldRun);
                break;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Idle:
                    CurrentPlayerState = PlayerState.Fighting;
                    HandleStateChange();
                    _fightingController.LeftPunch();
                    break;

                case PlayerState.Fighting:
                    _fightingController.LeftPunch();
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
                    _fightingController.RightPunch();
                    break;

                case PlayerState.Fighting:
                    _fightingController.RightPunch();
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
                    _fightingController.Headbutt();
                    break;

                case PlayerState.Fighting:
                    _fightingController.Headbutt();
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
                    _fightingController.LeftKick();
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
                    _fightingController.RightKick();
                    _elapsedTimeSincePunch = 0;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Idle:
                    if (_climbingController.StartClimb())
                    {
                        CurrentPlayerState = PlayerState.Climbing;
                        HandleStateChange();
                    }
                    else
                    {
                        _idleController.Jump();
                    }
                    
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
            _animator.SetBool("isClimbingState", false);
            StartCoroutine(CrossfadeLayer(0, 1.0f, 0.2f));
            StartCoroutine(CrossfadeLayer(1, 0.0f, 0.2f));
            StartCoroutine(CrossfadeLayer(2, 0.0f, 0.2f));
        }
        else if (CurrentPlayerState == PlayerState.Fighting)
        {
            _animator.SetBool("isIdleState", false);
            _animator.SetBool("isFightingState", true);
            _animator.SetBool("isClimbingState", false);
            StartCoroutine(CrossfadeLayer(0, 0.0f, 0.2f));
            StartCoroutine(CrossfadeLayer(1, 1.0f, 0.2f));
            StartCoroutine(CrossfadeLayer(2, 0.0f, 0.2f));
        }
        else if (CurrentPlayerState == PlayerState.Climbing)
        {
            _animator.SetBool("isIdleState", false);
            _animator.SetBool("isFightingState", false);
            _animator.SetBool("isClimbingState", true);
            StartCoroutine(CrossfadeLayer(0, 0.0f, 0.2f));
            StartCoroutine(CrossfadeLayer(1, 0.0f, 0.2f));
            StartCoroutine(CrossfadeLayer(2, 1.0f, 0.2f));
        }
    }

    private IEnumerator CrossfadeLayer(int layerIndex, float targetWeight, float duration)
    {
        float elapsed = 0f;
        float startWeight = _animator.GetLayerWeight(layerIndex);
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            _animator.SetLayerWeight(layerIndex, Mathf.Lerp(startWeight, targetWeight, t));
            yield return null;
        }
        
        _animator.SetLayerWeight(layerIndex, targetWeight);
    }

    void OnDestroy()
    {
    }
}
