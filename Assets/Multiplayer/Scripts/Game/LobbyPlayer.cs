using BeatsBang.Core.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class LobbyPlayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private TextMeshProUGUI _isReadyText;
        private LobbyPlayerData _data;


        public void SetData(LobbyPlayerData data)
        {
            _data = data;
            _playerName.text = _data.Gamertag;

            if (_data.IsReady)
            {
                if (_isReadyText.text == "")
                {
                    _isReadyText.text = "Ready";
                }
            }

            gameObject.SetActive(true);
        }
    }
}