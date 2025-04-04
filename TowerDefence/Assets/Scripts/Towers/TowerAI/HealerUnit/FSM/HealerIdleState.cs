using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerIdleState : HealerBaseState
{
    [Header("Class References")]
    private readonly UnitTracker unitTracker;
    private readonly TowerPlacement towerPlacement;
    
    [Header("Game Objects")]
    private readonly GameObject player;
    private readonly GameObject gameManager;
    
    public HealerIdleState(GameObject go)
    {
        Debug.Log("Healer: IdleState");
        
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

    public override HealerBaseState HandleInput(GameObject go)
    {
        if (unitTracker.UnitTargets != null && towerPlacement.hasBeenPlaced)
        {
            if (unitTracker.UnitTargets.Count >= 1)
            {
                return new HealerLocateAllyState(go);
            }
        }
        return null;
    }
}
