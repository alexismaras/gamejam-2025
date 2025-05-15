using UnityEngine;
using UnityEngine.AI;
using System;


public class EnemyPathfindingAi : MonoBehaviour
{
    public enum EnemyState { Idle, Fighting, Climbing }
    public EnemyState CurrentEnemyState;
    [SerializeField] GameObject _player;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private NavMeshPath _navMeshPath;
    private Vector3 _destination;
    private Vector3[] _pathCorners;
    private Rigidbody _rigidbody;
    private bool _following = false;
    private int _pathCornerIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshPath = new NavMeshPath();
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            NewDestination();
        }
        if (_following)
        {
            Follow();
        }
    }

    void NewDestination()
    {
        _destination = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
        Debug.Log(_navMeshAgent.CalculatePath(_destination, _navMeshPath));
        _pathCorners = _navMeshPath.corners;
        foreach(Vector3 pC in _pathCorners)
        {
            Debug.Log(pC.ToString());
        }
        _pathCornerIndex = 0;
        _following = true;
        _animator.SetBool("run", true);
    }

    void Follow()
    {
        
        Vector3 currentPos = transform.position;
        Vector3 targetPos;
        if(_pathCornerIndex < _pathCorners.Length)
        {
            targetPos = new Vector3(_pathCorners[_pathCornerIndex+1].x, currentPos.y, _pathCorners[_pathCornerIndex+1].z);
        }
        else
        {
            targetPos = new Vector3(_destination.x, currentPos.y, _destination.z);
        }
        Vector3 targetDir = targetPos-currentPos;
        Debug.Log("Pos" + targetDir + ", " + targetPos + ", " + currentPos);
        Quaternion newRotation = Quaternion.LookRotation(targetDir, Vector3.up);
        _rigidbody.MoveRotation(newRotation);  

    }
}
