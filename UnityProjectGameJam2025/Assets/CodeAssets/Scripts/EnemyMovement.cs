using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Cinemachine;

public class EnemyMovement : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;

    private float _detectionRadius = 3f;
    private float _maxDistance = 10f; // Adjust as needed
    [SerializeField] private LayerMask _playerLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _playerLayer);
        
        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {

                Vector3 playerPosition = hit.gameObject.transform.position;

                Vector3 lookDirection = new Vector3(playerPosition.x, transform.position.y, playerPosition.z) - transform.position; // y Coordinate is set to this GameObject to get horizontally straight vector

                Vector3 targetDirection = lookDirection.normalized;

                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 20f* Time.deltaTime, 0.0f);
                _rigidbody.MoveRotation(Quaternion.LookRotation(newDirection));

            }
        }
    }

    // Optional: Draw a spherecast gizmo for visualization in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }


}
