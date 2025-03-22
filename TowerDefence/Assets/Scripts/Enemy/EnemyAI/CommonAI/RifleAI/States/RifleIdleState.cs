using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RifleIdleState : RifleBaseState
{
    private NavMeshAgent agent;
    private Transform coreNodePosition;
    //TODO add a cooldown timer on spawn of 5 seconds
    // Constructor.
    public RifleIdleState(GameObject go)
    {
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = UnitTracker.UnitTargets[0].transform;
        Debug.Log("Rifle Drone: Idle State");
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
    public override RifleBaseState HandleInput(GameObject go)
    {
        // Idle -> Move
        if ( UnitTracker.UnitTargets != null)
        {
            // go to move state that handles target selection and where to go
            if (UnitTracker.UnitTargets.Count == 1) 
            {
                // Change the state -> MoveState.
                return new RifleMoveState(go);
            }
            // idle if at the core node
            if (Vector3.Distance(agent.transform.position, coreNodePosition.transform.position) <= 5)
            {
                return new RifleIdleState(go);
            }
        }
        return null;
    }
}