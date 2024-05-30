using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class CS_NetworkManager_UI : MonoBehaviour
{
    [SerializeField] private Button host_Btn;
    [SerializeField] private Button join_Btn;
    [SerializeField] private TextMeshProUGUI statut_Text;
    [SerializeField] private TMP_InputField inputField_IP;
    [SerializeField] private GameObject panel;

    private void Awake()
    {
        host_Btn.onClick.AddListener(() => { NetworkManager.Singleton.StartHost(); statut_Text.text = "Role: Client/Server"; statut_Text.color = new Color(1, 0.5f, 0); });
        join_Btn.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = CleanSpaceString(inputField_IP.text);  
            NetworkManager.Singleton.StartClient(); statut_Text.text = "Role: Client";   
            statut_Text.color = Color.cyan; 
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
}
