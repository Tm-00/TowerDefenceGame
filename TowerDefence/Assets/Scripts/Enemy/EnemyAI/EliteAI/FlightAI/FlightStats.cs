using UnityEngine;
using UnityEngine.UI;
public class FlightStats : MonoBehaviour, IEnemyStats, IStats
{
    [Header("Flight Stats")] 
    private float maxHealth = 50f;

    internal float currentHealth;
    private float scoreValue = 10;
    
    [Header("Class")]
    private UnitTracker unitTracker; 
    private readonly FlightAttackHandler flightAttackHandler;
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
        Debug.Log("Flight unit current HP: " + currentHealth);
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
        Debug.Log("Flight unit healed, current HP: " + currentHealth);
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    public void Die()
    {
        scoreManager.AddScore(scoreValue);
        Debug.Log("Flight unit has died.");
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
