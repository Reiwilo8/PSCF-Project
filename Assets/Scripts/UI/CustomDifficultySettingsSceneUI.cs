using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the UI logic for the custom difficulty settings scene,
/// allowing players to adjust Q-learning parameters before starting the game.
/// </summary>
public class CustomDifficultySettingsSceneUI : MonoBehaviour
{
    // UI references for buttons (assigned in the Inspector)
    public Button backButton;
    public Button playButton;

    // Sliders for adjusting Q-learning parameters
    public Slider alphaSlider;   // Learning rate
    public Slider gammaSlider;   // Discount factor (intelligence)
    public Slider epsilonSlider; // Randomness (exploration)

    // Scene name constants
    private const string DifficultySelectSceneName = "DifficultySelectScene";
    private const string GameSceneName = "GameScene";

    /// <summary>
    /// Initializes button click listeners on scene start.
    /// </summary>
    private void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
        playButton.onClick.AddListener(OnPlayClicked);
    }

    /// <summary>
    /// Returns the player to the difficulty selection screen.
    /// </summary>
    private void OnBackClicked()
    {
        SceneManager.LoadScene(DifficultySelectSceneName);
    }

    /// <summary>
    /// Applies selected difficulty parameters and loads the game scene.
    /// </summary>
    private void OnPlayClicked()
    {
        // Ensure values are clamped within valid range [0, 1]
        GameManager.Instance.CustomAlpha = Mathf.Clamp01(alphaSlider.value);
        GameManager.Instance.CustomGamma = Mathf.Clamp01(gammaSlider.value);
        GameManager.Instance.CustomEpsilon = Mathf.Clamp01(epsilonSlider.value);

        SceneManager.LoadScene(GameSceneName);
    }
}