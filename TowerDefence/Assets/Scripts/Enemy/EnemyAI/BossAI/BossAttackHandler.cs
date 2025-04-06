using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackHandler : MonoBehaviour, IAttackHandler, IRotatable
{
    [Header("Boss Values")] 
    public Transform shootLocation;

    [Header("Attack Foundations")] 
    public LayerMask allyLayerMask;
    public LayerMask unitLayerMask;
    private RaycastHit hit;
    private BossStats bossStats;
    
    [Header("Attack Values")] 
    public readonly float range = 100f;
    private bool enemyKilled;
    
    [Header("Heal Values")]
    public readonly float healRange = 20f;
    private readonly float aoeRadius = 10f;
    
    [Header("Cooldowns")]
    private readonly float cooldown = 5f;
    private float cooldownTime;
    
    
    private void Awake()
    {
        allyLayerMask = LayerMask.GetMask("Enemies");
        unitLayerMask = LayerMask.GetMask("Towers");
        bossStats = GetComponent<BossStats>();
    }

    public void Attack(GameObject targetHit)
    {
        if (targetHit != null)
        {
            IUnitStats targetStats = targetHit.GetComponent<IUnitStats>();
            if (cooldownTime <= 0)
            {
                cooldownTime = cooldown;
                targetStats?.ApplyDamage(bossStats.damageAmount);
            }
            else
            {
                cooldownTime -= Time.deltaTime;
            }
            DeathCheck(targetHit);
        }
    } 
    
    public void Heal(GameObject targetHit)
    {
        if (targetHit != null)
        {
            ApplyAoeHeal(targetHit.transform.position);
        }
    }
    
    private void ApplyAoeHeal(Vector3 aoeCenter)
    {
        // Find all colliders within the aoeRadius around the hit point
        Collider[] hitColliders = Physics.OverlapSphere(aoeCenter, aoeRadius, allyLayerMask);
        
        HashSet<GameObject> uniqueAllies = new HashSet<GameObject>();
        // Loop through each object in the radius
        foreach (var hitCollider in hitColliders)
        {
            uniqueAllies.Add(hitCollider.gameObject);
        }

        if (cooldownTime <= 0)
        {
            cooldownTime = cooldown;
            // Loop through each unique enemy and apply damage
            foreach (GameObject targetHit in uniqueAllies)
            {
                AllyAoeHeal(targetHit);
                DeathCheck(targetHit);
            }
        }
        else
        {
            cooldownTime -= Time.deltaTime;
        }
        //rays for visualising and debugging 
        Debug.DrawRay(aoeCenter, Vector3.up * 2f, Color.blue, 2.0f); // Draw the AoE center
        Debug.DrawLine(aoeCenter, aoeCenter + Vector3.up * 2f, Color.yellow, 2.0f);
    }
    
    private void AllyAoeHeal(GameObject targetHit)
    {
        if (targetHit != null)
        {
            IEnemyStats targetStats = targetHit.GetComponent<IEnemyStats>();
            cooldownTime = cooldown;
            targetStats?.ApplyHeal(bossStats.healAmount);
        }
    }

    
    public void Buff(GameObject targetHit)
    {
        
    }

    public bool IsEnemyKilled()
    {
        return enemyKilled;
    }

    public void ResetEnemyKilledStatus()
    {
        enemyKilled = false;
    }

    public void DeathCheck(GameObject targetHit)
    {
        IUnitStats targetHealth = targetHit?.GetComponent<IUnitStats>();  
        
        if (targetHealth != null && targetHealth.IsDead())  
        {
            enemyKilled = true;  // Set enemyKilled to true when an enemy is killed
        }
    }
}
