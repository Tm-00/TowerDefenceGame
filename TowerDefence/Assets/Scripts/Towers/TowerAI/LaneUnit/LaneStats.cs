using System;
using UnityEngine;
using UnityEngine.UI;

public class LaneStats : MonoBehaviour, IUnitStats, IStats, IRotatable
{
    [Header("Lane Stats")] 
    private float maxHealth = 50f;

    internal float currentHealth;
    private float scoreValue = 5;
    private float resourceValue = 10;
    
    [Header("Class")] 
    private UnitTracker unitTracker; 
    private ScoreManager scoreManager;
    private ResourceManager resourceManager;
    
    [Header("Health Bar")]
    public Image healthBar;
    
    [Header("Animator")] 
    private Animator anim;
    AnimatorStateInfo currentStateInfo;
    private int laneActionHash = Animator.StringToHash("LaneAction");
    
    public bool hasBeenPlaced { get; set; }
    
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        resourceManager = FindObjectOfType<ResourceManager>();
        unitTracker = FindObjectOfType<UnitTracker>();
        currentHealth = maxHealth;
        hasBeenPlaced = false;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        var closestEnemy = unitTracker.FindClosestEnemy(gameObject);
    }

    public void ApplyDamage(float amount)
    {
        anim.SetTrigger(laneActionHash);
        currentHealth -= amount;
        Debug.Log(" Lane current hp " + currentHealth);
        healthBar.fillAmount = currentHealth / maxHealth;
        
        if (currentHealth <= 0)
        {
            Die(); 
        }
    }
    
    public void ApplyHeal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.fillAmount = currentHealth / maxHealth; 
        Debug.Log("Lane current hp " + currentHealth);
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void OnPlacement()
    {
        resourceManager.SubtractResource(resourceValue);
        hasBeenPlaced = true; 
    }

    public void Die()
    {
        Debug.Log("Lane unit has died.");
        scoreManager.RemoveScore(scoreValue);
        unitTracker.EnemyTargets.Remove(gameObject);
        resourceManager.AddResource(resourceValue);
        hasBeenPlaced = false;
    }
    
    public void ApplyBuff(int amount)
    {
        maxHealth = Mathf.Clamp(maxHealth + amount + 5, 0, 65);
        Debug.Log("new max health " + maxHealth);
    }
    
    public void OnSpawn()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth;
    }
    
    public bool CanSpawn()
    {
        if (resourceManager.currentResource - resourceValue >= 0)
        {
            return true;
        }
        return false;
    }
    
}
