using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStats : MonoBehaviour, IUnitStats
{
    [Header("Turret Stats")] 
    private float maxHealth = 50f;
    private float currentHealth;
    
    [Header("Class")] 
    private UnitTracker unitTracker; 
    private TurretAttackHandler turretAttackHandler;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(" Turret current hp " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die(); 
        }
    }
    
    public void ApplyHeal(float amount)
    {
        currentHealth += amount;
        Debug.Log(" Turret current hp " + currentHealth);
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    public void Die()
    {
        Debug.Log("Turret unit has died.");
        UnitTracker.EnemyTargets.Remove(gameObject);
    }
    
    public void ApplyBuff(int amount)
    {
        currentHealth += amount;
        turretAttackHandler.damageAmount += amount;
    }
}
