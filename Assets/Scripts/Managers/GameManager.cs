using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameMode { PvP, PvE }
    public enum Difficulty { Easy, Medium, Hard, Custom }

    public GameMode SelectedGameMode { get; set; }
    public Difficulty SelectedDifficulty { get; set; }

    public bool IsPlayerOneStarting { get; set; } = true;
    public bool IsPlayerOneTurn { get; private set; }
    private bool swapNextStart = false;

    public Tile[,] board = new Tile[5, 5];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void ResetTurnOrder()
    {
        if (swapNextStart)
        {
            IsPlayerOneTurn = !IsPlayerOneStarting;
            swapNextStart = false;
        }
        else
        {
            IsPlayerOneTurn = IsPlayerOneStarting;
        }
    }

    public void SwapNextStarterOnce()
    {
        swapNextStart = true;
    }

    public void SwitchTurn()
    {
        IsPlayerOneTurn = !IsPlayerOneTurn;
    }

    public bool CheckForWin(int x, int y, string symbol)
    {
        if (CheckDirection(x, y, 1, 0, symbol) ||
            CheckDirection(x, y, 0, 1, symbol) ||
            CheckDirection(x, y, 1, 1, symbol) ||
            CheckDirection(x, y, 1, -1, symbol))
        {
            EndGame($"{symbol} wins!");
            return true;
        }

        if (IsBoardFull())
        {
            EndGame("Draw!");
            return true;
        }

        return false;
    }

    private bool CheckDirection(int sx, int sy, int dx, int dy, string sym)
    {
        int count = 1;
        int x = sx + dx, y = sy + dy;
        while (InBounds(x, y) && board[x, y].symbolText.text == sym)
        {
            count++; x += dx; y += dy;
        }
        x = sx - dx; y = sy - dy;
        while (InBounds(x, y) && board[x, y].symbolText.text == sym)
        {
            count++; x -= dx; y -= dy;
        }
        return count >= 4;
    }

    private bool InBounds(int x, int y) => x >= 0 && x < 5 && y >= 0 && y < 5;

    private bool IsBoardFull()
    {
        foreach (var t in board)
            if (t.symbolText.text == "") return false;
        return true;
    }

    private void EndGame(string message)
    {
        foreach (var t in board) t.button.interactable = false;
        SceneManager.LoadScene("StartScreen");
    }
}
