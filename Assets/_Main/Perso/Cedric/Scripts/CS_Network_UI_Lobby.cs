using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CS_Network_UI_Lobby : NetworkBehaviour
{
    [SerializeField] private Button start_Btn;

    private void Start()
    {
        start_Btn.onClick.AddListener(() =>
        {
            if (IsHost)
                NetworkManager.Singleton.SceneManager.LoadScene("Cedric_Main", LoadSceneMode.Single);
        });

        NetworkManager.Singleton.OnClientStarted += Singleton_OnClientStarted;
        NetworkManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
    }

    private void Singleton_OnServerStarted()
    {
    }

    private void Singleton_OnClientStarted()
    {
    }

    private void Update()
    {
        if(IsHost)
        {
            start_Btn.gameObject.SetActive(true);
        }
    }
}