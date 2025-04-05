using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutDeadState : ScoutBaseState
{
    [Header("Class References")]
    internal ScoutStats scoutStats;
    
    public ScoutDeadState(GameObject go)
    {
        scoutStats = go.GetComponent<ScoutStats>();
    }
    public override void Enter(GameObject go)
    {
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
