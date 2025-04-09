using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class ScoutFinishedState : ScoutBaseState
{
    [Header("Class References")]
    internal ScoutStats scoutStats;
    
    private NavMeshAgent agent;
    private Transform coreNodePosition;
    private GameObject coreNode;
    private CNHealth cnHealth;

    public ScoutFinishedState(GameObject go)
    {
        scoutStats = go.GetComponent<ScoutStats>();
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

    public override ScoutBaseState HandleInput(GameObject go)
    {
        if (scoutStats.currentHealth > 0)
        {
            return new ScoutIdleState(go);
        }
        return null;
    }
}