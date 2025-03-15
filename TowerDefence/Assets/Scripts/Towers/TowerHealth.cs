using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    public float currentHealth;
    private UnitTracker unitTracker; 
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Death();
        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject + "current hp " + currentHealth);
    }

    public bool Death()
    {
        if (currentHealth <= 0)
        {
            UnitTracker.UnitTargets.Remove(gameObject);
            return true;
        }
        return false;
    }
}
