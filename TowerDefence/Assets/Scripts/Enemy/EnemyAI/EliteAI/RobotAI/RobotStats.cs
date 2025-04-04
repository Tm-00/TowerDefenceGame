using UnityEngine;
using UnityEngine.UI;

public class RobotStats : MonoBehaviour, IEnemyStats, IStats
{
    [Header("Robot Stats")] 
    private float maxHealth = 50f;
    private float currentHealth;
    private float scoreValue = 10;

    
    [Header("Class")]
    private UnitTracker unitTracker; 
    private readonly RobotAttackHandler robotAttackHandler;
    private ScoreManager scoreManager;
    
    [Header("Health Bar")]
    public Image healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        currentHealth = maxHealth;
    }
    
    public void ApplyDamage(float amount)
    {
        currentHealth -= amount; 
        Debug.Log("Robot unit current HP: " + currentHealth);
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
        Debug.Log("Robot unit healed, current HP: " + currentHealth);
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    public void Die()
    {
        scoreManager.AddScore(scoreValue);
        Debug.Log("Robot unit has died.");
        unitTracker.EnemyTargets.Remove(gameObject);
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
