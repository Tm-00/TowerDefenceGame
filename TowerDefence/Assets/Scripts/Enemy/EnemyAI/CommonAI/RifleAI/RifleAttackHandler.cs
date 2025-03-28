using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RifleAttackHandler : MonoBehaviour, IAttackHandler, IRotatable
{
    [Header("Rifle Values")] 
    public Transform shootLocation;

    [Header("Attack Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Values")] 
    internal int damageAmount = 50;
    public readonly float range = 100f;
    private bool enemyKilled;
    
    [Header("Cooldowns")]
    private readonly float cooldown = 5f;
    private float cooldownTime;
    
    void Awake()
    {
        layerMask = LayerMask.GetMask("Towers");
    }
    
    // Implement Attack from IAttackHandler
    public void Attack(GameObject targetHit)
    {
        if (targetHit != null)
        {
            IUnitStats targetStats = targetHit.GetComponent<IUnitStats>();
            if (cooldownTime <= 0)
            {
                cooldownTime = cooldown;
                targetStats?.ApplyDamage(damageAmount);
            }
            else
            {
                cooldownTime -= Time.deltaTime;
            }
            DeathCheck(targetHit);
        }
    }
    
    // Perform a death check and set enemyKilled to true if an enemy is killed
    public void DeathCheck(GameObject targethit)
    {
        IUnitStats targetHealth = targethit?.GetComponent<IUnitStats>();  
        
        if (targetHealth != null && targetHealth.IsDead())  
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;  // Set enemyKilled to true when an enemy is killed
        }
    }
    
    // check if the enemy has been killed
    public bool IsEnemyKilled()
    {
        return enemyKilled;
    }

    // reset the enemyKilled status
    public void ResetEnemyKilledStatus()
    {
        enemyKilled = false;
    }
}
