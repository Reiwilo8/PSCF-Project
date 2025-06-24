using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the UI for selecting game modes and navigating back to the main menu.
/// </summary>
public class GameModeSceneUI : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button backButton;
    public Button pvpButton;
    public Button pveButton;

    // Scene name constants to avoid hardcoding strings multiple times
    private const string StartScreen = "StartScreen";
    private const string GameScene = "GameScene";
    private const string DifficultySelectScene = "DifficultySelectScene";

    /// <summary>
    /// Assigns button listeners at runtime.
    /// </summary>
    private void Start()
    {
        if (backButton != null)
            backButton.onClick.AddListener(OnBackClicked);

        if (pvpButton != null)
            pvpButton.onClick.AddListener(OnPvPClicked);

        if (pveButton != null)
            pveButton.onClick.AddListener(OnPvEClicked);
    }

    /// <summary>
    /// Returns to the start screen.
    /// </summary>
    private void OnBackClicked()
    {
        SceneManager.LoadScene(StartScreen);
    }

    /// <summary>
    /// Sets the game mode to PvP and loads the game scene.
    /// </summary>
    private void OnPvPClicked()
    {
        GameManager.Instance.SetGameMode(GameMode.PvP);
        SceneManager.LoadScene(GameScene);
    }

    /// <summary>
    /// Sets the game mode to PvE and proceeds to difficulty selection.
    /// </summary>
    private void OnPvEClicked()
    {
        GameManager.Instance.SetGameMode(GameMode.PvE);
        SceneManager.LoadScene(DifficultySelectScene);
    }
}
