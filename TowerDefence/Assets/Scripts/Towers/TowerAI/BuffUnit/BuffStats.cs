using UnityEngine;
using UnityEngine.UI;

public class BuffStats : MonoBehaviour, IUnitStats, IStats
{
    [Header("Buff Stats")] 
    private float maxHealth = 50f;
    
    internal int buffAmount = 20;

    
    internal float currentHealth;
    private float scoreValue = 5;
    private float resourceValue = 10;

    [Header("Class")] 
    private UnitTracker unitTracker; 
    private BuffHandler buffHandler;
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
        Debug.Log(" Buff current hp " + currentHealth);
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
        Debug.Log(" Buff current hp " + currentHealth);
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
        Debug.Log("Buff unit has died.");
        scoreManager.RemoveScore(scoreValue);
        unitTracker.EnemyTargets.Remove(gameObject);
        resourceManager.AddResource(resourceValue);
        hasBeenPlaced = false;
    }
    
    public void ApplyBuff(int amount)
    {
        maxHealth = Mathf.Clamp(maxHealth + amount + 5, 0, 65);
        buffAmount = Mathf.Clamp(buffAmount + amount, 0, 20);

        Debug.Log("new max health " + maxHealth);
        Debug.Log("new buff amount " + buffAmount);
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
