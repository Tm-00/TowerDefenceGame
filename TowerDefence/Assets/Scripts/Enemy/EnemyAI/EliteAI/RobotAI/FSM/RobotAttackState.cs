using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class RobotAttackState : RobotBaseState
{
    
    [Header("Robot Values")] 
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
    private readonly RobotAttackHandler robotAttackHandler;
    
    public RobotAttackState(GameObject go)
    {
        robotAttackHandler = go.GetComponent<RobotAttackHandler>();
        agent = go.GetComponent<NavMeshAgent>();
        coreNodePosition = UnitTracker.UnitTargets[0].transform;
        
        layerMask = robotAttackHandler.layerMask;
        shootLocation = robotAttackHandler.shootLocation;
        range = robotAttackHandler.range;
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        Debug.Log("Robot Drone: Attack State");
    }

  public override void Update(GameObject go)
    {
        closestTarget = UnitTracker.FindClosestWallUnit(agent)?.transform;
        if (closestTarget != null)
        {
            robotAttackHandler.RotateUnitToTarget(go, closestTarget, rotationSpeed);
            if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
            {
                // confirm a target was hit then store it as a gameobject 
                var targetHit = hit.collider.gameObject;
                    
                // check that the target hit was the cloest target then perform attack methods
                if (targetHit != null && targetHit == closestTarget.gameObject)
                {
                    robotAttackHandler.EnemyAttack(targetHit);
                }
            }
        }
    }
  
    // Exit
    public override void Exit(GameObject go)
    {
    
    }
    
    // input
    public override RobotBaseState HandleInput(GameObject go)
    {
        if (enemyKilled)
        {
            // if the target died find new target
            return new RobotMoveState(go);
        }
        return null;
    }
}
    

