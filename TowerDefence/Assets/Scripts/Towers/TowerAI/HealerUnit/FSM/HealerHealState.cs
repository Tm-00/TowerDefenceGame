using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealerHealState : HealerBaseState
{
    [Header("Turret Values")] 
    private const float RotationSpeed = 1.0f;
    
    [Header("Target Values")] 
    private Transform closestTarget;
    private readonly LayerMask healLayerMask;
    private RaycastHit hit;
    
    [Header("Interface References")]
    private readonly IRotatable rotatable;
    
    [Header("Class References")]
    private readonly HealerHealHandler healerHealHandler;
    private readonly HealerStats healerStats;
    private readonly UnitTracker unitTracker;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private readonly float range;
    private readonly float aoeRadius; 

    public HealerHealState(GameObject go)
    {
        var attackHandler = go.GetComponent<IAttackHandler>();
        var gameManager = GameObject.Find("GameManager");
        
        if (attackHandler == null)
        {
            Debug.LogError("GameObject is missing an IAttackHandler component!");
        }
        
        rotatable = go.GetComponent<IRotatable>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an IRotatable component!");
        }

        healerHealHandler = go.GetComponent<HealerHealHandler>();
        if (healerHealHandler == null)
        {
            Debug.LogError("GameObject is missing an HealHandler component!");
        }  
        
        healerStats = go.GetComponent<HealerStats>();
        if (healerStats == null)
        {
            Debug.LogError("GameObject is missing an HealerStats component!");
        }   
        
        unitTracker = gameManager.GetComponent<UnitTracker>();
        healLayerMask = healerHealHandler.layerMask;
        shootLocation = healerHealHandler.shootLocation;
        range = healerHealHandler.range;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Healer: Heal State");
    }

    public override void Update(GameObject go)
    {
        closestTarget = unitTracker?.FindClosestFloorUnit(go).transform;
        
        if (closestTarget != null)
        {
            // rotate unit towards target
            rotatable.RotateToTarget(go, closestTarget, RotationSpeed);
            
            // check if the shootlocation is assigned 
            if (shootLocation != null)
            {
                // shoot a raycast at a max distance of the range relating to the unit
                if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, healLayerMask))
                {
                    // confirm a target was hit then store it as a gameobject 
                    var targetHit = hit.collider.gameObject;
                    
                    // check that the target hit was the cloest target then perform attack methods
                    if (targetHit != null && targetHit == closestTarget.gameObject)
                    {
                        healerHealHandler.Attack(targetHit);
                    }
                }
            }
        }
    }
    
    public override void Exit(GameObject go)
    {
        healerHealHandler.ResetEnemyKilledStatus(); 
    }

    public override HealerBaseState HandleInput(GameObject go)
    {
        // if the unit kills an enemy or their target dies go to the locate state to find a new target 
        if (healerHealHandler.IsEnemyKilled())
        {
            return new HealerLocateAllyState(go);
        }
        if (healerStats.currentHealth <= 0)
        {
            return new HealerDeadState(go);
        }
        return null;
    }
}
