using TMPro;
using UnityEngine;

public class GameStatusUI : MonoBehaviour
{
    public TextMeshProUGUI statusText;

    private void Start()
    {
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        var gm = GameManager.Instance;

        if (gm.SelectedGameMode == GameMode.PvP)
        {
            string symbol = gm.IsPlayerOneTurn ? "X" : "O";
            statusText.text = $"{symbol}'s Turn";
        }
        else if (gm.SelectedGameMode == GameMode.PvE)
        {
            statusText.text = $"{gm.SelectedDifficulty}";
        }
    }

    public void ShowWin(string winnerSymbol)
    {
        statusText.text = $"{winnerSymbol} Wins";
    }

    public void ShowDraw()
    {
        statusText.text = "Draw";
    }
}
