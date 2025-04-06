using UnityEngine;
using UnityEngine.UI;
public class BossStats : MonoBehaviour, IEnemyStats, IStats
{
    [Header("Boss Stats")] 
    private float maxHealth = 150f;
    internal float currentHealth;
    private float scoreValue = 25;
    
    [Header("Class")]
    private UnitTracker unitTracker; 
    private readonly BossAttackHandler bossAttackHandler;
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
        Debug.Log("Boss unit current HP: " + currentHealth);
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
        Debug.Log("Boss unit healed, current HP: " + currentHealth);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void ApplyBuff(int amount)
    {
        // currentHealth += amount;
        // rifleAttackHandler.damageAmount += amount;;
    }

    public void Die()
    {
        scoreManager.AddScore(scoreValue);
        Debug.Log("Flight unit has died.");
        unitTracker.EnemyTargets.Remove(gameObject);
    }

    public bool CanSpawn()
    {
        return true;
    }

    public void OnSpawn()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth;
    }
}
