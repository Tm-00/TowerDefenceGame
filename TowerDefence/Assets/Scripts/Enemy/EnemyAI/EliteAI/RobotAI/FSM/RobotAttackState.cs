using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class RobotAttackState : RobotBaseState
{
    
    [Header("Robot Values")] 
    private readonly float rotationSpeed = 10.0f;
    private NavMeshAgent agent;
    private GameObject enemy;
    
    [Header("Target Values")] 
    private Transform coreNodePosition;
    private Transform closestTarget;
    private LayerMask robotLayerMask;
    private RaycastHit hit;
    
    [Header("Class References")]
    private IAttackHandler attackHandler; 
    private IRotatable rotatable;
    private readonly RobotAttackHandler robotAttackHandler;
    private readonly UnitTracker unitTracker;
    private readonly RobotStats robotStats;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    private bool enemyKilled;
    
    [Header("Attack Values")]
    private readonly float range;
    
    [Header("Animator")] 
    [SerializeField] private Animator anim;
    private int shootTriggerHash = Animator.StringToHash("RobotShoot");
    private int speedHash = Animator.StringToHash("RobotSpeed");
    
    public RobotAttackState(GameObject go)
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
        
        robotAttackHandler = go.GetComponent<RobotAttackHandler>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an RobotAttackHandler component!");
        }
        
        anim = go.GetComponent<Animator>();
        if (anim != null)
        {
            Debug.Log("success");
        }
        
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>(); 
        robotStats = go.GetComponent<RobotStats>();
        robotLayerMask = robotAttackHandler.layerMask;
        shootLocation = robotAttackHandler.shootLocation;
        range = robotAttackHandler.range;
        enemy = go;
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        anim.SetFloat(speedHash, 1);
        Debug.Log("Robot Drone: Attack State");
        agent = go.GetComponent<NavMeshAgent>();
        coreNodePosition = unitTracker.UnitTargets[0].transform;
        closestTarget = unitTracker.FindClosestUnit(enemy)?.transform;
    }

  public override void Update(GameObject go)
    {
        if (closestTarget != null)
        {
            // rotate unit towards target
            rotatable.RotateToTarget(go, closestTarget, rotationSpeed);

            if (shootLocation != null)
            {
                if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, robotLayerMask))
                {
                    // confirm a target was hit then store it as a gameobject 
                    var targetHit = hit.collider.gameObject;
                    
                    // check that the target hit was the cloest target then perform attack methods
                    if (targetHit != null && targetHit == closestTarget.gameObject)
                    {
                        //anim.SetTrigger(shootTriggerHash);
                        robotAttackHandler.Attack(targetHit);
                    }
                }
            }
        }
        Debug.DrawRay(shootLocation.transform.position, shootLocation.transform.forward * 10f, Color.green); // Green line showing current forward direction

    }
  
    // Exit
    public override void Exit(GameObject go)
    {
        robotAttackHandler.ResetEnemyKilledStatus(); 
        anim.SetFloat(speedHash, 0);
    }
    
    // input
    public override RobotBaseState HandleInput(GameObject go)
    {
        if (robotStats.currentHealth <= 0)
        {
            return new RobotDeadState(go);
        }
        // if the unit kills an enemy or their target dies go to the move state to find a new target 
        return robotAttackHandler.IsEnemyKilled() ? new RobotMoveState(go) : null;
    }
}
    

