using UnityEngine;

public abstract class RifleBaseState
{
    
    // base class constructor
    public RifleBaseState()
    {
    
    }
    
    // Enter the state
    public abstract void Enter(GameObject go);
    
    // Update the state
    public abstract void Update(GameObject go);
    
    // Exit the state
    public abstract void Exit(GameObject go);
    
    // Process any input 
    public abstract RifleBaseState HandleInput(GameObject go);
    
    public virtual void OnCollisionEnter(Collision col)
    {
        
    }
    
}