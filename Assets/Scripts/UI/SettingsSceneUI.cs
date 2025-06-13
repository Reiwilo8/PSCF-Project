using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingsSceneUI : MonoBehaviour
{
    public Button backButton;
    public Button profileButton;
    public Button aiButton;

    private string currentScene;
    private bool isTransitioning = false;

    private void Start()
    {
        currentScene = "SettingsProfileScene";

        backButton.onClick.AddListener(OnBackClicked);
        profileButton.onClick.AddListener(() => SwitchToScene("SettingsProfileScene"));
        aiButton.onClick.AddListener(() => SwitchToScene("SettingsAIScene"));

        StartCoroutine(LoadInitialScene());
    }

    private IEnumerator LoadInitialScene()
    {
        var asyncLoad = SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
        yield return asyncLoad;

        var loadedScene = SceneManager.GetSceneByName(currentScene);
        if (loadedScene.IsValid())
            SceneManager.SetActiveScene(loadedScene);

        UpdateButtonStates();
    }

    private void OnBackClicked()
    {
        UnloadIfLoaded(currentScene);
        SceneManager.LoadScene("StartScreen");
    }

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

    private void UnloadIfLoaded(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }

    private void UpdateButtonStates()
    {
        profileButton.interactable = currentScene != "SettingsProfileScene";
        aiButton.interactable = currentScene != "SettingsAIScene";
        backButton.interactable = true;
    }

    private void SetAllButtonsInteractable(bool state)
    {
        profileButton.interactable = state;
        aiButton.interactable = state;
        backButton.interactable = state;
    }
}
