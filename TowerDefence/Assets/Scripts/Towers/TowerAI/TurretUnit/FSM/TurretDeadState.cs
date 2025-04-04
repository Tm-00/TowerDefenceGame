using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDeadState : TurretBaseState
{
    [Header("Class References")]
    internal TurretStats turretStats;
    
    public TurretDeadState(GameObject go)
    {
        turretStats = go.GetComponent<TurretStats>();
    }
    public override void Enter(GameObject go)
    {
        Debug.Log("Turret: DeadState");
        ObjectPoolManager.ReturnObjectToPool(go);
    }

    public override void Update(GameObject go)
    {
        
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override TurretBaseState HandleInput(GameObject go)
    {
        if (turretStats.currentHealth > 0)
        {
            return new TurretIdleState(go);
        }
        return null;
    }
}