using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStats : MonoBehaviour, IStats
{
    [Header("Laser Stats")] 
    private float maxHealth = 50f;
    private float currentHealth;
    
    [Header("Class")] 
    private UnitTracker unitTracker; 
    private LaserAttackHandler laserAttackHandler;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(" Turret current hp " + currentHealth);
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
        laserAttackHandler.damageAmount += amount;
    }
}
