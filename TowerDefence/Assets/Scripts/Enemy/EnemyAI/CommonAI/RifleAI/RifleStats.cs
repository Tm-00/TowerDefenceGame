using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleStats : MonoBehaviour, IStats
{
    [Header("Rifle Stats")] 
    private readonly float maxHealth = 50f;
    private float currentHealth;
    
    [Header("Class")] 
    private UnitTracker unitTracker; 
    private readonly RifleAttackHandler rifleAttackHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Implement TakeDamage from IUnitStats
    public void ApplyDamage(float amount)
    {
        currentHealth -= amount; 
        Debug.Log("Rifle unit current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); 
        }
    }
    
    // Implement Heal from IUnitStats
    public void ApplyHeal(float amount)
    {
        currentHealth += amount;  
        currentHealth = Mathf.Min(currentHealth, maxHealth);  
        Debug.Log("Rifle unit healed, current HP: " + currentHealth);
    }

    // Implement IsDead from IUnitStats
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    public void Die()
    {
        Debug.Log("Rifle unit has died.");
        UnitTracker.EnemyTargets.Remove(gameObject);
    }

    // Implement ApplyBuff from IUnitStats
    public void ApplyBuff(int amount)
    {
        currentHealth += amount;
        rifleAttackHandler.damageAmount += amount;
    }
}