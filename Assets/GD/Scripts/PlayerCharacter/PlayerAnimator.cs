using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [HideInInspector]
    public bool isWalking;
    [HideInInspector]
    public bool isFalling;
    [HideInInspector]
    public bool solidified;
    [HideInInspector]
    public bool isGrabbing;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setWalking(bool walking)
    {
        animator.SetBool("walking", walking);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
