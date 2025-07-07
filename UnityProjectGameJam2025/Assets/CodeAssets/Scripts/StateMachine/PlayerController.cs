using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerController : MonoBehaviour
{
    private IState currentState;
    public Rigidbody PlayerRigidbody { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    public Collider PlayerCollider { get; private set; }

    public bool IsGrounded { get; private set; }


    [SerializeField] private Camera _playerCamera; // Assign in Inspector
    public Camera PlayerCamera => _playerCamera;

    [SerializeField] private float _jumpForce;

    public float JumpForce => _jumpForce;

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

    }
    void Start()
    {
        ChangeState(new IdleState(this));
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTransform = this.gameObject.transform;
        GroundCheck();
        currentState?.Update();

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentState is not FightState)
        {
            ChangeState(new FightState(this));
        }
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
