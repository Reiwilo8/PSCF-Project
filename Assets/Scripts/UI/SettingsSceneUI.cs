using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsSceneUI : MonoBehaviour
{
    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
        SceneManager.LoadScene("SettingsProfileScene", LoadSceneMode.Additive);
        //SceneManager.LoadScene("SettingsAIScene", LoadSceneMode.Additive);
    }

    private void OnBackClicked()
    {
        UnloadIfLoaded("SettingsProfileScene");
        UnloadIfLoaded("SettingsAIScene");
        SceneManager.LoadScene("StartScreen");
    }

    private void UnloadIfLoaded(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}
