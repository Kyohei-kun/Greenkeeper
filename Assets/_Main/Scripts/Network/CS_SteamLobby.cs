using NaughtyAttributes;
using Netcode.Transports;
using Steamworks;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CS_SteamLobby : NetworkBehaviour
{
    [SerializeField] private GameObject hostLobbyButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject joinButton;
    [SerializeField] private GameObject connectPanel;

    [Space] [SerializeField] [Scene] private String sceneToLoad;

    private NetworkManager networkManager;
    private string hostAdressKey = "HostAddress";

    #region Steam Callback
    protected Callback<LobbyCreated_t> cb_LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> cb_GameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> cb_LobbyEntered;
    #endregion

    private void Start()
    {
        networkManager = NetworkManager.Singleton;
        if (!SteamManager.Initialized) { return; }

        cb_LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreate);
        cb_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        cb_LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        startButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (IsHost)
                NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single); // Put this on a sceneManager
        });

        hostLobbyButton.GetComponent<Button>().onClick.AddListener(HostLobby);
        joinButton.GetComponent<Button>().onClick.AddListener(Join);

    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 5);
    }
    public void Join()
    {
        SteamFriends.ActivateGameOverlayToUser("Bonjour", SteamUser.GetSteamID());
    }

    private void OnLobbyCreate(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) { hostLobbyButton.SetActive(true); return; }

        networkManager.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAdressKey, SteamUser.GetSteamID().ToString());

        connectPanel.SetActive(false);
        startButton.SetActive(true);
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (IsHost)
            return;

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAdressKey);

        networkManager.GetComponent<SteamNetworkingSocketsTransport>().ConnectToSteamID = Convert.ToUInt64(hostAddress);
        networkManager.StartClient();
        connectPanel.SetActive(false);
    }
}