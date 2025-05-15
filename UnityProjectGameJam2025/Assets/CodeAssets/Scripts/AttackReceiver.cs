using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Cinemachine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CinemachineImpulseSource))]
public class AttackReceiver : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    private Animator _animator;
    private Rigidbody _rigidbody;

    private CinemachineImpulseSource _impulseSource;
    [SerializeField] private float _shakeIntensity = 1f;
    [SerializeField] private int _bloodSplatterEmissionCount = 1;

    [SerializeField] private Collider _upperHurtbox;
    [SerializeField] private Collider _lowerHurtbox;

    private bool _isAttacked;
    private Vector3 _attackStartVector;
    private GameObject _attackingBodyPart;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        AttackSource.OnAttackStart += HandleAttackStart;
        AttackSource.OnAttackEnd += HandleAttackEnd;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    

    void Update()
    {
        if (_isAttacked)
        {
            CheckAttackHit();
        }
    }

    // Gets Invoked by AttackSource Script
    void HandleAttackStart(Vector3 attackStartVector, GameObject attackingBodyPart)
    {
        _isAttacked = true;
        _attackStartVector = attackStartVector;
        _attackingBodyPart = attackingBodyPart;
    }

    // Gets Invoked by AttackSource Script (if Attack did NOT hit) or by CheckAttackHit() (if Attack did hit)
    void HandleAttackEnd()
    {
        _isAttacked = false;
        _attackStartVector = Vector3.zero;
        _attackingBodyPart = null;
    }

    // Checks if Attack Body Part of an Attack Source Script is colliding with one of the HurtBoxes, Plays hit Animation and applies an Impulse to the Rigidbody -> Gets Called in Update
    void CheckAttackHit()
    {
        Vector3 attackDirection = _attackingBodyPart.transform.position - _attackStartVector;
        Vector3 punchForceDirection = attackDirection.normalized;

        Collider attackingBodyPartCollider = _attackingBodyPart.GetComponent<Collider>();

        if (CollidersAreColliding(_upperHurtbox, attackingBodyPartCollider) && !LayerIsExcluded(_upperHurtbox, attackingBodyPartCollider))
        {
            _animator.Play("TopHit"+GetHitAnimation(punchForceDirection));

        }

        else if (CollidersAreColliding(_lowerHurtbox, attackingBodyPartCollider) && !LayerIsExcluded(_lowerHurtbox, attackingBodyPartCollider))
        {
            _animator.Play("MidHit"+GetHitAnimation(punchForceDirection));
        }

        else 
        {
            return;
        }

        Vector3 punchForceVector = new Vector3(punchForceDirection.x, 0, punchForceDirection.z);
        _rigidbody.AddForce(punchForceVector * 150f, ForceMode.Impulse);
        _impulseSource.GenerateImpulse(_shakeIntensity);
        _particleSystem.transform.position = new Vector3(_particleSystem.transform.position.x, _attackingBodyPart.transform.position.y, _particleSystem.transform.position.z);
        _particleSystem.Emit(_bloodSplatterEmissionCount);

        HandleAttackEnd();
    }


    // Detects what hit Animation to play, based on the Vector of the punch animation Tweak point (bodyPart that hits). 
    // Note: this should probably be switched to a determination based on hit position on the target!
    string GetHitAnimation(Vector3 hitDirection) 
    {   
        string animationString = "";
        string animationStringDirection = "";
        // Get local direction vectors
        Vector3 right = transform.right;
        Vector3 left = transform.right * -1f; // Equivalent to transform.left

        
        // Calculate dot products
        float dotRight = Vector3.Dot(hitDirection, right);
        float dotLeft = Vector3.Dot(hitDirection, left);

        if (dotRight > dotLeft) 
        {
            animationStringDirection += "RL";
        }
        else
        {
            animationStringDirection += "LR";
        }

        Debug.Log(animationStringDirection+"1");
        return animationStringDirection+"1";
    }

    // Checks if listener Collider has source Collider's Layer explicitly excluded
    bool LayerIsExcluded(Collider listener, Collider source)
    {
        LayerMask excludedLayers = listener.excludeLayers;
        return (excludedLayers.value & (1 <<source.gameObject.layer)) != 0;
    }

    // Checks if listener Collider and source Collider are Intersecting (does not account for excluded/included Layers)...=> Therefore LayersIsExcluded function
    // Why not just use OnTriggerEnter() ???  -> Because a List / Array System of all Colliders exiting and entering would kinda be overkill ig. Additionally, OnTriggerEnter does not
    // tell me if upper- or lower Hurtbox was Triggered.
    bool CollidersAreColliding(Collider listener, Collider source)
    {
        return listener.bounds.Intersects(source.bounds);
    }

    // Remove eventListeners when this gameObject is destroyed... stackoverflow told me to do so..
    void OnDestroy()
    {
        AttackSource.OnAttackStart -= HandleAttackStart;
        AttackSource.OnAttackEnd -= HandleAttackEnd;
    }

    
}
