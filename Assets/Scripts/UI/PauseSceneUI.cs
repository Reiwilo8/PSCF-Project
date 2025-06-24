using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles button interactions for the pause scene UI.
/// </summary>
public class PauseSceneUI : MonoBehaviour
{
    public Button backButton;
    public Button restartButton;
    public Button swapButton;
    public Button menuButton;

    /// <summary>
    /// Initializes button listeners at the start of the scene.
    /// </summary>
    private void Start()
    {
        if (backButton != null)
            backButton.onClick.AddListener(OnBackClicked);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);

        if (swapButton != null)
            swapButton.onClick.AddListener(OnSwapClicked);

        if (menuButton != null)
            menuButton.onClick.AddListener(OnMenuClicked);
    }

    /// <summary>
    /// Resumes the game by restarting round time tracking and unloading the pause scene.
    /// </summary>
    private void OnBackClicked()
    {
        if (StatsManager.Instance != null)
            StatsManager.Instance.StartRoundTime();

        SceneManager.UnloadSceneAsync("PauseScene");
    }

    /// <summary>
    /// Restarts the current game with the same starter and reloads the game scene.
    /// </summary>
    private void OnRestartClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartWithCurrentStarter();

        SceneManager.UnloadSceneAsync("PauseScene");
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Restarts the game with the starter swapped and reloads the game scene.
    /// </summary>
    private void OnSwapClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartWithSwappedStarter();

        SceneManager.UnloadSceneAsync("PauseScene");
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Returns to the main menu and resets game flags.
    /// </summary>
    private void OnMenuClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ResetAllFlags();

        SceneManager.LoadScene("StartScreen");
    }
}