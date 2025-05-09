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

    private CinemachineImpulseSource _impulseSource;
    [SerializeField] private float _shakeIntensity = 1f;
    private List<GameObject> _currentCollidingGameObjects = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        AttackSource.OnAttack += HandleAttack;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
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

    void OnAnimatorMove()
    {
        _rigidbody.MovePosition(_rigidbody.position + _animator.deltaPosition);
        _rigidbody.MoveRotation(_rigidbody.rotation * _animator.deltaRotation);
    }

    // Optional: Draw a spherecast gizmo for visualization in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }

    void HandleAttack(Vector3 punchForceDirection, GameObject attackingBodyPart)
    {
        if (_currentCollidingGameObjects.Contains(attackingBodyPart))
        {
            Debug.DrawRay(transform.position, punchForceDirection, Color.red, 50.0f, false);
            Debug.DrawRay(transform.position, transform.forward, Color.green, 50.0f, false);
            Debug.DrawRay(transform.position, transform.forward * -1f, Color.green, 50.0f, false);
            Debug.DrawRay(transform.position, transform.up, Color.green, 50.0f, false);
            Debug.DrawRay(transform.position, transform.up * -1f, Color.green, 50.0f, false);
            Debug.DrawRay(transform.position, transform.right, Color.green, 50.0f, false);
            Debug.DrawRay(transform.position, transform.right * -1f, Color.green, 50.0f, false);
            _animator.SetTrigger(GetHitAnimation(punchForceDirection));

            Vector3 punchForceVector = new Vector3(punchForceDirection.x, 0, punchForceDirection.z);

            _rigidbody.AddForce(punchForceVector * 200f, ForceMode.Impulse);
            _impulseSource.GenerateImpulse(_shakeIntensity);
        }
    }


    // Detects what hit Animation to play, based on the Vector of the punch animation Tweak point (bodyPart that hits). 
    // Note: this should probably be switched to a determination based on hit position on the target!
    string GetHitAnimation(Vector3 hitDirection) 
    {   
        string animationString = "";
        string animationStringY = "";
        string animationStringDirection = "";
        // Get local direction vectors
        Vector3 right = transform.right;
        Vector3 left = transform.right * -1f; // Equivalent to transform.left
        Vector3 back = transform.forward * -1f;
        Vector3 down = transform.up * -1f;

        
        // Calculate dot products
        float dotRight = Vector3.Dot(hitDirection, right);
        float dotLeft = Vector3.Dot(hitDirection, left);

        float dotBack = Vector3.Dot(hitDirection, back);
        float dotDown = Vector3.Dot(hitDirection, back);
        
        // Compare to determine dominant direction
        if (dotBack > dotDown) // Hit is more Vertical (Came from the top)
        {
            animationStringY += "Top";
        }
        else // Hit is more Horizontal 
        {
            animationStringY += "Mid";
        }

        if (dotRight > dotLeft) 
        {
            animationStringDirection += "RL";
        }
        else
        {
            animationStringDirection += "LR";
        }

        Debug.Log(animationStringY+"Hit"+animationStringDirection+"1");
        return animationStringY+"Hit"+animationStringDirection+"1";
    }

    

    void OnTriggerEnter(Collider other)
    {
        _currentCollidingGameObjects.Add(other.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        // Optional: Additional per-frame logic for each collider
    }

    void OnTriggerExit(Collider other)
    {
        _currentCollidingGameObjects.Remove(other.gameObject);
    }
    
    
}
