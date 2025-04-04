using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class RobotMoveState : RobotBaseState
{
    
    // will be able to reference itself
    private NavMeshAgent agent;

    private Vector3 closestTarget;

    private bool allunitsdead;
    
    private GameObject enemy;
        
    // reference to the core node 
    private Transform coreNodePosition;
    
    private readonly UnitTracker unitTracker;
    
    // Constructor.
    public RobotMoveState(GameObject go)
    {
        // assign variables 
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = unitTracker.UnitTargets[0].transform;
        enemy = go;
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        Debug.Log("Rifle Drone: Move State");
    }
    
    // Update
    public override void Update(GameObject go)
    {
        FilterTargets();
    }
    
    // Exit
    public override void Exit(GameObject gameObject)
    {
        
    }
    
    // Input
    public override RobotBaseState HandleInput(GameObject go)
    {
        // Move -> Attack
        if (Vector3.Distance(agent.transform.position, closestTarget) <= 6 && allunitsdead != true)
        {
            return new RobotAttackState(go);
        }
        if (Vector3.Distance(agent.transform.position, coreNodePosition.transform.position) <= 5)
        {
            return new RobotFinishedState(go);
        }
        //TODO add a death state transition + a health script for this enemy type
        // TODO add a game over script for finished state
        return null;
    }

    private void FilterTargets()
    {
        var cloestEnemy = unitTracker.FindClosestWallUnit(enemy);
        if (cloestEnemy != null && unitTracker.UnitTargets.Count > 1)
        {
            closestTarget = unitTracker.FindClosestWallUnit(enemy).transform.position;
            agent.destination = closestTarget;
            allunitsdead = false;
        }
        else
        {
            agent.destination = coreNodePosition.transform.position;
            allunitsdead = true;
        }
    }
}