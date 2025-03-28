using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeStats : MonoBehaviour, IUnitStats
{
    [Header("Melee Stats")] 
    private float maxHealth = 50f;
    private float currentHealth;
    
    [Header("Class")] 
    private UnitTracker unitTracker; 
    private MeleeAttackHandler meleeAttackHandler;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(" Melee current hp " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die(); 
        }
    }
    
    public void ApplyHeal(float amount)
    {
        currentHealth += amount;
        Debug.Log(" Melee current hp " + currentHealth);
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    public void Die()
    {
        Debug.Log("Melee unit has died.");
        UnitTracker.EnemyTargets.Remove(gameObject);
    }
    
    public void ApplyBuff(int amount)
    {
        currentHealth += amount;
        meleeAttackHandler.damageAmount += amount;
    }
}   

