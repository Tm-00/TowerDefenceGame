using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class RifleFinishedState : RifleBaseState
{
    [Header("Class References")]
    internal RifleStats rifleStats;
    
    
    private NavMeshAgent agent;
    private Transform coreNodePosition;
    private GameObject coreNode;
    private CNHealth cnHealth;

    public RifleFinishedState(GameObject go)
    {
        rifleStats = go.GetComponent<RifleStats>();
        coreNode = GameObject.Find("CoreNode");
        cnHealth = coreNode.GetComponent<CNHealth>();
    }

    public override void Enter(GameObject go)
    {
        cnHealth.HealthHandler();
        ObjectPoolManager.ReturnObjectToPool(go);
    }

    public override void Update(GameObject go)
    {
        
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override RifleBaseState HandleInput(GameObject go)
    {
        if (rifleStats.currentHealth > 0)
        {
            return new RifleIdleState(go);
        }
        return null;
    }
}