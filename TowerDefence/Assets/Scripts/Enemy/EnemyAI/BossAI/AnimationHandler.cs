using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [Header("Animator")] 
    [SerializeField] private Animator anim;
    AnimatorStateInfo currentStateInfo;
    private int shootTriggerHash = Animator.StringToHash("Shoot");
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
