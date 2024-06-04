using NaughtyAttributes;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CS_Net_Init_Player : NetworkBehaviour
{
    [SerializeField] bool offline = false;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject virtualCamera;
    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject crown;

    private void Start()
    {
        thirdPersonController.Offline = offline;
        if (offline)
        {
            playerInput.enabled = true;
            thirdPersonController.enabled = true;
            characterController.enabled = true;
            GetComponent<CS_Shoot>().enabled = true;
            Destroy(GetComponent<NetworkObject>());
            Destroy(GetComponent<ClientNetworkTransform>());
        }
    }

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
