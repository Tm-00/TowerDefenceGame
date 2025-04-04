using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDeadState : MeleeBaseState
{
    [Header("Class References")]
    internal MeleeStats meleeStats;
    
    public MeleeDeadState(GameObject go)
    {
        meleeStats = go.GetComponent<MeleeStats>();
    }
    public override void Enter(GameObject go)
    {
        Debug.Log("Melee: DeadState");
        ObjectPoolManager.ReturnObjectToPool(go);
    }

    public override void Update(GameObject go)
    {
        
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override MeleeBaseState HandleInput(GameObject go)
    {
        if (meleeStats.currentHealth > 0)
        {
            return new MeleeIdleState(go);
        }
        return null; 
    }
}