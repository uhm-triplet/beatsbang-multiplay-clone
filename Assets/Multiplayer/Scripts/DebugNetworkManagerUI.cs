using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugNetworkManagerUI : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    // [SerializeField] private TextMeshProUGUI playersInGameText;

    // private NetworkVariable<int> playerNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private void Awake()
    {

        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();

        });
        clientButton.onClick.AddListener(() =>
        {

            NetworkManager.Singleton.StartClient();
        });

    }



    // private void OnClientConnectedCallback(ulong clientId)
    // {
    //     if (!IsServer) return;
    //     GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    // }

}
