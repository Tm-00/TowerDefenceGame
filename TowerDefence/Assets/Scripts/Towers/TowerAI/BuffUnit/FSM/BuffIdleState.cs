using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIdleState : BuffBaseState
{
    [Header("Class References")]
    private readonly UnitTracker unitTracker;
    private readonly TowerPlacement towerPlacement;
    
    [Header("Game Objects")]
    private readonly GameObject player;
    private readonly GameObject gameManager;
    
    public BuffIdleState(GameObject go)
    {
        Debug.Log("Buff: IdleState");
        
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

    public override BuffBaseState HandleInput(GameObject go)
    {
        if (unitTracker.UnitTargets != null && towerPlacement.hasBeenPlaced)
        {
            if (unitTracker.UnitTargets.Count >= 1)
            {
                return new BuffLocateAllyState(go);
            }
        }
        return null;
    }
}
