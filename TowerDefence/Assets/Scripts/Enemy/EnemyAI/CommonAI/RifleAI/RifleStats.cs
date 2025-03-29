using UnityEngine;
using UnityEngine.UI;

public class RifleStats : MonoBehaviour, IEnemyStats
{
    [Header("Rifle Stats")] 
    private readonly float maxHealth = 50f;
    private float currentHealth;
    
    [Header("Class")] 
    private UnitTracker unitTracker; 
    private readonly RifleAttackHandler rifleAttackHandler;
    
    [Header("Health Bar")]
    public Image healthBar;
    
    void Start()
    {
        currentHealth = maxHealth;
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
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
        UnitTracker.EnemyTargets.Remove(gameObject);
    }
    
    public void ApplyBuff(int amount)
    {
       // currentHealth += amount;
       // rifleAttackHandler.damageAmount += amount;
    }
}