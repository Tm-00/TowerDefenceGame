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
    
    [Header("Attack Values")]
    private float range;
    
    // reference to the core node 
    private LayerMask layerMask;
    private RaycastHit hit;

    
    public RifleAttackState(GameObject go)
    {
        // assign variables 
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = UnitTracker.UnitTargets[0].transform;
        
        RifleAttackHandler rifleAttackHandler = go.AddComponent<RifleAttackHandler>();
        
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
        RifleAttackHandler rifleAttackHandler = go.AddComponent<RifleAttackHandler>();
        
        closestTarget = UnitTracker.FindClosestWallUnit(agent)?.transform;
        
        if (closestTarget != null)
        {
            RotateUnitToTarget(go);
            
            if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
            {
                GameObject targethit = hit.collider.gameObject;
                if (targethit != null && targethit == closestTarget.gameObject)
                {
                    rifleAttackHandler.RifleAttackUnit(targethit);
                }
                TowerHealth targetHealth = targethit.GetComponent<TowerHealth>();
                if (targetHealth.Death())
                {
                    ObjectPoolManager.ReturnObjectToPool(targethit);
                    enemyKilled = true;
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
    
    private void RotateUnitToTarget(GameObject go)
    {
        Vector3 targetDirection = new Vector3(closestTarget.position.x - go.transform.position.x, 0,
            closestTarget.position.z - go.transform.position.z).normalized;
        float singlestep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(go.transform.forward, targetDirection, singlestep, 0.0f);
        go.transform.localRotation = Quaternion.LookRotation(newDirection);
        DrawRay(targetDirection, go);
    }

    private void DrawRay(Vector3 targetDirection, GameObject go)
    {
        // Debug lines to visualize the directions
        Debug.DrawRay(shootLocation.position, targetDirection * 10f, Color.red);  // Red line pointing towards target
        Debug.DrawRay(shootLocation.transform.position, go.transform.forward * 10f, Color.green); // Green line showing current forward direction
    }
}
    

