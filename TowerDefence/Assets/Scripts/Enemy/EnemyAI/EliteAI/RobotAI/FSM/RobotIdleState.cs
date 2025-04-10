using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotIdleState : RobotBaseState
{
    private NavMeshAgent agent;
    private Transform coreNodePosition;
    private readonly UnitTracker unitTracker;
    
    // Constructor.
    public RobotIdleState(GameObject go)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = unitTracker.UnitTargets[0].transform;
        Debug.Log("Robot Drone: Idle State");
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
    public override RobotBaseState HandleInput(GameObject go)
    {
        // Change the state -> MoveState.
        if (unitTracker.UnitTargets != null)
        {
            return new RobotMoveState(go);
        }
            
        // idle if at the core node
        if (Vector3.Distance(agent.transform.position, coreNodePosition.transform.position) <= 5)
        {
            return new RobotFinishedState(go);
        }
        return null;
    }
}