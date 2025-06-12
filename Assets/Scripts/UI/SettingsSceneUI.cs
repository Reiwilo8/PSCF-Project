using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsSceneUI : MonoBehaviour
{
    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
    }

    private void OnBackClicked()
    {
        SceneManager.LoadScene("StartScreen");
    }
}
