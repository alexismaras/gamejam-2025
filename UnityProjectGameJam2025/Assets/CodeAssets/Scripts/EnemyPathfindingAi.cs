using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;
using System.Linq;


public class EnemyPathfindingAi : MonoBehaviour
{
    public enum EnemyState { Idle, Fighting, Climbing }
    public EnemyState CurrentEnemyState;
    [SerializeField] GameObject _player;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private NavMeshPath _navMeshPath;
    private Vector3 _destination;
    private List<Vector3> _pathCorners = new List<Vector3>();
    private Rigidbody _rigidbody;
    private bool _following = false;
    private int _pathCornerIndex = 0;

    private bool _cNewDest = false;
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
            _cNewDest = !_cNewDest;
        }

        if(_cNewDest)
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
        _navMeshPath = new NavMeshPath();
        _navMeshAgent.ResetPath();

        _navMeshAgent.enabled = false;
        _navMeshAgent.enabled = true;

        bool pathFound = _navMeshAgent.CalculatePath(_destination, _navMeshPath);
    
        if (!pathFound)
        {
            Debug.LogWarning("No valid path found!");
            return;
        }

        _pathCorners = new List<Vector3>(_navMeshPath.corners);

        Debug.Log("Path Corners: " + string.Join(" -> ", 
        _pathCorners.Select(v => $"({v.x:F1}, {v.z:F1})")));
    
        _pathCornerIndex = 0;
        _following = true;
        _animator.SetBool("run", true);

        for (int i = 0; i < _pathCorners.Count - 1; i++)
        {
            Debug.DrawLine(_pathCorners[i], _pathCorners[i+1], Color.red, 2f);
        }
        }

    void Follow()
    {
        
        Vector3 currentPos = transform.position;
        Vector3 targetPos;
        if(_pathCornerIndex + 1 < _pathCorners.Count)
        {
            targetPos = new Vector3(_pathCorners[_pathCornerIndex+1].x, currentPos.y, _pathCorners[_pathCornerIndex+1].z);
        }
        else
        {
            targetPos = new Vector3(_destination.x, currentPos.y, _destination.z);
        }
        if (AreVectorsClose(currentPos, targetPos))
        {
            _rigidbody.MovePosition(targetPos);
            _pathCornerIndex++;
        }
        if (AreVectorsClose(currentPos, _destination))
        {
            _pathCornerIndex=0;
            _following = false;
            _animator.SetBool("run", false);
        }
        Vector3 targetDir = targetPos-currentPos;
        Quaternion newRotation = Quaternion.LookRotation(targetDir, Vector3.up);
        _rigidbody.MoveRotation(newRotation);  

    }
    bool AreVectorsClose(Vector3 a, Vector3 b, float threshold = 0.05f)
    {
        return (a - b).sqrMagnitude < threshold;
    }
}
