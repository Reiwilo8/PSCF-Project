using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Manages UI interactions within the Statistics screen, including sub-scene switching and navigation.
/// </summary>
public class StatisticsSceneUI : MonoBehaviour
{
    // UI button references (assigned via Inspector)
    public Button backButton;
    public Button timeButton;
    public Button pvpButton;
    public Button pveButton;

    // Scene name constants
    private const string TimeSceneName = "StatisticsTimeScene";
    private const string PvPSceneName = "StatisticsPvPScene";
    private const string PvESceneName = "StatisticsPvEScene";
    private const string StartScreenSceneName = "StartScreen";

    private string currentScene;
    private bool isTransitioning = false;

    /// <summary>
    /// Initializes button listeners and loads the default sub-scene.
    /// </summary>
    private void Start()
    {
        currentScene = TimeSceneName;

        if (backButton != null)
            backButton.onClick.AddListener(OnBackClicked);

        if (timeButton != null)
            timeButton.onClick.AddListener(() => SwitchToScene(TimeSceneName));

        if (pvpButton != null)
            pvpButton.onClick.AddListener(() => SwitchToScene(PvPSceneName));

        if (pveButton != null)
            pveButton.onClick.AddListener(() => SwitchToScene(PvESceneName));

        StartCoroutine(LoadInitialScene());
    }

    /// <summary>
    /// Loads the initial statistics sub-scene additively and sets it as active.
    /// </summary>
    private IEnumerator LoadInitialScene()
    {
        var asyncLoad = SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
        yield return asyncLoad;

        var loadedScene = SceneManager.GetSceneByName(currentScene);
        if (loadedScene.IsValid())
            SceneManager.SetActiveScene(loadedScene);

        UpdateButtonStates();
    }

    /// <summary>
    /// Returns to the Start Screen and unloads the current sub-scene.
    /// </summary>
    private void OnBackClicked()
    {
        UnloadIfLoaded(currentScene);
        SceneManager.LoadScene(StartScreenSceneName);
    }

    /// <summary>
    /// Switches between sub-scenes asynchronously, ensuring only one is active at a time.
    /// </summary>
    /// <param name="targetScene">The name of the sub-scene to switch to.</param>
    private async void SwitchToScene(string targetScene)
    {
        if (isTransitioning || currentScene == targetScene)
            return;

        isTransitioning = true;
        SetAllButtonsInteractable(false);

        if (SceneManager.GetSceneByName(currentScene).isLoaded)
        {
            await SceneManager.UnloadSceneAsync(currentScene);
        }

        currentScene = targetScene;
        await SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);

        var loadedScene = SceneManager.GetSceneByName(currentScene);
        if (loadedScene.IsValid())
            SceneManager.SetActiveScene(loadedScene);

        UpdateButtonStates();
        isTransitioning = false;
    }

    /// <summary>
    /// Unloads a scene if it's currently loaded.
    /// </summary>
    /// <param name="sceneName">The name of the scene to unload.</param>
    private void UnloadIfLoaded(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene.name);
        }
    }

    /// <summary>
    /// Updates button interactability based on the active sub-scene.
    /// </summary>
    private void UpdateButtonStates()
    {
        if (timeButton != null)
            timeButton.interactable = currentScene != TimeSceneName;

        if (pvpButton != null)
            pvpButton.interactable = currentScene != PvPSceneName;

        if (pveButton != null)
            pveButton.interactable = currentScene != PvESceneName;

        if (backButton != null)
            backButton.interactable = true;
    }

    /// <summary>
    /// Enables or disables all navigation buttons.
    /// </summary>
    /// <param name="state">Whether the buttons should be interactable.</param>
    private void SetAllButtonsInteractable(bool state)
    {
        if (timeButton != null)
            timeButton.interactable = state;

        if (pvpButton != null)
            pvpButton.interactable = state;

        if (pveButton != null)
            pveButton.interactable = state;

        if (backButton != null)
            backButton.interactable = state;
    }
}
