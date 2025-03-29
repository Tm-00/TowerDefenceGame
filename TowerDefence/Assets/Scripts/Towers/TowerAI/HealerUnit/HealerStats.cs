using UnityEngine;
using UnityEngine.UI;

public class HealerStats : MonoBehaviour, IUnitStats
{
    [Header("Healer Stats")] 
    private float maxHealth = 50f;
    private float currentHealth;

    [Header("Class")] 
    private UnitTracker unitTracker; 
    private HealerHealHandler healerHealHandler;
    
    [Header("Health Bar")]
    public Image healthBar;
    
    void Start()
    {
        currentHealth = maxHealth;
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
    
    public void Die()
    {
        Debug.Log("Heal unit has died.");
        UnitTracker.EnemyTargets.Remove(gameObject);
    }
    
    public void ApplyBuff(int amount)
    {
        currentHealth += amount;
        healerHealHandler.healAmount += amount;
    }
}
