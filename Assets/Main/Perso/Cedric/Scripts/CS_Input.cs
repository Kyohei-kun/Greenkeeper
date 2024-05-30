using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CS_Input : NetworkBehaviour
{
    private Vector2 dir;

    public override void OnNetworkSpawn()
    {

    }

    private void Update()
    {
        Vector3 moveDir = Vector3.zero;
        float moveSpeed = 3f;

        //if (Input.GetKey(KeyCode.W)) moveDir.z = -1f;
        //if (Input.GetKey(KeyCode.S)) moveDir.z = 1f;
        //if (Input.GetKey(KeyCode.A)) moveDir.x = 1f;
        //if (Input.GetKey(KeyCode.D)) moveDir.x = -1f;

        moveDir = new Vector3(-dir.x, 0, -dir.y);
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void OnMove(InputValue value)
    {
        Debug.Log((value.Get<Vector2>()));
        dir = value.Get<Vector2>();
    }

    }
