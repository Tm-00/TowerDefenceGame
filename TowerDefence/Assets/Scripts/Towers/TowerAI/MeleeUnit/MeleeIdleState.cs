using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeIdleState : MeleeBaseState
{
    public MeleeIdleState(GameObject go)
    {
        Debug.Log("Turret: IdleState");
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

    public override MeleeBaseState HandleInput(GameObject go)
    {
        if (UnitTracker.EnemyTargets != null && TowerPlacement.hasBeenPlaced)
        {
            if (UnitTracker.EnemyTargets.Count >= 1)
            {
                return new MeleeLocateEnemyState(go);
            }
        }
        return null;
    }
}
