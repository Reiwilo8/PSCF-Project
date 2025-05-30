using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        SceneManager.LoadScene("GameModeScreen");
    }

    void OnStatisticsClicked()
    {
        
    }

    void OnSettingsClicked()
    {
        
    }

    void OnExitClicked()
    {
        GameManager.Instance.QuitGame();
    }
}
