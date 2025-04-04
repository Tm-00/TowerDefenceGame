using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
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

    public bool EnemyDeath()
    {
        if (currentHealth <= 0)
        {
            unitTracker.EnemyTargets.Remove(gameObject);
            return true;
        }
        return false;
    }
}