using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CS_Bullet : NetworkBehaviour
{
    float timer = 5;

    private void Update()
    {
        float speed = 10;
        transform.position += Time.deltaTime * transform.forward * speed;

        timer -= Time.deltaTime;
        if (timer <= 0)
            DestroyServerRPC();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CS_Target>() != null)
        {
            other.GetComponent<CS_Target>().DestroyServerRPC();
        }
        if (other.tag != "Player" && other.tag != "Ground")
        {
            DestroyServerRPC();
        }
    }

    [ServerRpc]
    public void DestroyServerRPC()
    {
        GetComponent<NetworkObject>().Despawn(true);
    }
}
