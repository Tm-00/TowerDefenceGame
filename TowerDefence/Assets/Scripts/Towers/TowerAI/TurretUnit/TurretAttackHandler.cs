using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttackHandler : MonoBehaviour
{
    [Header("Unit Values")] 
    public Transform shootLocation;
    
    [Header("Target Values")] 
    private Transform closestTarget;

    [Header("Attack Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Values")]
    private readonly int damageAmount = 50;
    public readonly float range = 100f;
    private bool enemyKilled;
    
    [Header("Cooldowns")]
    public float cooldown = 5f;
    private float cooldownTime;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        layerMask = LayerMask.GetMask("Enemies");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnitAttack(GameObject targethit)
    {
        if (targethit != null)
        {
            FlightStats flightStats = targethit.GetComponent<FlightStats>();
         
            if (cooldownTime <= 0)
            {
                cooldownTime = cooldown;
                flightStats.EnemyTakeDamage(damageAmount);
            }
            else
            {
                cooldownTime -= Time.deltaTime;
            }
        }
    }
}
