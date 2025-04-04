using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneDeadState : LaneBaseState
{
    [Header("Class References")]
    private readonly LaneStats laneStats;
    
    public LaneDeadState(GameObject go)
    {
        laneStats = go.GetComponent<LaneStats>();

    }
    public override void Enter(GameObject go)
    {
        Debug.Log("Lane: DeadState");
        ObjectPoolManager.ReturnObjectToPool(go);
    }

    public override void Update(GameObject go)
    {

    }

    public override void Exit(GameObject go)
    {

    }

    public override LaneBaseState HandleInput(GameObject go)
    {
        if (laneStats.currentHealth > 0)
        {
            return new LaneIdleState(go);
        }
        return null;
    }
}