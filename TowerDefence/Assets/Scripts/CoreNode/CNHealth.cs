using UnityEngine;


public class CNHealth : MonoBehaviour
{
    [SerializeField] private GameObject coreNode;
    private static readonly int maxHealth = 10;
    private static int currentHealth = 10;
    private static readonly int amount = 1;
    public static bool hit = false;
    public static bool isDead = false;
    
    private static int HealthHandler(bool takenDamage)
    {
        if (takenDamage)
        {
            // Reset the flag after handling the event
            hit = false;
            currentHealth -= amount;
            Debug.Log("Core Node Health: "+ currentHealth + "/" +  maxHealth);
        }
        return currentHealth;
    }

    public static bool CoreNodeDead()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
        }
        return isDead;
    }
    
    public void OnCollisionEnter(Collision col)
    {
        //TODO might have to reconsider how enemy tags work
        if (col.gameObject.tag.Contains("Enemy"))
        {
            GameObject.Destroy(col.gameObject);
            hit = true;
        }
    }
}
