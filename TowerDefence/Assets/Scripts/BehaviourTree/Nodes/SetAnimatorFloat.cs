using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore; // Code attributes.
using Pada1.BBCore.Tasks; // TaskStatus.
using BBUnity.Actions; // Using GOAction - Gives us access to the gameobject.
[Action("MyActions/SetAnimatorFloat")]
[Help("Get the animator attached to the gameobject and set a float parameter.")]
public class SetAnimatorFloat : GOAction
{
    // Define the float parameter name "floatParameterName".
    [InParam("FloatParameterName")]
    public string floatParameterName;
    // Define the float parameter value "floatParameterValue".
    [InParam("FloatParameterValue")]
    public float floatParameterValue;
    // Define the animator state value we are moving to. Used to check if
    // we have reached the state. NONE = do not wait.
    [InParam("NextAnimatorStateName", DefaultValue = "NONE")]
    public string nextAnimatorStateName = "";
    // Hash to animator parameter.
    private int parameterHash;
    // Store the animator for the gameobject.
    private Animator anim;
    // Store the current AnimatorStateInfo.
    private AnimatorStateInfo currentStateInfo;
    // Initialization method.
    public override void OnStart()
    {
        if (anim == null)
        {
            anim = gameObject.GetComponent<Animator>();
            parameterHash = Animator.StringToHash(floatParameterName);
        }
        // Set the Parameter.
        anim.SetFloat(parameterHash, floatParameterValue);
    }
    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        // Check for expected state change.
        currentStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (currentStateInfo.IsName(nextAnimatorStateName))
        {
            // The action is completed. We must inform the execution engine.
            return TaskStatus.COMPLETED;
        }
        if(nextAnimatorStateName.Equals("NONE"))
        {
            // The action is completed. We must inform the execution engine.
            return TaskStatus.COMPLETED;
        }
        
        // The action is still running.
        return TaskStatus.RUNNING;
    } // OnUpdate
}