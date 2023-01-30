using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private GameObject networkUI;
    public override void OnNetworkSpawn()
    {
        // if (!IsOwner) return;
        // networkUI = GameObject.Find("NetworkManagerCanvas");
        // networkUI.SetActive(false);
        if (IsServer)
        {
            Debug.Log("I am Server");
        }
        if (IsHost)
        {
            Debug.Log("I am Host");
        }
        if (IsClient)
        {
            Debug.Log("I am Client");
        }

    }
}
