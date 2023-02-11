using BeatsBang.Core.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class LobbyPlayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerName;
        private LobbyPlayerData _data;
        public void SetData(LobbyPlayerData data)
        {
            _data = data;
            _playerName.text = _data.Gamertag;
            gameObject.SetActive(true);
        }
    }
}