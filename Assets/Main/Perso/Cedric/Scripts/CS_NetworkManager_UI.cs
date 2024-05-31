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

public class CS_NetworkManager_UI : NetworkBehaviour
{
    [SerializeField] private Button host_Btn;
    [SerializeField] private Button join_Btn;
    [SerializeField] private Button start_Btn;
    [SerializeField] private TMP_InputField inputField_IP;
    [SerializeField] private GameObject panel;

    private void Start()
    {
        start_Btn.onClick.AddListener(() =>
        {
            if (IsHost)
                NetworkManager.Singleton.SceneManager.LoadScene("Cedric_Main", LoadSceneMode.Single);
        });
        host_Btn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();

        });
        join_Btn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = CleanSpaceString(inputField_IP.text);
            NetworkManager.Singleton.StartClient();
        });

        NetworkManager.Singleton.OnClientStarted += Singleton_OnClientStarted;
        NetworkManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
    }

    private void Singleton_OnServerStarted()
    {
        panel.SetActive(false);
    }

    private void Singleton_OnClientStarted()
    {
        panel.SetActive(false);
    }

    private string CleanSpaceString(string ip)
    {
        return ip.Replace(" ", string.Empty);
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}