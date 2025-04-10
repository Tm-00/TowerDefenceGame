using UnityEngine;
using UnityEngine.UI;

public class MissileStats : MonoBehaviour, IUnitStats, IStats
{
    [Header("Missile Stats")] 
    private float maxHealth = 150f;
    internal float currentHealth;
    
    internal int damageAmount = 35;
    
    private float scoreValue = 5;
    private float resourceValue = 14;
    
    [Header("Class References")] 
    private UnitTracker unitTracker; 
    private MissileAttackHandler missileAttackHandler;
    private ScoreManager scoreManager;
    private ResourceManager resourceManager;
    
    [Header("Health Bar")]
    public Image healthBar;
    
    public bool hasBeenPlaced { get; set; }
    
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        resourceManager = FindObjectOfType<ResourceManager>();
        unitTracker = FindObjectOfType<UnitTracker>();
        currentHealth = maxHealth;
        hasBeenPlaced = false;
    }

    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(" Missile current hp " + currentHealth);
        healthBar.fillAmount = currentHealth / maxHealth;
        
        if (currentHealth <= 0)
        {
            Die(); 
        }
    }
    
    public void ApplyHeal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthBar.fillAmount = currentHealth / maxHealth; 
        Debug.Log(" Missile current hp " + currentHealth);
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
        Debug.Log("Missile unit has died.");
        scoreManager.RemoveScore(scoreValue);
        unitTracker.EnemyTargets.Remove(gameObject);
        resourceManager.AddResource(resourceValue);
        hasBeenPlaced = false;
    }
    
    public void ApplyBuff(int amount)
    {
        maxHealth = Mathf.Clamp(maxHealth + amount + 5, 0, 175);
        damageAmount = Mathf.Clamp(damageAmount + amount, 0, 55);

        Debug.Log("new max health " + maxHealth);
        Debug.Log("new buff amount " + damageAmount);
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

