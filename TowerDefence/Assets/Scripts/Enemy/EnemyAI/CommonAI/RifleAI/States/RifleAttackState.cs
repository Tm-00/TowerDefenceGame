using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class RifleAttackState : RifleBaseState
{
    [Header("Rifle Values")] 
    private readonly float rotationSpeed = 1.0f;
    private NavMeshAgent agent;
    
    [Header("Target Values")] 
    private Transform coreNodePosition;
    private Transform closestTarget;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    private bool enemyKilled;
    private LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Values")]
    private float range;
    
    [Header("Class")]
    private readonly RifleAttackHandler rifleAttackHandler;
    
    public RifleAttackState(GameObject go)
    {
        rifleAttackHandler = go.GetComponent<RifleAttackHandler>();
        agent = go.GetComponent<NavMeshAgent>();
        coreNodePosition = UnitTracker.UnitTargets[0].transform;
        
        layerMask = rifleAttackHandler.layerMask;
        shootLocation = rifleAttackHandler.shootLocation;
        range = rifleAttackHandler.range;
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        Debug.Log("Rifle Drone: Attack State");
    }

    // Update
    public override void Update(GameObject go)
    {
        closestTarget = UnitTracker.FindClosestWallUnit(agent)?.transform;
        if (closestTarget != null)
        {
            rifleAttackHandler.RotateUnitToTarget(go, closestTarget, rotationSpeed);
            if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
            {
                // confirm a target was hit then store it as a gameobject 
                var targetHit = hit.collider.gameObject;
                    
                // check that the target hit was the cloest target then perform attack methods
                if (targetHit != null && targetHit == closestTarget.gameObject)
                {
                    rifleAttackHandler.EnemyAttack(targetHit);
                }
            }
        }
    }
    
    // Exit
    public override void Exit(GameObject go)
    {
    
    }
    
    // input
    public override RifleBaseState HandleInput(GameObject go)
    {
        if (enemyKilled)
        {
            return new RifleMoveState(go);
        }
        return null;
    }
}
    

