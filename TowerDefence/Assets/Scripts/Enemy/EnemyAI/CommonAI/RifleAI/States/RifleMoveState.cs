using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class RifleMoveState : RifleBaseState
{
    
    // will be able to reference itself
    private NavMeshAgent agent;

    private Vector3 closestTarget;

    private bool allunitsdead;
        
    // reference to the core node 
    private Transform coreNodePosition;
    
    // Constructor.
    public RifleMoveState(GameObject go)
    {
        // assign variables 
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = UnitTracker.UnitTargets[0].transform;
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
    public override RifleBaseState HandleInput(GameObject go)
    {
        // Move -> Attack
        if (Vector3.Distance(agent.transform.position, closestTarget) <= 6 && allunitsdead != true)
        {
            return new RifleAttackState(go);
        }
        if (Vector3.Distance(agent.transform.position, coreNodePosition.transform.position) <= 5)
        {
            return new RifleFinishedState(go);
        }
        //TODO add a death state transition + a health script for this enemy type
        // TODO add a game over script for finished state
        return null;
    }

    private void FilterTargets()
    {
        var cloestEnemy = UnitTracker.FindClosestWallUnit(agent);
        if (cloestEnemy != null && UnitTracker.UnitTargets.Count > 1)
        {
            closestTarget = UnitTracker.FindClosestWallUnit(agent).transform.position;
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