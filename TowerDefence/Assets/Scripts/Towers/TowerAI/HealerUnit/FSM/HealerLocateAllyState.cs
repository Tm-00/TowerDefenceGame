using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealerLocateAllyState : HealerBaseState
{
    private Vector3 closestTarget;
  //  private float speed = 1.0f;
    private GameObject gameManager;
    private readonly UnitTracker unitTracker;
    private HealerStats healerStats;

    
    public HealerLocateAllyState(GameObject go)
    {
        gameManager = GameObject.Find("GameManager");
        healerStats = go.GetComponent<HealerStats>();
        unitTracker = gameManager.GetComponent<UnitTracker>();
    }
    public override void Enter(GameObject go)
    {
        Debug.Log("Healer: LocateAllyState");
    }

    public override void Update(GameObject go)
    { 
        var closestAlly = unitTracker?.FindClosestUnit(go)?.gameObject;
        
        if (closestAlly != null)
        {
            closestTarget = unitTracker.FindClosestUnit(go).transform.position;
        }
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override HealerBaseState HandleInput(GameObject go)
    {
        // Move -> Heal
        if (Vector3.Distance(go.transform.position, closestTarget) <= 20)
        {
            return new HealerHealState(go);
        }

        if (healerStats.currentHealth <= 0 )
        {
            return new HealerDeadState(go);
        }
        return null;
    }
}