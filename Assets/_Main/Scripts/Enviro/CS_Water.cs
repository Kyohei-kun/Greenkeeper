using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Water : MonoBehaviour
{
    [HideInInspector]
    public Vector3 newDest;

    // Start is called before the first frame update
    void Start()
    {
        newDest =  GetComponentInChildren<CS_WaterDestination>().transform.position;
    }
}
