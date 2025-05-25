using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;
using System.Linq;

public class EmptyState : IState
{
    private EnemyController enemy;
    
    public EmptyState(EnemyController enemy) 
    {
        this.enemy = enemy;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Enter()
    {
    }

    // Update is called once per frame
    public void Update()
    {
    }

    public void Exit()
    {

    }
}
