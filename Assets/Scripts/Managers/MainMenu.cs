using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnPlayButton()
    {
        GameManager.Instance.LoadScene("LobbyScene");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
