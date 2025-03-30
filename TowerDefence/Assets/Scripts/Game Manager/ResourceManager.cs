using System;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Resources")] 
    private static float totalResource = 30;
    public  float currentResource;

    [Header("Cooldowns")] 
    private float cooldown = 60;
    private float currentCooldown;
    
    [Header("UI")] 
    public TextMeshProUGUI currentResourceAmount;

    private void Awake()
    {
        currentResource = totalResource;
        currentCooldown = cooldown;
    }

    private void Update()
    {
        AddTotalResource();
    }

    public void SubtractResource(float unitCost)
    {
        currentResource = Mathf.Clamp(currentResource, 0, totalResource);
        currentResource -= unitCost;
        currentResourceAmount.text = "Resources: " + currentResource + " / " + totalResource;
    }

    private void AddTotalResource()
    {
        if (currentCooldown <= 0)
        {
            totalResource += 5;

            if (cooldown > 30)
            {
                cooldown = Mathf.Clamp(cooldown, 30, 60);
                cooldown -= 10;
            }
            currentCooldown = cooldown;
        }
        currentCooldown -= Time.deltaTime;
    }
    
    //TODO implement Upgrades
}
