using UnityEngine;

public class ApplyRootMotionToRidigbody : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAnimatorMove()
    {
        _rigidbody.MovePosition(_rigidbody.position + _animator.deltaPosition);
        // _rigidbody.MoveRotation(_rigidbody.rotation * _animator.deltaRotation);
    }
}
