using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStats : MonoBehaviour
{
    [Header("Turret Stats")] 
    private float maxHealth = 50f;
    private float currentHealth;
    
    
    [Header("Class")] 
    private UnitTracker unitTracker; 
    private readonly TurretAttackHandler turretAttackHandler;
    
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
    
    public void UnitBuffed(int amount)
    {
        currentHealth += amount;
        turretAttackHandler.damageAmount += amount;
    }
}
