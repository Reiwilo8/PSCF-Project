using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StatisticsSceneUI : MonoBehaviour
{
    public Button backButton;
    public Button timeButton;
    public Button pvpButton;
    public Button pveButton;

    private string currentScene;
    private bool isTransitioning = false;

    private void Start()
    {
        currentScene = "StatisticsTimeScene";

        backButton.onClick.AddListener(OnBackClicked);
        timeButton.onClick.AddListener(() => SwitchToScene("StatisticsTimeScene"));
        pvpButton.onClick.AddListener(() => SwitchToScene("StatisticsPvPScene"));
        pveButton.onClick.AddListener(() => SwitchToScene("StatisticsPvEScene"));

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
        timeButton.interactable = currentScene != "StatisticsTimeScene";
        pvpButton.interactable = currentScene != "StatisticsPvPScene";
        pveButton.interactable = currentScene != "StatisticsPvEScene";
        backButton.interactable = true;
    }

    private void SetAllButtonsInteractable(bool state)
    {
        timeButton.interactable = state;
        pvpButton.interactable = state;
        pveButton.interactable = state;
        backButton.interactable = state;
    }
}
