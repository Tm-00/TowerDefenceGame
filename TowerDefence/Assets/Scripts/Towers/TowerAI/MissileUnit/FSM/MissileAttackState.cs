using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MissileAttackState : MissileBaseState
{
    [Header("Turret Values")] 
    private const float RotationSpeed = 10.0f;
    
    [Header("Target Values")] 
    private Transform closestTarget;
    private readonly LayerMask missileLayerMask;
    private RaycastHit hit;
    
    [Header("Interface References")]
    private readonly IRotatable rotatable;
    
    [Header("Class References")]
    private readonly MissileAttackHandler missileAttackHandler;
    private readonly MissileStats missileStats;
    private readonly UnitTracker unitTracker;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private readonly float range;
    private readonly float aoeRadius; // AoE radius for damage
    
    
    public MissileAttackState(GameObject go)
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

        missileAttackHandler = go.GetComponent<MissileAttackHandler>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an MissileAttackHandler component!");
        }   
        
        missileStats = go.GetComponent<MissileStats>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an MissileStats component!");
        }
        
        unitTracker = gameManager.GetComponent<UnitTracker>();
        missileLayerMask = missileAttackHandler.layerMask;
        shootLocation = missileAttackHandler.shootLocation;
        range = missileAttackHandler.range;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Missile Unit: Attack State");
        closestTarget = unitTracker.FindClosestEnemy(go)?.transform;
    }

    public override void Update(GameObject go)
    {
        if (closestTarget!= null)
        {
            // rotate unit towards target
            rotatable.RotateToTarget(go, closestTarget, RotationSpeed);
            
            // check if the shootlocation is assigned 
            if (shootLocation != null)
            {
                // shoot a raycast at a max distance of the range relating to the unit
                if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, missileLayerMask))
                {
                    // confirm a target was hit then store it as a gameobject 
                    var targetHit = hit.collider.gameObject;
                    
                    // check that the target hit was the cloest target then perform attack methods
                    if (targetHit != null && targetHit == closestTarget.gameObject)
                    {
                        missileAttackHandler.Attack(targetHit);
                    }
                }
            }
        }
    }
    
    public override void Exit(GameObject go)
    {
        missileAttackHandler.ResetEnemyKilledStatus(); 
    }

    public override MissileBaseState HandleInput(GameObject go)
    {
        // if the unit kills an enemy or their target dies go to the locate state to find a new target 
        if (missileAttackHandler.IsEnemyKilled())
        {
            return new MissileLocateEnemyState(go);
        }
        if (missileStats.currentHealth <= 0)
        {
            return new MissileDeadState(go);
        }
        return null;
    }
}
