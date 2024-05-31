using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CS_Target : NetworkBehaviour
{
    [ServerRpc]
    public void DestroyServerRPC()
    {
        GetComponent<NetworkObject>().Despawn(true);
    }
}
