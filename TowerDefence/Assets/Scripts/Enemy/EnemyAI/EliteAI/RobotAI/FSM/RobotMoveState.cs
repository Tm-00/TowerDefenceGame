using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class RobotMoveState : RobotBaseState
{
    
    // will be able to reference itself
    private NavMeshAgent agent;

    private Vector3 closestTarget;

    private bool allunitsdead;
    
    private GameObject enemy;

    private RobotStats robotStats;
        
    // reference to the core node 
    private Transform coreNodePosition;
    
    private float currentSpeed;
    
    private readonly UnitTracker unitTracker;
    
    [Header("Animator")] 
    private Animator anim;
    private int speedHash = Animator.StringToHash("RobotSpeed");
    
    // Constructor.
    public RobotMoveState(GameObject go)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
        
        // assign variables 
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = unitTracker.UnitTargets[0].transform;
        enemy = go;
        robotStats = go.GetComponent<RobotStats>();
        anim = go.GetComponent<Animator>();
        if (anim != null)
        {
            Debug.Log("success");
        }
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        anim.SetFloat(speedHash, 1);
        Debug.Log("Robot Drone: Move State");
    }
    
    // Update
    public override void Update(GameObject go)
    {
        currentSpeed = agent.velocity.magnitude;
        anim.SetFloat(speedHash, currentSpeed);
        FilterTargets();
    }
    
    // Exit
    public override void Exit(GameObject gameObject)
    { 
        anim.SetFloat(speedHash, 0);
    }
    
    // Input
    public override RobotBaseState HandleInput(GameObject go)
    {
        // Move -> Attack
        if (Vector3.Distance(agent.transform.position, closestTarget) <= 25 && allunitsdead != true)
        {
            return new RobotAttackState(go);
        }
        if (Vector3.Distance(agent.transform.position, coreNodePosition.transform.position) <= 5)
        {
            return new RobotFinishedState(go);
        }
        if (robotStats.currentHealth <= 0)
        {
            return new RobotDeadState(go);
        }
        return null;
    }

    private void FilterTargets()
    {
        var cloestEnemy = unitTracker.FindClosestWallUnit(enemy);
        if (cloestEnemy != null && unitTracker.UnitTargets.Count > 1)
        {
            closestTarget = unitTracker.FindClosestWallUnit(enemy).transform.position;
            agent.destination = closestTarget;
            allunitsdead = false;
        }
        else
        {
            agent.destination = coreNodePosition.transform.position;
            allunitsdead = true;
        }
    }
}