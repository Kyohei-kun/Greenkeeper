using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_PouicAnimation : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        anim = GetComponent<Animator>();
        float rdm = UnityEngine.Random.Range(0f, 1f);
        anim.Play("walk 0", 0, rdm);
    }
}
