using UnityEngine;

public class ClimbingInverseKinematics : MonoBehaviour
{
    [SerializeField] LayerMask _climbableLayer;
    [SerializeField] private GameObject _characterLeftHand;
    [SerializeField] private GameObject _characterRightHand;
    [SerializeField] private GameObject _characterLeftFoot;
    [SerializeField] private GameObject _characterRightFoot;
    [SerializeField] private GameObject _characterHead;
    private Animator _animator;
    private Vector3 curGripPosForGiz;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (_animator.GetCurrentAnimatorStateInfo(layerIndex).IsName("Climbing Blend Tree"))
        {

            _animator.SetIKPosition(AvatarIKGoal.LeftHand, FindGripPosition(_characterLeftHand));
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);

            _animator.SetIKPosition(AvatarIKGoal.RightHand, FindGripPosition(_characterRightHand));
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);

            _animator.SetIKPosition(AvatarIKGoal.LeftFoot, FindGripPosition(_characterLeftFoot));
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);

            _animator.SetIKPosition(AvatarIKGoal.RightFoot, FindGripPosition(_characterRightFoot));
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
        }
    }

    Vector3 FindGripPosition(GameObject bodyPart)
    {
        RaycastHit hit;
        Vector3 rayOrigin = bodyPart.transform.position;
        Vector3 rayDir = transform.forward;
        if (Physics.Raycast(rayOrigin, rayDir, out hit, 1f, _climbableLayer))
        {
            Debug.DrawRay(rayOrigin, rayDir, Color.green, 0.02f,false);
            Vector3 gripPos = bodyPart.transform.position + hit.point - bodyPart.transform.position;
            curGripPosForGiz = gripPos;
            return gripPos;
        }
        return bodyPart.transform.position; // Fallback
    }

    void OnDrawGizmos()
    {
        float radius = 0.2f;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(curGripPosForGiz, radius);
    }
}
