using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    public Button pauseButton;

    private void Start()
    {
        pauseButton.onClick.AddListener(OnPauseClicked);
    }
    public void OnPauseClicked()
    {
        StatsManager.Instance.StopRoundTime();
        SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive);
    }

    public void OpenEndGameScreen()
    {
        StatsManager.Instance.StopRoundTime();
        SceneManager.LoadScene("EndGameScene", LoadSceneMode.Additive);
    }
}
