using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CS_ClientRemover : NetworkBehaviour
{
    [SerializeField] private Transform crown;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(crown.gameObject);
        }
    }
}
