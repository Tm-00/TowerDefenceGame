using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RifleIdleState : RifleBaseState
{
    private NavMeshAgent agent;
    private Transform coreNodePosition;
    private readonly UnitTracker unitTracker;
    
    public RifleIdleState(GameObject go)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
        
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = unitTracker.UnitTargets[0].transform;
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
        if (unitTracker.UnitTargets != null)
        {
            // Change the state -> MoveState.
            return new RifleMoveState(go);
        }
            
        //if at the core node
        if (Vector3.Distance(agent.transform.position, coreNodePosition.transform.position) <= 5)
        {
            return new RifleFinishedState(go);
        }
        return null;
    }
}