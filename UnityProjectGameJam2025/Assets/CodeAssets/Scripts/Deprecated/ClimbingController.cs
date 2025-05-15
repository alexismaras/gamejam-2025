using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(CharacterStateMachine))]
public class ClimbingController : MonoBehaviour
{
    [SerializeField] private LayerMask _climbableLayer;
    [SerializeField] private float _maxDetectionHeight = 10f;
    [SerializeField] private float _detectionOffsetForward = 0.8f;
    [SerializeField] private float _wallDetectionRangeForward = 1.0f;
    [SerializeField] private float _detectWallEndOffsetY = 1.7f;
    [SerializeField] private float _minClimbHeight = 2f;
    [SerializeField] private float _climbWallDistance = 0.6f;
    
    private Rigidbody _rigidbody;
    private Collider _capsuleCollider;
    private Animator _animator;
    private CharacterStateMachine _characterStateMachine;

    private float _detectedWallHeight;
    private RaycastHit _wallHit;
    private bool _performingClimb;

    private float _locomotionDampingParameter = 0.15f;
    private int _climbDirXParameterHash;
    private int _climbDirYParameterHash;

    public static event Action<bool> OnClimbEnd; // This should NOT be static because the other Characters/Entitys would listen to that.. change ASAP

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        _characterStateMachine = GetComponent<CharacterStateMachine>();

        _climbDirXParameterHash = Animator.StringToHash("climbDirX");
        _climbDirYParameterHash = Animator.StringToHash("climbDirY");
    }

    // Constantly detect if a wall is in front of the gameObject and how tall it is.
    void Update()
    { 
        _detectedWallHeight = DetectWallHeight();
        _wallHit = DetectWall();
        _animator.SetBool("wallEnd", DetectWallEnd());
    }

    // Sets Animator floats based on verical and horizontal Input, Invoked by CharacterStateMachine Class if State is Climbing
    public void ProcessDirectionalInput(float verticalInput, float horizontalInput, bool shouldRun)
    {
        _animator.SetFloat(_climbDirYParameterHash, verticalInput, _locomotionDampingParameter, Time.deltaTime);
        _animator.SetFloat(_climbDirXParameterHash, horizontalInput, _locomotionDampingParameter, Time.deltaTime);

    }

    // Casts a Ray from _maxDetectionHeight above and _detectionOffsetForward in front of the gameObject in dircetion down.
    // Returns the by _maxDetectionHeight normalized hit distance => Will return 0 if hit.distance is equal to _maxDetectionHeight
    // Fallback returns 0 if no object of layer _climbableWall is detected
    float DetectWallHeight()
    {
        RaycastHit hit;
        Vector3 raycastStart = new Vector3(transform.position.x, transform.position.y, transform.position.z) + transform.forward * _detectionOffsetForward + Vector3.up * _maxDetectionHeight;
        if (Physics.Raycast(raycastStart, Vector3.down, out hit, _maxDetectionHeight, _climbableLayer))
        {
            Debug.DrawRay(raycastStart, Vector3.down * _maxDetectionHeight, Color.blue, 0.02f, false);
            return _maxDetectionHeight - hit.distance;
        }
        return 0f;
    }

    // Gets a detected wall's normal direction and flips it -> (apparently hit.normal works like a laserbeam being reflected on the wall so it always points away from the wall, therefore we flip it)
    // Return gets casted to Quaternion because its need for rotating the rigidbody
    Quaternion GetWallRotation(RaycastHit hit)
    {
        Vector3 wallNormal = hit.normal;

        wallNormal *= -1;

        return Quaternion.LookRotation(wallNormal);
    }
    
    // Casts a Ray from GameObject transform.position in direciton transform.forward
    // Detects any wall that is within _wallDetectionRangeForward in front of this gameObject
    // Returns the RayCast hit for further processing
    // Fallback returns a default RaycastHit if no wall is detected
    RaycastHit DetectWall()
    {
        RaycastHit hit;
        Vector3 raycastStart = transform.position;
        if (Physics.Raycast(raycastStart, transform.forward, out hit, _wallDetectionRangeForward, _climbableLayer))
        {
            Debug.DrawRay(raycastStart, transform.forward * _wallDetectionRangeForward, Color.blue, 0.02f, false);
            return hit;
        }
        return default(RaycastHit);
    }

    // Casts a Ray from GameObject transform.position, offset by _detectWallEndOffsetY in Y-Direction. Points in direction GameObject transform.forward
    // Detects any wall that is within _wallDetectionRangeForward in front of this gameObject
    // Returns false if the RayCast is still detecting a wall
    // Fallback returns true => no wall detected
    bool DetectWallEnd()
    {
        RaycastHit hit;
        Vector3 raycastStart = new Vector3(transform.position.x, transform.position.y + _detectWallEndOffsetY, transform.position.z);
        if (Physics.Raycast(raycastStart, transform.forward, out hit, _wallDetectionRangeForward, _climbableLayer))
        {
            Debug.DrawRay(raycastStart, transform.forward * _wallDetectionRangeForward, Color.blue, 0.02f, false);
            return false;
        }
        return true;
    }

    // Called by CharacterStateMachine when pressing Space
    // If Detected Wall is not minimum _minClimbHeight, the Method returns false so CharacterStateMachine can turn the Space Input into a regular Jump
    // If wall is high enough:
    // Prepare Rigidboyd Constraints
    // Rotate gameObjects rigidbody in wall direction and adjust the position so the character does not glitch through the wall
    // Tell Animator that we are climbing
    // Set global _performingClimb bool true
    public bool StartClimb()
    {
        if (_detectedWallHeight >= _minClimbHeight && _wallHit.collider != null)
        {
            _rigidbody.useGravity = false;
            _capsuleCollider.enabled = false;
            _rigidbody.MoveRotation(GetWallRotation(_wallHit));
            _rigidbody.MovePosition(_wallHit.point + _wallHit.normal * _climbWallDistance); // This does not work properly yet
            _performingClimb = true;
            _animator.SetBool("isClimbing", _performingClimb);
            return true;
        }
        return false;
    }


    // Gets Invoked when "Pull Up Wall" Animation finished => That should be changed seems dirty idk, maybe save the wall height at the start of the climb end when the gameObject y-Coordinate is there then invoke?
    // Usual Rigidbody Constraints get reactivated
    // global bool _performingClimb is set to false again as we finished climbing
    void HandlePullUpFinished()
    {
        _rigidbody.useGravity = true;
        _capsuleCollider.enabled = true;
        _performingClimb = false;
        _animator.SetBool("isClimbing", _performingClimb);
    }

    // Removing eventListeners
    void OnDestroy()
    {
    }

}
