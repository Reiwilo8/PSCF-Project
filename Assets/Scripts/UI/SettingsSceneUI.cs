using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Manages UI interactions within the Settings screen, including sub-scene switching and navigation.
/// </summary>
public class SettingsSceneUI : MonoBehaviour
{
    // UI button references (assigned via Inspector)
    public Button backButton;
    public Button profileButton;
    public Button aiButton;

    // Scene name constants
    private const string ProfileSceneName = "SettingsProfileScene";
    private const string AISceneName = "SettingsAIScene";
    private const string StartScreenSceneName = "StartScreen";

    private string currentScene;
    private bool isTransitioning = false;

    /// <summary>
    /// Initializes button listeners and loads the default sub-scene.
    /// </summary>
    private void Start()
    {
        currentScene = ProfileSceneName;

        if (backButton != null)
            backButton.onClick.AddListener(OnBackClicked);

        if (profileButton != null)
            profileButton.onClick.AddListener(() => SwitchToScene(ProfileSceneName));

        if (aiButton != null)
            aiButton.onClick.AddListener(() => SwitchToScene(AISceneName));

        StartCoroutine(LoadInitialScene());
    }

    /// <summary>
    /// Loads the initial settings sub-scene additively and sets it as active.
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
            SceneManager.UnloadSceneAsync(scene);
        }
    }

    /// <summary>
    /// Updates button interactability based on the active sub-scene.
    /// </summary>
    private void UpdateButtonStates()
    {
        if (profileButton != null)
            profileButton.interactable = currentScene != ProfileSceneName;

        if (aiButton != null)
            aiButton.interactable = currentScene != AISceneName;

        if (backButton != null)
            backButton.interactable = true;
    }

    /// <summary>
    /// Enables or disables all navigation buttons.
    /// </summary>
    /// <param name="state">Whether the buttons should be interactable.</param>
    private void SetAllButtonsInteractable(bool state)
    {
        if (profileButton != null)
            profileButton.interactable = state;

        if (aiButton != null)
            aiButton.interactable = state;

        if (backButton != null)
            backButton.interactable = state;
    }
}