using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;
using System.Linq;

public class FollowState : IState
{
    private EnemyController enemy;
    
    private Vector3 _destination;
    private List<Vector3> _pathCorners = new List<Vector3>();
    private bool _following = false;
    private int _pathCornerIndex = 0;
    private NavMeshPath _navMeshPath;

    public FollowState(EnemyController enemy) 
    {
        this.enemy = enemy;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Enter()
    {
        _navMeshPath = new NavMeshPath();
        enemy.EnemyNavMeshAgent.updatePosition = false;
        enemy.EnemyNavMeshAgent.updateRotation = false;
    }

    // Update is called once per frame
    public void Update()
    {
        Debug.Log("x");
        NewDestination();
        if (_following)
        {
            Follow();
        }
    }

    void NewDestination()
    {
        _destination = new Vector3(enemy.FollowTarget.transform.position.x, enemy.transform.position.y, enemy.FollowTarget.transform.position.z);
        _navMeshPath = new NavMeshPath();
        enemy.EnemyNavMeshAgent.ResetPath();

        enemy.EnemyNavMeshAgent.enabled = false;
        enemy.EnemyNavMeshAgent.enabled = true;

        bool pathFound = enemy.EnemyNavMeshAgent.CalculatePath(_destination, _navMeshPath);
    
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
        enemy.EnemyAnimator.SetBool("run", true);

        for (int i = 0; i < _pathCorners.Count - 1; i++)
        {
            Debug.DrawLine(_pathCorners[i], _pathCorners[i+1], Color.red, 2f);
        }
        }

    void Follow()
    {
        
        Vector3 currentPos = enemy.transform.position;
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
            enemy.EnemyRigidbody.MovePosition(targetPos);
            _pathCornerIndex++;
        }
        if (AreVectorsClose(currentPos, _destination))
        {
            _pathCornerIndex=0;
            _following = false;
            enemy.EnemyAnimator.SetBool("run", false);
        }
        Vector3 targetDir = targetPos-currentPos;
        Quaternion newRotation = Quaternion.LookRotation(targetDir, Vector3.up);
        enemy.EnemyRigidbody.MoveRotation(newRotation);  

    }
    bool AreVectorsClose(Vector3 a, Vector3 b, float threshold = 0.05f)
    {
        return (a - b).sqrMagnitude < threshold;
    }
    
    public void Exit()
    {
        enemy.EnemyAnimator.SetBool("run", false);

        _following = false;
        _pathCornerIndex = 0;
        _pathCorners = new List<Vector3>();
        _destination = Vector3.zero;
    }
}
