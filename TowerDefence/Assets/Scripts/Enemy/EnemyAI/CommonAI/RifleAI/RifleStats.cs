using UnityEngine;
using UnityEngine.UI;

public class RifleStats : MonoBehaviour, IEnemyStats, IStats
{
    [Header("Rifle Stats")] 
    private readonly float maxHealth = 50f;
    
    internal float currentHealth;
    private readonly float scoreValue = 5;
    
    [Header("Class")] 
    private UnitTracker unitTracker; 
    private readonly RifleAttackHandler rifleAttackHandler;
    private ScoreManager scoreManager;
    
    private float rv = 4;
    private ResourceManager resourceManager;
    
    [Header("Health Bar")]
    public Image healthBar;
    
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        unitTracker = FindObjectOfType<UnitTracker>();
        currentHealth = maxHealth;
        resourceManager = FindObjectOfType<ResourceManager>();
    }
    
    public void ApplyDamage(float amount)
    {
        currentHealth -= amount; 
        Debug.Log("Rifle unit current HP: " + currentHealth);
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
        Debug.Log("Rifle unit healed, current HP: " + currentHealth);
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    public void Die()
    {
        Debug.Log("Rifle unit has died.");
        unitTracker.EnemyTargets.Remove(gameObject);
        resourceManager.AddResource(rv);
        scoreManager.AddScore(scoreValue);
    }
    
    public void ApplyBuff(int amount)
    {
       // currentHealth += amount;
       // rifleAttackHandler.damageAmount += amount;
    }
    
    public void OnSpawn()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth;
    }
    
    public bool CanSpawn()
    {
        return true;
    }
}