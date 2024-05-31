using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CS_Shoot : NetworkBehaviour
{
    [SerializeField] private Transform bullet_pref;


    private void Update()
    {
        if(IsOwner)
        {
            if(Input.GetMouseButtonDown(0))
            {
                SpawnBulletServerRPC();
            }
        }
    }

    [ServerRpc]
    public void SpawnBulletServerRPC()
    {
        Transform bullet = Instantiate(bullet_pref);
        bullet.position = this.transform.position;
        bullet.forward = this.transform.forward;
        bullet.GetComponent<NetworkObject>().Spawn();
    }

}
