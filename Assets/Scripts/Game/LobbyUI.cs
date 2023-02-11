using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game
{


    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _lobbyCodeText;
        // Start is called before the first frame update
        void Start()
        {
            _lobbyCodeText.text = $"Lobby Code: {GameLobbyManager.Instance.GetLobbyCode()}";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
