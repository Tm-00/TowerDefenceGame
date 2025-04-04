using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuffAllyState : BuffBaseState
{
    [Header("Buff Values")] 
    private const float RotationSpeed = 1.0f;
    
    [Header("Target Values")] 
    private Transform closestAlly;
    private readonly LayerMask buffLayerMask;
    private RaycastHit hit;

    [Header("Interface References")]
    private readonly IRotatable rotatable;
    
    [Header("Class References")]
    private readonly BuffHandler buffHandler;
    private readonly BuffStats buffStats;
    private readonly UnitTracker unitTracker;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private readonly float range;
    private readonly float aoeRadius; // AoE radius for buffs
    

    public BuffAllyState(GameObject go)
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

        buffHandler = go.GetComponent<BuffHandler>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an BuffHandler component!");
        }
        
        buffStats = go.GetComponent<BuffStats>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an BuffStats component!");
        }
        
        unitTracker = gameManager.GetComponent<UnitTracker>();
        buffLayerMask = buffHandler.layerMask;
        shootLocation = buffHandler.shootLocation;
        range = buffHandler.range;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Healer: Heal State");
        closestAlly = unitTracker.FindClosestUnit(go)?.transform;
    }

    public override void Update(GameObject go)
    {
        if (closestAlly != null)
        {
            // rotate unit towards target
            rotatable.RotateToTarget(go, closestAlly, RotationSpeed);
            
            // check if the shootlocation is assigned 
            if (shootLocation != null)
            {
                // shoot a raycast at a max distance of the range relating to the unit
                if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, buffLayerMask))
                {
                    // confirm a target was hit then store it as a gameobject 
                    var targetHit = hit.collider.gameObject;
                    
                    // check that the target hit was the cloest target then perform attack methods
                    if (targetHit != null && targetHit == closestAlly.gameObject)
                    {
                        buffHandler.Attack(targetHit);
                    }
                }
            }
        }
    }

    public override void Exit(GameObject go)
    {
        buffHandler.ResetEnemyKilledStatus(); 
    }

    public override BuffBaseState HandleInput(GameObject go)
    {
        // if the unit kills an enemy or their target dies go to the locate state to find a new target 
        if (buffHandler.IsEnemyKilled())
        {
            return new BuffLocateAllyState(go);
        }
        if (buffStats.currentHealth <= 0)
        {
            return new BuffDeadState(go);
        }
        return null;
    }
}
