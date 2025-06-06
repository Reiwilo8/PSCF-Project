using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DifficultySelectUI : MonoBehaviour
{
    public Button backButton;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button customButton;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
        easyButton.onClick.AddListener(() => OnDifficultySelected(Difficulty.Easy));
        mediumButton.onClick.AddListener(() => OnDifficultySelected(Difficulty.Medium));
        hardButton.onClick.AddListener(() => OnDifficultySelected(Difficulty.Hard));
        customButton.onClick.AddListener(() => OnDifficultySelected(Difficulty.Custom));
    }

    void OnBackClicked()
    {
        SceneManager.LoadScene("GameModeScene");
    }

    void OnDifficultySelected(Difficulty difficulty)
    {
        GameManager.Instance.SetGameMode(GameMode.PvE);
        GameManager.Instance.SetDifficulty(difficulty);

        SceneManager.LoadScene("GameScene");
    }
}
