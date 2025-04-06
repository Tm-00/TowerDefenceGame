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
    
    [Header("Attack Values")] 
    internal int damageAmount = 50;
    public readonly float range = 100f;
    private bool enemyKilled;
    
    [Header("Cooldowns")]
    private readonly float cooldown = 5f;
    private float cooldownTime;
    
    
    private void Awake()
    {
        allyLayerMask = LayerMask.GetMask("Enemies");
        unitLayerMask = LayerMask.GetMask("Towers");
    }

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
    
    public void Heal(GameObject targetHit)
    {
        throw new System.NotImplementedException();
    }
    
    public void Buff(GameObject targetHit)
    {
        throw new System.NotImplementedException();
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
