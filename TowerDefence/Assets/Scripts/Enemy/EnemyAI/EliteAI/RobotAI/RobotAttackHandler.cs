using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAttackHandler : MonoBehaviour
{
    
    // TODO take cloest target as a parameter 
    //private Transform closestTarget;

    [Header("Robot Values")] 
    private NavMeshAgent agent;

    public Transform shootLocation;
    
    [Header("Target Values")] 
    private Transform closestTarget;
    
    [Header("Attack Foundations")]
    private readonly LayerMask layerMask = LayerMask.GetMask("Towers");
    private RaycastHit hit;
    
    [Header("Attack Values")]
    private readonly int damageAmount = 50;

    public readonly float range = 100f;
    private bool enemyKilled;
    
    [Header("Cooldowns")]
    private readonly float cooldown = 5f;
    private float cooldownTime;
    
    
    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackUnit(GameObject targethit)
    {
        if (targethit != null)
        {
            TowerHealth targetHealth = targethit.GetComponent<TowerHealth>();
            if (cooldownTime <= 0)
            {
                cooldownTime = cooldown;
                targetHealth.TakeDamage(damageAmount);
            }
            else
            {
                cooldownTime -= Time.deltaTime;
                //Debug.Log("active cd time " + cooldownTime);
            }
            //Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
        }
    }
    
}
