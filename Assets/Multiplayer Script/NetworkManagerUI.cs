using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerUI : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TextMeshProUGUI playersInGameText;

    private NetworkVariable<int> playerNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private void Awake()
    {

        hostButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartHost())
            {
                Logger.Instance.LogInfo("Host started...");
            }
            else
            {
                Logger.Instance.LogInfo("Host could not be started...");
            }
        });
        clientButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                Logger.Instance.LogInfo("Client started...");
            }
            else
            {
                Logger.Instance.LogInfo("Client could not be started...");
            }
        });

    }

    private void Update()
    {
        playersInGameText.text = "Players" + playerNum.Value.ToString();
        if (!IsServer) return;
        playerNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }



    // private void OnClientConnectedCallback(ulong clientId)
    // {
    //     if (!IsServer) return;
    //     GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    // }

}
