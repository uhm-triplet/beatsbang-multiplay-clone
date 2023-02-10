using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
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
}
