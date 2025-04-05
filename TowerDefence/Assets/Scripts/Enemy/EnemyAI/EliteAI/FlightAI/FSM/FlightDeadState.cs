using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightDeadState : FlightBaseState
{
    [Header("Class References")]
    internal FlightStats flightStats;

    public FlightDeadState(GameObject go)
    {
        flightStats = go.GetComponent<FlightStats>();
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

    public override FlightBaseState HandleInput(GameObject go)
    {
        if (flightStats.currentHealth > 0)
        {
            return new FlightIdleState(go);
        }
        return null;
    }
}
