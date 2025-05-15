using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(WallDetector))]
public class PlayerController : MonoBehaviour
{
    private IState currentState;
    public Rigidbody PlayerRigidbody { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    public Collider PlayerCollider { get; private set; }
    public RaycastHit PlayerWallHit { get; private set; }
    public float PlayerWallHeight { get; private set; }
    public bool IsGrounded { get; private set; }

    [SerializeField] private LayerMask _climbableLayer;
    public LayerMask ClimbableLayer; 

    [SerializeField] private Camera _playerCamera; // Assign in Inspector
    public Camera PlayerCamera => _playerCamera;

    private WallDetector _wallDetector;

    public Coroutine StartStateCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        PlayerTransform = this.gameObject.transform;
        PlayerAnimator = GetComponent<Animator>();
        PlayerCollider = GetComponent<Collider>();

        _wallDetector = GetComponent<WallDetector>();
    }
    void Start()
    {
        PlayerAnimator.SetLayerWeight(0, 1);
        PlayerAnimator.SetLayerWeight(1, 0);
        ChangeState(new IdleState(this));
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTransform = this.gameObject.transform;
        PlayerWallHit = _wallDetector.DetectWall();
        PlayerWallHeight = _wallDetector.DetectWallHeight();
        GroundCheck();
        currentState?.Update();
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit(); // Clean up previous state
        currentState = newState;
        currentState.Enter(); // Initialize new state
    }

    void GroundCheck()
    {
        RaycastHit hit;
        Vector3 raycastStart = transform.position;
        if (Physics.Raycast(raycastStart, Vector3.down, out hit, 100f))
        {
            Debug.DrawRay(raycastStart, Vector3.down * 100f, Color.green, 0.02f, false);
        }

        if (hit.distance <= 0.01f)
        {
            Debug.Log("yyy");
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }
}
