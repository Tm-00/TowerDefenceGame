using UnityEngine;

public class FSM_CON : MonoBehaviour
{
    // The Current State
    private BaseState state;
    
    //[SerializeField] public static GameObject coreNode;
    
    // Start is called before the first frame update
    void Start()
    {
        // Create a new instance of the inital state
        BaseState _IdleState  = new IdleState(this.gameObject);
        
        // set the default / start state
        state = _IdleState ;
        
    }

    // Update is called once per frame
    void Update()
    {
        // update the state
        state.Update(this.gameObject);
        
        // handle input. including game events etc.
        BaseState newState = state.HandleInput(this.gameObject);

        if (newState != null)
        {
            // run the exit code
            state.Exit(this.gameObject);
            
            // set the new state
            state = newState;
            
            // run enter code for new state
            state.Enter(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        //Debug.Log("OnCollisionEnter - " + col.gameObject.name);
        
        // tell the current state a collision enter event has happened
        state.OnCollisionEnter(col);
    }
}