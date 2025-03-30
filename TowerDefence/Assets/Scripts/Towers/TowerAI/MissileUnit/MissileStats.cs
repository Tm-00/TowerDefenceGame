using UnityEngine;
using UnityEngine.UI;

public class MissileStats : MonoBehaviour, IUnitStats, IStats
{
    [Header("Missile Stats")] 
    private float maxHealth = 50f;
    private float currentHealth;
    private float scoreValue = 5;
    private float resourceValue = 10;
    
    [Header("Class")] 
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
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
        UnitTracker.EnemyTargets.Remove(gameObject);
        hasBeenPlaced = false;
    }
    
    public void ApplyBuff(int amount)
    {
        currentHealth += amount;
        missileAttackHandler.damageAmount += amount;
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

