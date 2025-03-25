using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleStats : MonoBehaviour
{
    [Header("Rifle Stats")] 
    private float maxHealth = 50f;
    private float currentHealth;
    
    [Header("Class")] 
    private UnitTracker unitTracker; 
    private readonly RifleAttackHandler rifleAttackHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDeath();
    }

    public void EnemyTakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(" drone current hp " + currentHealth);
    }
    
    public void EnemyTakeHeal(float amount)
    {
        currentHealth += amount;
        Debug.Log(" drone current hp " + currentHealth);
    }

    public bool EnemyDeath()
    {
        if (currentHealth <= 0)
        {
            UnitTracker.EnemyTargets.Remove(gameObject);
            return true;
        }
        return false;
    }
    
    public void EnemyBuffed(int amount)
    {
        currentHealth += amount;
        rifleAttackHandler.damageAmount += amount;
    }
}
