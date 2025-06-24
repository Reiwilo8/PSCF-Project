using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the difficulty selection scene, including navigation and game setup.
/// </summary>
public class DifficultySelectUI : MonoBehaviour
{
    // Button references assigned via Inspector
    public Button backButton;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button customButton;

    /// <summary>
    /// Initializes button listeners on startup.
    /// </summary>
    private void Start()
    {
        if (backButton != null)
            backButton.onClick.AddListener(OnBackClicked);

        if (easyButton != null)
            easyButton.onClick.AddListener(() => OnDifficultySelected(Difficulty.Easy));

        if (mediumButton != null)
            mediumButton.onClick.AddListener(() => OnDifficultySelected(Difficulty.Medium));

        if (hardButton != null)
            hardButton.onClick.AddListener(() => OnDifficultySelected(Difficulty.Hard));

        if (customButton != null)
            customButton.onClick.AddListener(() => OnDifficultySelected(Difficulty.Custom));
    }

    /// <summary>
    /// Returns to the game mode selection scene.
    /// </summary>
    private void OnBackClicked()
    {
        SceneManager.LoadScene("GameModeScene");
    }

    /// <summary>
    /// Sets game mode and difficulty, then loads the appropriate scene.
    /// </summary>
    /// <param name="difficulty">Selected difficulty level.</param>
    private void OnDifficultySelected(Difficulty difficulty)
    {
        GameManager.Instance.SetGameMode(GameMode.PvE);
        GameManager.Instance.SetDifficulty(difficulty);

        string sceneName = (difficulty == Difficulty.Custom)
            ? "CustomDifficultySettingsScene"
            : "GameScene";

        SceneManager.LoadScene(sceneName);
    }
}