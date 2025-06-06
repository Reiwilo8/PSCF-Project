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
        SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive);
        Time.timeScale = 0f;
    }

    public void OpenEndGameScreen(string result)
    {
        PlayerPrefs.SetString("EndGameResult", result);
        SceneManager.LoadScene("EndGameScene", LoadSceneMode.Additive);
        Time.timeScale = 0f;
    }
}
