using TMPro;
using UnityEngine;

/// <summary>
/// Handles display of game status messages (turns, results, etc.).
/// </summary>
public class GameStatusUI : MonoBehaviour
{
    // UI reference (assigned via Inspector)
    public TextMeshProUGUI statusText;

    /// <summary>
    /// Sets initial status message when the game starts.
    /// </summary>
    private void Start()
    {
        UpdateStatus();
    }

    /// <summary>
    /// Updates status based on current game state.
    /// </summary>
    public void UpdateStatus()
    {
        if (statusText == null || GameManager.Instance == null)
            return;

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

    /// <summary>
    /// Displays the winner symbol.
    /// </summary>
    public void ShowWin(string winnerSymbol)
    {
        if (statusText != null)
            statusText.text = $"{winnerSymbol} Wins";
    }

    /// <summary>
    /// Displays a draw message.
    /// </summary>
    public void ShowDraw()
    {
        if (statusText != null)
            statusText.text = "Draw";
    }
}