using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class RifleAttackState : RifleBaseState
{
    [Header("Rifle Values")] 
    private readonly float rotationSpeed = 10.0f;
    private NavMeshAgent agent;
    private GameObject enemy;
    
    [Header("Target Values")] 
    private Transform coreNodePosition;
    private Transform closestTarget;
    private LayerMask rifleLayerMask;
    private RaycastHit hit;
    
    [Header("Class References")]
    private IAttackHandler attackHandler; 
    private IRotatable rotatable;
    private readonly RifleAttackHandler rifleAttackHandler;
    private readonly UnitTracker unitTracker;
    private readonly RifleStats rifleStats;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    private bool enemyKilled;
    
    [Header("Attack Values")]
    private readonly float range;
    
    public RifleAttackState(GameObject go)
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
        
        rifleAttackHandler = go.GetComponent<RifleAttackHandler>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an RifleAttackHandler component!");
        }

        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
        
        rifleStats = go.GetComponent<RifleStats>();
        rifleLayerMask = rifleAttackHandler.layerMask;
        shootLocation = rifleAttackHandler.shootLocation;
        range = rifleAttackHandler.range;
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        enemy = go;
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        Debug.Log("Rifle Drone: Attack State");
        coreNodePosition = unitTracker.UnitTargets[0].transform;
    }

    // Update
    public override void Update(GameObject go)
    {
        closestTarget = unitTracker.FindClosestUnit(enemy);
        
        if (closestTarget != null)
        {
            agent.destination = closestTarget.transform.position;
            // rotate unit towards target
            rotatable.RotateToTarget(go, closestTarget, rotationSpeed);

            if (shootLocation != null)
            {
                if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, rifleLayerMask))
                {
                    // confirm a target was hit then store it as a gameobject 
                    var targetHit = hit.collider.gameObject;
                    
                    // check that the target hit was the cloest target then perform attack methods
                    if (targetHit != null && targetHit == closestTarget.gameObject)
                    {
                        rifleAttackHandler.Attack(targetHit);
                    }
                }
            }
        }
    }
    
    // Exit
    public override void Exit(GameObject go)
    {
        rifleAttackHandler.ResetEnemyKilledStatus(); 
    }
    
    // input
    public override RifleBaseState HandleInput(GameObject go)
    {
        if (rifleAttackHandler.IsEnemyKilled())
        {
            return new RifleMoveState(go);
        }
        if (rifleStats.currentHealth <= 0)
        {
            return new RifleDeadState(go);
        }
        // if the unit kills an enemy or their target dies go to the move state to find a new target 
        //return rifleAttackHandler.IsEnemyKilled() ? new RifleMoveState(go) : null;
        return null;
    }
}
    

