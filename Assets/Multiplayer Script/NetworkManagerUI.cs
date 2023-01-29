using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

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
