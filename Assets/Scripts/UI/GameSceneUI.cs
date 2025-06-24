using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles in-game UI actions, such as pausing the game or opening the end screen.
/// </summary>
public class GameSceneUI : MonoBehaviour
{
    // UI button reference (assigned via Inspector)
    public Button pauseButton;

    // Scene name constants
    private const string PauseSceneName = "PauseScene";
    private const string EndGameSceneName = "EndGameScene";

    /// <summary>
    /// Registers button click listener at startup.
    /// </summary>
    private void Start()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseClicked);
    }

    /// <summary>
    /// Loads the pause scene and stops round time.
    /// </summary>
    public void OnPauseClicked()
    {
        if (StatsManager.Instance != null)
            StatsManager.Instance.StopRoundTime();

        SceneManager.LoadScene(PauseSceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Loads the end game scene and stops round time.
    /// </summary>
    public void OpenEndGameScreen()
    {
        if (StatsManager.Instance != null)
            StatsManager.Instance.StopRoundTime();

        SceneManager.LoadScene(EndGameSceneName, LoadSceneMode.Additive);
    }
}