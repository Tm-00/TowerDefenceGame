using UnityEngine;
using UnityEngine.UI;

public class ScoutStats : MonoBehaviour, IEnemyStats, IStats
{
    [Header("Scout Stats")] 
    private float maxHealth = 50f;

    internal float currentHealth;
    private float scoreValue = 1;
    
    [Header("Class")]
    private UnitTracker unitTracker; 
    private ScoreManager scoreManager;
    
    private float rv = 4;
    private ResourceManager resourceManager;
    
    [Header("Health Bar")]
    public Image healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        currentHealth = maxHealth;
        resourceManager = FindObjectOfType<ResourceManager>();
        unitTracker = FindObjectOfType<UnitTracker>();
    }

    public void ApplyDamage(float amount)
    {
        currentHealth -= amount; 
        Debug.Log("Scout unit current HP: " + currentHealth);
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
        Debug.Log("Scout unit healed, current HP: " + currentHealth);
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    public void Die()
    {
        Debug.Log("Scout unit has died.");
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
