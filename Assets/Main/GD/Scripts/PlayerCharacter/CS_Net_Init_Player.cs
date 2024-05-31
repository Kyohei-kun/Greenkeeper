using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CS_Net_Init_Player : NetworkBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject virtualCamera;
    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject crown;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            playerInput.enabled = true;
            thirdPersonController.enabled = true;
            characterController.enabled = true;
            GetComponent<CS_Shoot>().enabled = true;
        }
        else
        {
            Destroy(virtualCamera);
            Destroy(crown);
        }
    }
}
