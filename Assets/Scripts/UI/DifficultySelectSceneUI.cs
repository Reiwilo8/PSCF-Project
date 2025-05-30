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
        easyButton.onClick.AddListener(() => OnDifficultySelected(GameManager.Difficulty.Easy));
        mediumButton.onClick.AddListener(() => OnDifficultySelected(GameManager.Difficulty.Medium));
        hardButton.onClick.AddListener(() => OnDifficultySelected(GameManager.Difficulty.Hard));
        customButton.onClick.AddListener(() => OnDifficultySelected(GameManager.Difficulty.Custom));
    }

    void OnBackClicked()
    {
        SceneManager.LoadScene("GameModeScene");
    }

    void OnDifficultySelected(GameManager.Difficulty difficulty)
    {
        GameManager.Instance.SelectedGameMode = GameManager.GameMode.PvE;
        GameManager.Instance.SelectedDifficulty = difficulty;

        SceneManager.LoadScene("GameScene");
    }
}
