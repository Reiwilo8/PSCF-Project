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
        SceneManager.LoadScene("GameModeScene");
    }

    void OnStatisticsClicked()
    {
        SceneManager.LoadScene("StatisticsScene");
    }

    void OnSettingsClicked()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    void OnExitClicked()
    {
        StatsManager.Instance.SaveStats();
        Application.Quit();
    }
}
