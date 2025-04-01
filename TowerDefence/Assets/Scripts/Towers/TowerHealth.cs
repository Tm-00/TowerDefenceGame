using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    public float currentHealth;
    public float damage;
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
    
    public void TakeHeal(float amount)
    {
        currentHealth += amount;
        Debug.Log(gameObject + "current hp " + currentHealth);
    }
    
    public void TakeBuff(float amount)
    {
        damage += amount;
        Debug.Log(gameObject + "current hp " + currentHealth);
    }
    

    public bool Death()
    {
        if (currentHealth <= 0)
        {
            unitTracker.UnitTargets.Remove(gameObject);
            return true;
        }
        return false;
    }
}
