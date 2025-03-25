using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackState : MeleeBaseState
{
    [Header("Turret Values")] 
    private readonly float rotationSpeed = 1.0f;
    
    [Header("Target Values")] 
    private Transform closestTarget;
    private readonly LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private readonly float range;
    private readonly float aoeRadius; // AoE radius for damage
    
    [Header("Class")]
    private readonly MeleeAttackHandler meleeAttackHandler;
    

    public MeleeAttackState(GameObject go)
    {
        meleeAttackHandler = go.GetComponent<MeleeAttackHandler>();
        if (meleeAttackHandler == null)
        {
            meleeAttackHandler = go.AddComponent<MeleeAttackHandler>();
        }
        layerMask = meleeAttackHandler.layerMask;
        shootLocation = meleeAttackHandler.shootLocation;
        range = meleeAttackHandler.range;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Melee: Attack State");
    }

    public override void Update(GameObject go)
    {
        // Find and identify the closest enemy
        closestTarget = UnitTracker.FindClosestEnemy(go)?.transform;
        if (closestTarget != null)
        {
            // rotate unit towards target
            meleeAttackHandler.RotateUnitToTarget(go, closestTarget, rotationSpeed);
            // check if the shootlocation is assigned 
            if (shootLocation != null)
            {
                // shoot a raycast at a max distance of the range relating to the unit
                if (Physics.Raycast(shootLocation.position, go.transform.forward, out hit, range, layerMask))
                {
                    meleeAttackHandler.ApplyAoeDamage(hit.point);
                }
            }
        }
    }
    
    public override void Exit(GameObject go)
    {
        
    }

    public override MeleeBaseState HandleInput(GameObject go)
    {
        return null;
    }
}
