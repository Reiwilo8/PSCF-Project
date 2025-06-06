using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode { PvP, PvE }
public enum Difficulty { Easy, Medium, Hard, Custom }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameSceneUI gameSceneUIManager;
    public GameStatusUI statusUI;
    public WinLineDrawer winLineDrawer;

    public GameMode SelectedGameMode { get; set; }
    public Difficulty SelectedDifficulty { get; set; }

    public bool IsPlayerOneStarting { get; private set; } = true;
    public bool IsPlayerOneTurn { get; private set; }

    public Tile[,] board = new Tile[5, 5];
    private Vector2Int winStart, winEnd;

    private bool swapNextStart = false;
    private bool currentRoundStarter = true;
    private bool overrideStarterNextGame = false;
    private bool overrideStarterValue = true;

    private bool gameEnded = false;

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
            gameSceneUIManager = Object.FindAnyObjectByType<GameSceneUI>();
            statusUI = Object.FindAnyObjectByType<GameStatusUI>();
            winLineDrawer = Object.FindAnyObjectByType<WinLineDrawer>();

            ResetTurnOrder();
            ResetBoard();
            statusUI?.UpdateStatus();
        }
    }

    public void SetGameMode(GameMode mode) => SelectedGameMode = mode;
    public void SetDifficulty(Difficulty diff) => SelectedDifficulty = diff;

    public void SwapNextStarterOnce()
    {
        swapNextStart = true;
    }

    public void RestartWithCurrentStarter()
    {
        overrideStarterNextGame = true;
        overrideStarterValue = currentRoundStarter;
    }

    public void RestartWithSwappedStarter()
    {
        overrideStarterNextGame = true;
        overrideStarterValue = !currentRoundStarter;
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

        currentRoundStarter = IsPlayerOneTurn;

        gameEnded = false;
        statusUI?.UpdateStatus();
    }

    public void RegisterMove(int x, int y, string symbol)
    {
        if (gameEnded) return;
        if (board[x, y].IsOccupied) return;

        board[x, y].SetSymbol(symbol);

        if (CheckForWin(x, y, symbol))
        {
            gameEnded = true;
            statusUI.ShowWin(symbol);
            BlockAllTiles();
            StartCoroutine(LoadSceneCoroutine($"{symbol} Wins", 1f));
        }
        else if (IsBoardFull())
        {
            gameEnded = true;
            statusUI.ShowDraw();
            BlockAllTiles();
            StartCoroutine(LoadSceneCoroutine("Draw", 1f));
        }
        else
        {
            SwitchTurn();
        }
    }

    private void SwitchTurn()
    {
        IsPlayerOneTurn = !IsPlayerOneTurn;
        statusUI?.UpdateStatus();
    }

    private bool IsBoardFull()
    {
        foreach (var t in board)
            if (!t.IsOccupied) return false;
        return true;
    }

    private bool CheckForWin(int sx, int sy, string sym)
    {
        Vector2Int[] dirs = {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1)
        };

        foreach (var d in dirs)
        {
            int count = 1;
            int x = sx + d.x, y = sy + d.y;
            Vector2Int start = new Vector2Int(sx, sy);
            Vector2Int end = new Vector2Int(sx, sy);

            while (InBounds(x, y) && board[x, y].symbolText.text == sym)
            {
                end = new Vector2Int(x, y);
                count++; x += d.x; y += d.y;
            }
            x = sx - d.x; y = sy - d.y;
            while (InBounds(x, y) && board[x, y].symbolText.text == sym)
            {
                start = new Vector2Int(x, y);
                count++; x -= d.x; y -= d.y;
            }

            if (count >= 4)
            {
                winLineDrawer?.DrawLine(start, end);
                return true;
            }
        }

        return false;
    }

    private bool InBounds(int x, int y)
    {
        return x >= 0 && x < 5 && y >= 0 && y < 5;
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

    private void ResetBoard()
    {
        for (int x = 0; x < 5; x++)
            for (int y = 0; y < 5; y++)
                board[x, y].Init(x, y);
    }

    private void BlockAllTiles()
    {
        for (int x = 0; x < 5; x++)
            for (int y = 0; y < 5; y++)
                board[x, y].button.interactable = false;
    }
}
