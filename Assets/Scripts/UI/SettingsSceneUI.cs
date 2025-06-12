using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsSceneUI : MonoBehaviour
{
    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
        SceneManager.LoadScene("SettingsAIScene", LoadSceneMode.Additive);
    }

    private void OnBackClicked()
    {
        SceneManager.UnloadSceneAsync("SettingsAIScene");
        SceneManager.LoadScene("StartScreen");
    }
}
