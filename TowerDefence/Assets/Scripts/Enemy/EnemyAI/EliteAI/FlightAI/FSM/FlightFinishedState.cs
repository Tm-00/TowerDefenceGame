using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class FlightFinishedState : FlightBaseState
{
    [Header("Class References")]
    internal FlightStats flightStats;
    
    private NavMeshAgent agent;
    private Transform coreNodePosition;
    private GameObject coreNode;
    private CNHealth cnHealth;

    public FlightFinishedState(GameObject go)
    {
        flightStats = go.GetComponent<FlightStats>();
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

    public override FlightBaseState HandleInput(GameObject go)
    {
        if (flightStats.currentHealth > 0)
        {
            return new FlightIdleState(go);
        }
        return null;
    }
}