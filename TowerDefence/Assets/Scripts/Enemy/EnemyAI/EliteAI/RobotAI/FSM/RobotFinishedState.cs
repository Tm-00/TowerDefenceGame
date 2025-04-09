using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class RobotFinishedState : RobotBaseState
{
    [Header("Class References")]
    internal RobotStats robotStats;
    
    private NavMeshAgent agent;
    private Transform coreNodePosition;
    private GameObject coreNode;
    private CNHealth cnHealth;

    public RobotFinishedState(GameObject go)
    {
        robotStats = go.GetComponent<RobotStats>();
        coreNode = GameObject.Find("CoreNode");
        cnHealth = coreNode.GetComponent<CNHealth>();
    }

    public override void Enter(GameObject go)
    {
        cnHealth.HealthHandler(1);
        ObjectPoolManager.ReturnObjectToPool(go);
    }

    public override void Update(GameObject go)
    {
        
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override RobotBaseState HandleInput(GameObject go)
    {
        if (robotStats.currentHealth > 0)
        {
            return new RobotIdleState(go);
        }
        return null;    }
}