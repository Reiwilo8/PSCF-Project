using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    public Button rematchButton;
    public Button swapButton;
    public Button menuButton;

    private void Start()
    {
        rematchButton.onClick.AddListener(OnRematchClicked);
        swapButton.onClick.AddListener(OnSwapClicked);
        menuButton.onClick.AddListener(OnMenuClicked);
    }

    void OnRematchClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    void OnSwapClicked()
    {
        Time.timeScale = 1f;
        GameManager.Instance.SwapNextStarterOnce();
        SceneManager.LoadScene("GameScene");
    }


    void OnMenuClicked()
    {
        GameManager.Instance.ResetAllFlags();
        SceneManager.LoadScene("StartScreen");
    }
}
