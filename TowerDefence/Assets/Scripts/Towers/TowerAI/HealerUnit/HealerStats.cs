using UnityEngine;
using UnityEngine.UI;

public class HealerStats : MonoBehaviour, IUnitStats, IStats
{
    [Header("Healer Stats")] 
    private float maxHealth = 75f;
    internal float currentHealth;
    
    internal int healAmount = 50;
    
    private float scoreValue = 5;
    private float resourceValue = 20;


    [Header("Class References")] 
    private UnitTracker unitTracker; 
    private HealerHealHandler healerHealHandler;
    private ScoreManager scoreManager;
    private ResourceManager resourceManager;
    
    [Header("Health Bar")]
    public Image healthBar;
    
    public bool hasBeenPlaced { get; set; }
    
    void Start()
    {
        unitTracker = FindObjectOfType<UnitTracker>();
        scoreManager = FindObjectOfType<ScoreManager>();
        resourceManager = FindObjectOfType<ResourceManager>();
        currentHealth = maxHealth;
        hasBeenPlaced = false;
    }
    
    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(" Heal current hp " + currentHealth);
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
        Debug.Log(" Heal current hp " + currentHealth);
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
        Debug.Log("Heal unit has died.");
        scoreManager.RemoveScore(scoreValue);
        unitTracker.EnemyTargets.Remove(gameObject);
        resourceManager.AddResource(resourceValue);
        hasBeenPlaced = false;
    }
    
    public void ApplyBuff(int amount)
    {
        maxHealth = Mathf.Clamp(maxHealth + amount + 5, 0, 65);
        healAmount = Mathf.Clamp(healAmount + amount, 0, 20);

        Debug.Log("new max health " + maxHealth);
        Debug.Log("new buff amount " + healAmount);
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
