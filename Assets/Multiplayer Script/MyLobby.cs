// using System.Collections;
// using System.Collections.Generic;
// using Unity.Services.Authentication;
// using Unity.Services.Core;
// using Unity.Services.Lobbies;
// using Unity.Services.Lobbies.Models;
// using UnityEngine;

// public class MyLobby : MonoBehaviour
// {
//     private MyLobby hostLobby;
//     private float heartbeatTimer;
//     // Start is called before the first frame update
//     private async void Start()
//     {
//         await UnityServices.InitializeAsync();

//         AuthenticationService.Instance.SignedIn += () =>
//         {
//             Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
//         };
//         await AuthenticationService.Instance.SignInAnonymouslyAsync();
//     }

//     private void Update()
//     {
//         HandleLobbyHeartbeat();
//     }

//     private async void HandleLobbyHeartbeat()
//     {
//         if (hostLobby != null)
//         {
//             heartbeatTimer -= Time.deltaTime;
//             if (heartbeatTimer < 0f)
//             {
//                 float heartbeatTimerMax = 15;
//                 heartbeatTimer = heartbeatTimerMax;
//                 await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
//             }
//         }
//     }

//     private async void CreateLobby()
//     {
//         try
//         {
//             string lobbyName = "MyLobby";
//             int maxPlayers = 4;
//             Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
//             hostLobby = lobby;
//             Debug.Log("Created lobby! " + lobby.Name + " " + lobby.MaxPlayers);
//         }
//         catch (LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }

//     private async void ListLobbies()
//     {
//         try
//         {

//             QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
//             Debug.Log("Lobbies found: " + queryResponse.Results.Count);

//             foreach (Lobby lobby in queryResponse.Results)
//             {
//                 Debug.Log(lobby.Name);
//             }
//         }
//         catch (LobbyServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }


// }
