using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuffLocateAllyState : BuffBaseState
{
    private Vector3 closestTarget;
    private UnitTracker unitTracker;

    
    public BuffLocateAllyState(GameObject go)
    {
     
    }
    public override void Enter(GameObject go)
    {
        Debug.Log("Turret: LocateEnemyState");
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
    }

    public override void Update(GameObject go)
    {
        var closestAlly = unitTracker?.FindClosestUnit(go);
        
        if (closestAlly != null)
        {
            closestTarget = unitTracker.FindClosestUnit(go).transform.position;
        }
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override BuffBaseState HandleInput(GameObject go)
    {
        // Move -> Attack
        if (Vector3.Distance(go.transform.position, closestTarget) <= 10)
        {
            return new BuffAllyState(go);
        }
        return null;
    }
}