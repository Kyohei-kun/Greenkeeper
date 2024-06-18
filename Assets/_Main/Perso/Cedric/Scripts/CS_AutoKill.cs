using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AutoKill : MonoBehaviour
{
    [SerializeField] float _timer;

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
            Destroy(gameObject);
    }
}
