using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button playBtn;
    public Button quitBtn;

    private void OnEnable()
    {
        playBtn.onClick.AddListener(PlayGame);
        quitBtn.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        playBtn.onClick.RemoveListener(PlayGame);
        quitBtn.onClick.RemoveListener(QuitGame);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
