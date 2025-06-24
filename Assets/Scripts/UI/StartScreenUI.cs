using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles interactions on the main start screen UI, including navigation and application exit.
/// </summary>
public class StartScreenUI : MonoBehaviour
{
    // UI button references (assigned via Inspector)
    public Button playButton;
    public Button statisticsButton;
    public Button settingsButton;
    public Button exitButton;

    // Scene name constants
    private const string GameModeSceneName = "GameModeScene";
    private const string StatisticsSceneName = "StatisticsScene";
    private const string SettingsSceneName = "SettingsScene";

    /// <summary>
    /// Registers button click listeners at startup.
    /// </summary>
    private void Start()
    {
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);

        if (statisticsButton != null)
            statisticsButton.onClick.AddListener(OnStatisticsClicked);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitClicked);
    }

    /// <summary>
    /// Loads the game mode selection scene.
    /// </summary>
    void OnPlayClicked()
    {
        SceneManager.LoadScene(GameModeSceneName);
    }

    /// <summary>
    /// Loads the statistics screen scene.
    /// </summary>
    void OnStatisticsClicked()
    {
        SceneManager.LoadScene(StatisticsSceneName);
    }

    /// <summary>
    /// Loads the settings screen scene.
    /// </summary>
    void OnSettingsClicked()
    {
        SceneManager.LoadScene(SettingsSceneName);
    }

    /// <summary>
    /// Saves statistics and exits the application.
    /// In the Unity Editor, stops play mode instead.
    /// </summary>
    void OnExitClicked()
    {
        if (StatsManager.Instance != null)
            StatsManager.Instance.SaveStats();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}