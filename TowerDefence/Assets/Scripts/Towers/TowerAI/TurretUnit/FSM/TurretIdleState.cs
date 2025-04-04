using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIdleState : TurretBaseState
{
    [Header("Class References")]
    private readonly UnitTracker unitTracker;
    private readonly TowerPlacement towerPlacement;
    
    [Header("Game Objects")]
    private readonly GameObject player;
    private readonly GameObject gameManager;
    
    public TurretIdleState(GameObject go)
    {
        Debug.Log("Turret: IdleState");
        
        gameManager = GameObject.Find("GameManager");
        player = GameObject.Find("Player");
        
        unitTracker = gameManager.GetComponent<UnitTracker>();
        towerPlacement = player.GetComponent<TowerPlacement>();
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

    public override TurretBaseState HandleInput(GameObject go)
    {
        if (unitTracker.EnemyTargets != null && towerPlacement.hasBeenPlaced)
        {
            if (unitTracker.EnemyTargets.Count >= 1)
            {
                return new TurretLocateEnemyState(go);
            }
        }
        return null;
    }
}
