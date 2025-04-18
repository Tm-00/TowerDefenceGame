using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlightIdleState : FlightBaseState
{
    private NavMeshAgent agent;
    private Transform coreNodePosition;
    
    private readonly UnitTracker unitTracker;  
    // Constructor.
    public FlightIdleState(GameObject go)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
        
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = unitTracker.UnitTargets[0].transform;
        Debug.Log("Flight Drone: Idle State");
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        
    }
    
    // Update
    public override void Update(GameObject go)
    {

    }
    
    // Exit
    public override void Exit(GameObject gameObject)
    {
        
    }
    
    // Input
    public override FlightBaseState HandleInput(GameObject go)
    {
        // Idle -> Move
        if ( unitTracker.UnitTargets != null)
        {
            return new FlightMoveState(go);
        }
        
        // if at the core node
        if (Vector3.Distance(agent.transform.position, coreNodePosition.transform.position) <= 5)
        {
            return new FlightFinishedState(go);
        }
        return null;
    }
}