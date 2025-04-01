using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class ScoutMoveState : ScoutBaseState
{
    
    // will be able to reference itself
    private NavMeshAgent agent;

    private Vector3 closestTarget;

    private bool allunitsdead;
        
    // reference to the core node 
    private Transform coreNodePosition;
    
    private readonly UnitTracker unitTracker;

    
    // Constructor.
    public ScoutMoveState(GameObject go)
    {
        // assign variables 
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = unitTracker.UnitTargets[0].transform;
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        Debug.Log("Scout Drone: Move State");
        agent.SetDestination(coreNodePosition.position);
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
    public override ScoutBaseState HandleInput(GameObject go)
    {
        if (Vector3.Distance(agent.transform.position, coreNodePosition.transform.position) <= 5)
        {
            return new ScoutFinishedState(go);
        }
        return null;
    }

  
}