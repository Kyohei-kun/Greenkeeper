using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CS_AutoKill : NetworkBehaviour
{
    [SerializeField] float _timer;

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            if (IsHost && GetComponent<NetworkObject>() != null)
                GetComponent<NetworkObject>().Despawn(true);
            else
                Destroy(gameObject);
        }
    }
}
