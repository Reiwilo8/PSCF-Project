using UnityEngine;
using UnityEngine.UI;

public class StartScreenUI : MonoBehaviour
{
    public Button playButton;
    public Button statisticsButton;
    public Button settingsButton;
    public Button exitButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        statisticsButton.onClick.AddListener(OnStatisticsClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    void OnPlayClicked()
    {
        // Tu póŸniej: otwieranie menu PvP/PvE
        Debug.Log("Play clicked");
    }

    void OnStatisticsClicked()
    {
        Debug.Log("Statistics clicked");
        // SceneManager.LoadScene("Statistics");
    }

    void OnSettingsClicked()
    {
        Debug.Log("Settings clicked");
        // SceneManager.LoadScene("Settings");
    }

    void OnExitClicked()
    {
        GameManager.Instance.QuitGame();
    }
}
