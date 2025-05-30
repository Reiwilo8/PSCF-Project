using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameModeUI : MonoBehaviour
{
    public Button backButton;
    public Button pvpButton;
    public Button pveButton;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
        pvpButton.onClick.AddListener(OnPvPClicked);
        pveButton.onClick.AddListener(OnPvEClicked);
    }

    private void OnBackClicked()
    {
        SceneManager.LoadScene("StartScreen");
    }

    private void OnPvPClicked()
    {
        GameManager.Instance.SelectedGameMode = GameManager.GameMode.PvP;
    }

    private void OnPvEClicked()
    {
        GameManager.Instance.SelectedGameMode = GameManager.GameMode.PvE;
    }
}
