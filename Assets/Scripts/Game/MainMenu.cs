using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
namespace Game
{


    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button submitCodeButton;
        [SerializeField] private TMP_InputField codeInputField;
        [SerializeField] private TextMeshProUGUI codeText;

        // Start is called before the first frame update
        public void StartSingleplay()
        {
            SceneManager.LoadScene("SingleGame");
        }

        public void StartMultiplay()
        {
            hostButton.gameObject.SetActive(true);
            joinButton.gameObject.SetActive(true);
            // SceneManager.LoadScene("Lobby");

        }

        public async void OnHostClicked()
        {
            bool succeeded = await GameLobbyManager.Instance.CreateLobby();
            if (succeeded)
            {
                SceneManager.LoadSceneAsync("Lobby");
            }
        }

        public void OnJoinClicked()
        {
            codeInputField.gameObject.SetActive(true);
            submitCodeButton.gameObject.SetActive(true);
        }

        public async void OnSubmitCodeClicked()
        {
            string code = codeText.text;
            code = code.Substring(0, code.Length - 1);

            bool succeeded = await GameLobbyManager.Instance.JoinLobby(code);
            if (succeeded)
            {
                SceneManager.LoadSceneAsync("Lobby");
            }
        }
    }
}