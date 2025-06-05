using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameSceneUI gameSceneUIManager;

    public WinLineDrawer winLineDrawer;
    public GameStatusUI statusUI;
    public enum GameMode { PvP, PvE }
    public enum Difficulty { Easy, Medium, Hard, Custom }

    public GameMode SelectedGameMode { get; set; }
    public Difficulty SelectedDifficulty { get; set; }

    public bool IsPlayerOneStarting { get; set; } = true;
    public bool IsPlayerOneTurn { get; private set; }
    private bool swapNextStart = false;

    private bool overrideStarterNextGame = false;
    private bool overrideStarterValue = true;

    public Tile[,] board = new Tile[5, 5];

    private Vector2Int winStart, winEnd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            statusUI = Object.FindAnyObjectByType<GameStatusUI>();
            gameSceneUIManager = Object.FindAnyObjectByType<GameSceneUI>();
            winLineDrawer = Object.FindAnyObjectByType<WinLineDrawer>();
            statusUI?.UpdateStatus();
        }
    }
    public void ResetTurnOrder()
    {
        if (overrideStarterNextGame)
        {
            IsPlayerOneTurn = overrideStarterValue;
            overrideStarterNextGame = false;
        }
        else if (swapNextStart)
        {
            IsPlayerOneTurn = !IsPlayerOneStarting;
            swapNextStart = false;
        }
        else
        {
            IsPlayerOneTurn = IsPlayerOneStarting;
        }

        statusUI?.UpdateStatus();
    }

    public void SwapNextStarterOnce() => swapNextStart = true;

    public void RestartWithCurrentStarter()
    {
        overrideStarterNextGame = true;
        overrideStarterValue = IsPlayerOneTurn;
    }

    public void RestartWithSwappedStarter()
    {
        overrideStarterNextGame = true;
        overrideStarterValue = !IsPlayerOneTurn;
    }

    public void SwitchTurn()
    {
        IsPlayerOneTurn = !IsPlayerOneTurn;
        statusUI?.UpdateStatus();
    }

    public bool CheckForWin(int x, int y, string symbol)
    {
        if (FindWinLine(x, y, 1, 0, symbol) ||
            FindWinLine(x, y, 0, 1, symbol) ||
            FindWinLine(x, y, 1, 1, symbol) ||
            FindWinLine(x, y, 1, -1, symbol))
        {
            EndGame(symbol);
            return true;
        }

        if (IsBoardFull())
        {
            EndGame("Draw");
            return true;
        }

        return false;
    }

    private bool FindWinLine(int sx, int sy, int dx, int dy, string sym)
    {
        int count = 1;
        Vector2Int s = new Vector2Int(sx, sy);
        Vector2Int e = new Vector2Int(sx, sy);

        int x = sx + dx, y = sy + dy;
        while (InBounds(x, y) && board[x, y].symbolText.text == sym)
        {
            e = new Vector2Int(x, y);
            count++; x += dx; y += dy;
        }

        x = sx - dx; y = sy - dy;
        while (InBounds(x, y) && board[x, y].symbolText.text == sym)
        {
            s = new Vector2Int(x, y);
            count++; x -= dx; y -= dy;
        }

        if (count >= 4)
        {
            winStart = s;
            winEnd = e;
            return true;
        }
        return false;
    }

    private bool InBounds(int x, int y) => x >= 0 && x < 5 && y >= 0 && y < 5;

    private bool IsBoardFull()
    {
        foreach (var t in board)
            if (t.symbolText.text == "") return false;
        return true;
    }

    public void LoadSceneWithDelay(string message, float delay)
    {
        StartCoroutine(LoadSceneCoroutine(message, delay));
    }

    private IEnumerator LoadSceneCoroutine(string message, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0f;
        gameSceneUIManager?.OpenEndGameScreen(message);
    }


    private void EndGame(string winner)
    {
        foreach (var t in board)
            t.button.interactable = false;

        if (winner != "Draw")
        {
            winLineDrawer?.DrawLine(winStart, winEnd);
            statusUI.ShowWin(winner);
        }
        else
        {
            statusUI.ShowDraw();
        }

            LoadSceneWithDelay(winner, 1f);
    }
}
