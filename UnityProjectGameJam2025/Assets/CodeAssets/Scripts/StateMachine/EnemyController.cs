using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyController : MonoBehaviour
{
    private IState currentState;
    private float _detectionRadius = 3f;
    private float _maxDistance = 10f;
    [SerializeField] private LayerMask _playerLayer;
    public Rigidbody EnemyRigidbody { get; private set; }
    public Animator EnemyAnimator { get; private set; }
    public NavMeshAgent EnemyNavMeshAgent { get; private set; }
    [SerializeField] private GameObject _followTarget;
    public GameObject FollowTarget => _followTarget; 

    void Awake()
    {
        EnemyRigidbody = GetComponent<Rigidbody>();
        EnemyAnimator = GetComponent<Animator>();
        EnemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeState(new FollowState(this));
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Update();

        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _playerLayer);
        
        if (hits.Length > 0)
        {
            if (currentState is FollowState)
            {
                ChangeState(new EmptyState(this));
            }
            foreach (Collider hit in hits)
            {
                RotateToTarget(hit.gameObject.transform.position);
            }
        }
        else if(hits.Length == 0 && currentState is EmptyState)
        {
            ChangeState(new FollowState(this));
        }
    }

    // Optional: Draw a spherecast gizmo for visualization in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit(); // Clean up previous state
        currentState = newState;
        currentState.Enter(); // Initialize new state
    }

    void RotateToTarget(Vector3 target)
    {
        Vector3 lookDirection = new Vector3(target.x, transform.position.y, target.z) - transform.position; // y Coordinate is set to this GameObject to get horizontally straight vector

        Vector3 targetDirection = lookDirection.normalized;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 20f* Time.deltaTime, 0.0f);
        EnemyRigidbody.MoveRotation(Quaternion.LookRotation(newDirection));

    }
}

