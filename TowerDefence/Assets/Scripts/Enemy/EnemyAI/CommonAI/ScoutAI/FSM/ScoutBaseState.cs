using UnityEngine;

public abstract class ScoutBaseState
{
    
    // base class constructor
    public ScoutBaseState()
    {
    
    }
    
    // Enter the state
    public abstract void Enter(GameObject go);
    
    // Update the state
    public abstract void Update(GameObject go);
    
    // Exit the state
    public abstract void Exit(GameObject go);
    
    // Process any input 
    public abstract ScoutBaseState HandleInput(GameObject go);
    
    public virtual void OnCollisionEnter(Collision col)
    {
        
    }
    
}