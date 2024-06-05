using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DontDestroy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
