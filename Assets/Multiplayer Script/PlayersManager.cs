using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BeatsBang.Core.Singletons;

public class PlayersManager : Singleton<PlayersManager>
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    public int PlayersInGame
    {
        get { return playersInGame.Value; }
    }

    private void Start()
    {

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                Logger.Instance.LogInfo($"{id} just connected");
                playersInGame.Value++;
            }


        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Logger.Instance.LogInfo($"{id} just disconnected");
                playersInGame.Value--;
            }
        };


    }
}
