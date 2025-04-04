using UnityEngine;
using UnityEngine.UI;

public class TurretStats : MonoBehaviour, IUnitStats, IStats
{
    [Header("Turret Stats")] 
    private readonly float maxHealth = 50f;
    internal float currentHealth;
    private readonly float scoreValue = 5;
    private readonly float resourceValue = 10;
    
    [Header("Class References")] 
    private UnitTracker unitTracker; 
    private TurretAttackHandler turretAttackHandler;
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
        Debug.Log(" Turret current hp " + currentHealth);
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
        Debug.Log(" Turret current hp " + currentHealth);
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
        Debug.Log("Turret unit has died.");
        scoreManager.RemoveScore(scoreValue);
        unitTracker.EnemyTargets.Remove(gameObject);
        hasBeenPlaced = false;
    }
    
    public void ApplyBuff(int amount)
    {
        currentHealth += amount;
        turretAttackHandler.damageAmount += amount;
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
