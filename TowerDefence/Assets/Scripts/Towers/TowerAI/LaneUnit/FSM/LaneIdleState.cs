using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneIdleState : LaneBaseState
{
    [Header("Class References")]
    private readonly TowerPlacement towerPlacement;
    private readonly LaneStats laneStats;
    private UnitTracker unitTracker;
    private IRotatable rotatable;
    
    [Header("Game Objects")]
    private readonly GameObject player;
    
    public LaneIdleState(GameObject go)
    {
        Debug.Log("Lane: IdleState");
        player = GameObject.Find("Player");

        laneStats = go.GetComponent<LaneStats>();
        towerPlacement = player.GetComponent<TowerPlacement>();
        rotatable = go.GetComponent<IRotatable>();
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>(); 
    }
    public override void Enter(GameObject go)
    {
        
    }

    public override void Update(GameObject go)
    {
        var closestTarget = unitTracker.FindClosestEnemy(go)?.transform;
        rotatable.RotateToTarget(go, closestTarget, 1);
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override LaneBaseState HandleInput(GameObject go)
    {
        if (laneStats.currentHealth <= 0 && towerPlacement.hasBeenPlaced)
        {
            return new LaneDeadState(go);
        }
        return null;
    }
}
