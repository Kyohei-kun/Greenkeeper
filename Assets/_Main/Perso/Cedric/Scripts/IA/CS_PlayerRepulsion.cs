using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CS_PlayerRepulsion : MonoBehaviour
{
    private CS_PouicIA_Manager _manager;

    public void OnRepulsion(InputValue value)
    {
        if (_manager == null)
        {
            _manager = GameObject.FindGameObjectWithTag("PouicIA_Manager").GetComponent<CS_PouicIA_Manager>();
            if (_manager == null) { return; }
        }
        if (value.isPressed)
        {
            _manager.AddPlayerForce(transform, 10, 1);
        }
    }
}
