using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles interactions on the end game screen UI, including rematch, swapping players, and returning to the menu.
/// </summary>
public class EndGameUI : MonoBehaviour
{
    // UI button references (assigned via Inspector)
    public Button rematchButton;
    public Button swapButton;
    public Button menuButton;

    // Scene name constants
    private const string GameSceneName = "GameScene";
    private const string EndGameSceneName = "EndGameScene";
    private const string StartScreenSceneName = "StartScreen";

    /// <summary>
    /// Registers button click listeners at startup.
    /// </summary>
    private void Start()
    {
        if (rematchButton != null)
            rematchButton.onClick.AddListener(OnRematchClicked);

        if (swapButton != null)
            swapButton.onClick.AddListener(OnSwapClicked);

        if (menuButton != null)
            menuButton.onClick.AddListener(OnMenuClicked);
    }

    /// <summary>
    /// Reloads the game scene to start a rematch.
    /// </summary>
    private void OnRematchClicked()
    {
        SceneManager.UnloadSceneAsync(EndGameSceneName);
        SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Swaps the starting player and reloads the game scene.
    /// </summary>
    private void OnSwapClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SwapNextStarterOnce();

        SceneManager.UnloadSceneAsync(EndGameSceneName);
        SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Resets game flags and loads the main start screen.
    /// </summary>
    private void OnMenuClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ResetAllFlags();

        SceneManager.LoadScene(StartScreenSceneName, LoadSceneMode.Single);
    }
}