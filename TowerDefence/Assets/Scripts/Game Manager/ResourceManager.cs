using System;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Resources")] 
    private static float totalResource = 30;
    public  float currentResource;

    [Header("Cooldowns")] 
    private float totalResourceCooldownTimer = 60;
    private float currentResourceCooldownTimer = 15;
    private float totalResourceCooldown;
    private float currentResourceCooldown;
    
    [Header("UI")] 
    public TextMeshProUGUI currentResourceAmount;

    private void Awake()
    {
        currentResource = totalResource;
        totalResourceCooldown = totalResourceCooldownTimer;
        currentResourceCooldown = totalResourceCooldownTimer;
    }

    private void Update()
    {
        AddTotalResource();
        AddCurrentResource();
    }

    public void SubtractResource(float unitCost)
    {
        currentResource = Mathf.Clamp(currentResource, 0, totalResource);
        currentResource -= unitCost;
        currentResourceAmount.text = "Resources: " + currentResource + " / " + totalResource;
    }
    
    private void AddCurrentResource()
    {
        if (currentResourceCooldown <= 0)
        {
            currentResource = Mathf.Clamp(currentResource, 0, totalResource);
            currentResource += 1;
            if (currentResourceCooldownTimer > 5)
            {
                currentResourceCooldownTimer = Mathf.Clamp(currentResourceCooldownTimer, 5, 15);
                totalResourceCooldownTimer -= 1;
            }
            currentResourceCooldown = currentResourceCooldownTimer;
        }
        currentResourceAmount.text = "Resources: " + currentResource + " / " + totalResource;
        currentResourceCooldown -= Time.deltaTime;
    }
    
    private void AddTotalResource()
    {
        if (totalResourceCooldown <= 0)
        {
            totalResource += 5;

            if (totalResourceCooldownTimer > 30)
            {
                totalResourceCooldownTimer = Mathf.Clamp(totalResourceCooldownTimer, 30, 60);
                totalResourceCooldownTimer -= 10;
            }
            totalResourceCooldown = totalResourceCooldownTimer;
        }
        currentResourceAmount.text = "Resources: " + currentResource + " / " + totalResource;
        totalResourceCooldown -= Time.deltaTime;
    }
    
    //TODO implement Upgrades
}
