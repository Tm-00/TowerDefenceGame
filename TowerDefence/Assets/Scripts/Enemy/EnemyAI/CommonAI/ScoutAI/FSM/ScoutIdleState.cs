using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScoutIdleState : ScoutBaseState
{
    private NavMeshAgent agent;
    private Transform coreNodePosition;
    private readonly UnitTracker unitTracker;
    
    // Constructor.
    public ScoutIdleState(GameObject go)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
        
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = unitTracker.UnitTargets[0].transform;
        Debug.Log("Scout Drone: Idle State");
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
    public override ScoutBaseState HandleInput(GameObject go)
    {

        // go to move state that handles target selection and where to go
        if (unitTracker.UnitTargets != null)
        {
            return new ScoutMoveState(go);
        }
        // idle if at the core node
        if (Vector3.Distance(agent.transform.position, coreNodePosition.transform.position) <= 5)
        {
            return new ScoutIdleState(go);
        }
        
        return null;
    }
}