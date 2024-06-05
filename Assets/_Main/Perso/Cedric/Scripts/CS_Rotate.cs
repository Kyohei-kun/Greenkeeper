using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotate;

    private void Update()
    {
        transform.Rotate(rotate);
    }
}
