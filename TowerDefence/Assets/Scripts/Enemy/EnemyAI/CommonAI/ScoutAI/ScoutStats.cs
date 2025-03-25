using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutStats : MonoBehaviour
{
    [Header("Scout Stats")] 
    private float maxHealth = 50f;
    private float currentHealth;
    private UnitTracker unitTracker; 
    
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
    }
}
