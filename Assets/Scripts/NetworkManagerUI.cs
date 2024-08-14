using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private TextMeshProUGUI playersCountText;

    private NetworkVariable<int> playersCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
        
        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Update()
    {
        playersCountText.text = "Players: " + playersCount.Value;
        
        if(!IsServer) return;
        playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
}
