using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseSceneUI : MonoBehaviour
{
    public Button backButton;
    public Button restartButton;
    public Button swapButton;
    public Button menuButton;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        swapButton.onClick.AddListener(OnSwapClicked);
        menuButton.onClick.AddListener(OnMenuClicked);
    }

    private void OnBackClicked()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("PauseScene");
    }

    private void OnRestartClicked()
    {
        Time.timeScale = 1f;
        GameManager.Instance.RestartWithCurrentStarter();
        SceneManager.UnloadSceneAsync("PauseScene");
        SceneManager.LoadScene("GameScene");
    }

    private void OnSwapClicked()
    {
        Time.timeScale = 1f;
        GameManager.Instance.RestartWithSwappedStarter();
        SceneManager.UnloadSceneAsync("PauseScene");
        SceneManager.LoadScene("GameScene");
    }

    private void OnMenuClicked()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ResetAllFlags();
        SceneManager.LoadScene("StartScreen");
    }
}
