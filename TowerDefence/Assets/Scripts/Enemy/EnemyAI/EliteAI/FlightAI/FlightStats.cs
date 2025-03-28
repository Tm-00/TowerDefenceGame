using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightStats : MonoBehaviour, IEnemyStats
{
    [Header("Flight Stats")] 
    private float maxHealth = 50f;
    private float currentHealth;
    
    [Header("Class")]
    private UnitTracker unitTracker; 
    private readonly FlightAttackHandler flightAttackHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void ApplyDamage(float amount)
    {
        currentHealth -= amount; 
        Debug.Log("Flight unit current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); 
        }
    }
    
    public void ApplyHeal(float amount)
    {
        currentHealth += amount;  
        currentHealth = Mathf.Min(currentHealth, maxHealth);  
        Debug.Log("Flight unit healed, current HP: " + currentHealth);
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    public void Die()
    {
        Debug.Log("Flight unit has died.");
        UnitTracker.EnemyTargets.Remove(gameObject);
    }
    
    public void ApplyBuff(int amount)
    {
        // currentHealth += amount;
        // rifleAttackHandler.damageAmount += amount;
    }
}
