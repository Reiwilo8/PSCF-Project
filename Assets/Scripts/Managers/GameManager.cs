using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameMode { PvP, PvE }
    public enum Difficulty { Easy, Medium, Hard, Custom }

    public GameMode SelectedGameMode { get; set; }
    public Difficulty SelectedDifficulty { get; set; }

    public bool IsPlayerOneTurn { get; private set; } = true;

    public Tile[,] board = new Tile[5, 5];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SwitchTurn()
    {
        IsPlayerOneTurn = !IsPlayerOneTurn;

        if (SelectedGameMode == GameMode.PvE && !IsPlayerOneTurn)
        {
            Invoke(nameof(MakeAiMove), 0.5f);
        }
    }

    private void MakeAiMove()
    {
        SwitchTurn();
    }

    public void CheckForWin(int x, int y, string symbol)
    {
        if (CheckDirection(x, y, 1, 0, symbol) ||
            CheckDirection(x, y, 0, 1, symbol) ||
            CheckDirection(x, y, 1, 1, symbol) ||
            CheckDirection(x, y, 1, -1, symbol))
        {
            EndGame($"{symbol} wins!");
            return;
        }

        if (IsBoardFull())
        {
            EndGame("Draw!");
        }
    }

    private bool CheckDirection(int startX, int startY, int dx, int dy, string symbol)
    {
        int count = 1;

        int x = startX + dx;
        int y = startY + dy;
        while (InBounds(x, y) && board[x, y].symbolText.text == symbol)
        {
            count++;
            x += dx;
            y += dy;
        }

        x = startX - dx;
        y = startY - dy;
        while (InBounds(x, y) && board[x, y].symbolText.text == symbol)
        {
            count++;
            x -= dx;
            y -= dy;
        }

        return count >= 4;
    }

    private bool InBounds(int x, int y)
    {
        return x >= 0 && x < 5 && y >= 0 && y < 5;
    }

    private bool IsBoardFull()
    {
        foreach (var tile in board)
        {
            if (tile.symbolText.text == "")
                return false;
        }
        return true;
    }

    private void EndGame(string message)
    {
        foreach (var tile in board)
        {
            tile.button.interactable = false;
        }

        SceneManager.LoadScene("StartScreen");
    }
}
