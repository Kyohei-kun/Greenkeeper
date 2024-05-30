using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CS_Net_CameraManager : NetworkBehaviour
{
    [SerializeField] private Transform crown;
    [SerializeField] private Transform virtualCamera;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
        {
            Destroy(crown.gameObject);
            Destroy(virtualCamera.gameObject);
        }
    }
}
