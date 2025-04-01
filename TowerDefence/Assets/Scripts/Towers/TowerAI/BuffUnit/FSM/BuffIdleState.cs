using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIdleState : BuffBaseState
{
    
    private readonly UnitTracker unitTracker;
    
    public BuffIdleState(GameObject go)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
        Debug.Log("Buff: IdleState");
    }
    public override void Enter(GameObject go)
    {
        
    }

    public override void Update(GameObject go)
    {
        
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override BuffBaseState HandleInput(GameObject go)
    {
        if (unitTracker.UnitTargets != null && TowerPlacement.hasBeenPlaced)
        {
            if (unitTracker.UnitTargets.Count >= 1)
            {
                return new BuffLocateAllyState(go);
            }
        }
        return null;
    }
}
