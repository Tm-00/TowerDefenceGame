using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerStats : MonoBehaviour
{
    [Header("Healer Stats")] 
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
        UnitDeath();
    }

    public void UnitTakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(" drone current hp " + currentHealth);
    }

    public void UnitTakeHeal(float amount)
    {
        currentHealth += amount;
        Debug.Log(" drone current hp " + currentHealth);
    }
    
    public bool UnitDeath()
    {
        if (currentHealth <= 0)
        {
            UnitTracker.UnitTargets.Remove(gameObject);
            return true;
        }
        return false;
    }
}
