using UnityEngine;
using UnityEngine.UI;

public class MeleeStats : MonoBehaviour, IUnitStats, IStats
{
    [Header("Melee Stats")] 
    private float maxHealth = 50f;
    internal float currentHealth;
    private float scoreValue = 5;
    private float resourceValue = 10;
    
    [Header("Class")] 
    private UnitTracker unitTracker; 
    private MeleeAttackHandler meleeAttackHandler;
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
        Debug.Log(" Melee current hp " + currentHealth);
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
        Debug.Log(" Melee current hp " + currentHealth);
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
        Debug.Log("Melee unit has died.");
        scoreManager.RemoveScore(scoreValue);
        unitTracker.EnemyTargets.Remove(gameObject);
        hasBeenPlaced = false;
    }
    
    public void ApplyBuff(int amount)
    {
        currentHealth += amount;
        meleeAttackHandler.damageAmount += amount;
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

