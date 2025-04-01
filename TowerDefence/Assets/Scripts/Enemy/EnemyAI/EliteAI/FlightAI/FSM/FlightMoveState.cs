using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class FlightMoveState : FlightBaseState
{
    
    // will be able to reference itself
    private NavMeshAgent agent;

    private Vector3 closestTarget;

    private bool allunitsdead;
        
    private readonly UnitTracker unitTracker;
    
    // reference to the core node 
    private Transform coreNodePosition;
    
    // Constructor.
    public FlightMoveState(GameObject go)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
        
        // assign variables 
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = unitTracker.UnitTargets[0].transform;
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
    public override FlightBaseState HandleInput(GameObject go)
    {
        // Move -> Attack
        if (Vector3.Distance(agent.transform.position, closestTarget) <= 6 && allunitsdead != true)
        {
            return new FlightAttackState(go);
        }
        if (Vector3.Distance(agent.transform.position, coreNodePosition.transform.position) <= 5)
        {
            return new FlightFinishedState(go);
        }
        return null;
    }

    private void FilterTargets()
    {
        var closestEnemy = unitTracker.FindClosestWallUnit(agent);
        if (closestEnemy != null && unitTracker.UnitTargets.Count > 1)
        {
            closestTarget = unitTracker.FindClosestWallUnit(agent).transform.position;
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