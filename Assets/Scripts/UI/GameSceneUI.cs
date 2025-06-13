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
        GameManager.Instance.PauseTime();
        SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive);
    }

    public void OpenEndGameScreen(string result)
    {
        PlayerPrefs.SetString("EndGameResult", result);
        SceneManager.LoadScene("EndGameScene", LoadSceneMode.Additive);
    }
}
