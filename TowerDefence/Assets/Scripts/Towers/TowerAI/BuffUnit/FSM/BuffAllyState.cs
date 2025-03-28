using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuffAllyState : BuffBaseState
{
    [Header("Buff Values")] 
    private readonly float rotationSpeed = 1.0f;
    
    [Header("Target Values")] 
    private Transform closestAlly;
    private readonly LayerMask buffLayerMask;
    private RaycastHit hit;
    
    [Header("Class References")]
    private IAttackHandler attackHandler; 
    private IRotatable rotatable;
    private readonly BuffHandler buffHandler;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private readonly float range;
    private readonly float aoeRadius; // AoE radius for buffs
    

    public BuffAllyState(GameObject go)
    {
        attackHandler = go.GetComponent<IAttackHandler>();
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
        
        buffLayerMask = buffHandler.layerMask;
        shootLocation = buffHandler.shootLocation;
        range = buffHandler.range;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Healer: Heal State");
        closestAlly = UnitTracker.FindClosestAlly(go)?.transform;
    }

    public override void Update(GameObject go)
    {
        // rotate unit towards target
        rotatable.RotateToTarget(go, closestAlly, rotationSpeed);
            
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

    public override void Exit(GameObject go)
    {
        buffHandler.ResetEnemyKilledStatus(); 
    }

    public override BuffBaseState HandleInput(GameObject go)
    {
        // if the unit kills an enemy or their target dies go to the locate state to find a new target 
        return buffHandler.IsEnemyKilled() ? new BuffLocateAllyState(go) : null;
    }
}
