using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text time;
    public TMP_Text towerHealth;
    public TMP_Text npcCount;
    public TMP_Text enemiesCount;

    public Slider enemyHealthBar;
    public GameObject resultsPanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public Button retryBtn;
    public Button ResumeBtn;
    public Button quitBtn;
    public Button nextLevel;
    public static UIManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            retryBtn.gameObject.SetActive(true);
            pausePanel.SetActive(true);
        }
    }

    private void OnEnable()
    {
        gameOverPanel.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        resultsPanel.SetActive(false);
        pausePanel.SetActive(false);
        ResumeBtn.onClick.AddListener(ResumeClicked);
        retryBtn.onClick.AddListener(RetryClicked);
        quitBtn.onClick.AddListener(QuitClicked);
        nextLevel.onClick.AddListener(NextLevelClicked);
    }

    private void OnDisable()
    {

        ResumeBtn.onClick.RemoveListener(ResumeClicked);
        retryBtn.onClick.RemoveListener(RetryClicked);
        quitBtn.onClick.RemoveListener(QuitClicked);
        nextLevel.onClick.RemoveListener(NextLevelClicked);
    }

    private void ResumeClicked()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    private void RetryClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void QuitClicked()
    {
        Application.Quit();
    }

    private void NextLevelClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void RestartGamePanel()
    {
        gameOverPanel.SetActive(true);
        retryBtn.gameObject.SetActive(true);
        quitBtn.gameObject.SetActive(true);
    }
    
    public void ResultScreenPanel()
    {
        Time.timeScale = 0;
        resultsPanel.SetActive(true);
        quitBtn.gameObject.SetActive(true);
    }

}
