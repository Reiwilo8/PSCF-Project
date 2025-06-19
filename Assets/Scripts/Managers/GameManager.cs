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

    public QLearningAI qLearningAI;

    public GameMode SelectedGameMode { get; set; }
    public Difficulty SelectedDifficulty { get; set; }

    public float CustomEpsilon { get; set; } = 0.1f;
    public float CustomAlpha { get; set; } = 0.5f;
    public float CustomGamma { get; set; } = 0.9f;

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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject);
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
            qLearningAI = Object.FindAnyObjectByType<QLearningAI>();

            qLearningAI?.InitAI();
            ResetTurnOrder();
            ResetBoard();
            statusUI?.UpdateStatus();
        }
    }

    public void SetGameMode(GameMode mode) => SelectedGameMode = mode;
    public void SetDifficulty(Difficulty diff) => SelectedDifficulty = diff;

    public void EndRound()
    {
        gameEnded = true;
        BlockAllTiles();
    }

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

        if (SelectedGameMode == GameMode.PvE && qLearningAI != null)
        {
            string aiSymbol = IsPlayerOneTurn ? "O" : "X";
            qLearningAI.SetAISymbol(aiSymbol);
        }

        if (SelectedGameMode == GameMode.PvE && !IsPlayerOneTurn)
        {
            StartCoroutine(DelayedAIMove());
        }

        gameEnded = false;
        statusUI?.UpdateStatus();
        StatsManager.Instance.StartRoundTime();
    }

    public void RegisterMove(int x, int y, string symbol)
    {
        if (gameEnded) return;
        if (board[x, y].IsOccupied) return;

        board[x, y].SetSymbol(symbol);

        if (SelectedGameMode == GameMode.PvE && qLearningAI != null && !gameEnded)
        {
            string state = qLearningAI.BuildStateString(board);
            int action = y * 5 + x;
            qLearningAI.RecordStep(state, action);
        }

        if (CheckForWin(x, y, symbol))
        {
            EndRound();
            statusUI.ShowWin(symbol);

            if (SelectedGameMode == GameMode.PvE && qLearningAI != null)
            {
                qLearningAI.LearnFromEpisode(symbol);
            }

            string message = $"{symbol} Wins";

            LoadSceneWithDelay(message, 1f);
        }
        else if (IsBoardFull())
        {
            EndRound();
            statusUI.ShowDraw();

            const string message = "Draw";

            if (SelectedGameMode == GameMode.PvE && qLearningAI != null)
            {
                qLearningAI.LearnFromEpisode(message);
            }

            LoadSceneWithDelay(message, 1f);
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

        if (!gameEnded && SelectedGameMode == GameMode.PvE && !IsPlayerOneTurn)
        {
            StartCoroutine(DelayedAIMove());
        }
    }

    private bool IsBoardFull()
    {
        foreach (var t in board)
            if (!t.IsOccupied) return false;
        return true;
    }

    private IEnumerator DelayedAIMove()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        MakeAiMove();
    }

    private void MakeAiMove()
    {
        if (qLearningAI == null) return;

        int actionIndex = qLearningAI.GetNextMove(board, IsPlayerOneTurn);

        int ax = actionIndex % 5;
        int ay = actionIndex / 5;

        string aiSymbol = IsPlayerOneTurn ? "X" : "O";

        RegisterMove(ax, ay, aiSymbol);
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

    private void UpdateGameStats(string result)
    {
        var stats = StatsManager.Instance.Stats;

        if (SelectedGameMode == GameMode.PvP)
        {
            stats.pvpGames++;
            if (result.Contains("X")) stats.pvpXWins++;
            else if (result.Contains("O")) stats.pvpOWins++;
            else stats.pvpDraws++;
        }
        else if (SelectedGameMode == GameMode.PvE)
        {
            bool playerWon = DidPlayerWin(result);

            stats.pveGames++;

            switch (SelectedDifficulty)
            {
                case Difficulty.Easy:
                    stats.pveGamesEasy++;
                    if (result == "Draw") stats.pveDrawsEasy++;
                    else if (playerWon) stats.pvePlayerWinsEasy++;
                    break;
                case Difficulty.Medium:
                    stats.pveGamesMedium++;
                    if (result == "Draw") stats.pveDrawsMedium++;
                    else if (playerWon) stats.pvePlayerWinsMedium++;
                    break;
                case Difficulty.Hard:
                    stats.pveGamesHard++;
                    if (result == "Draw") stats.pveDrawsHard++;
                    else if (playerWon) stats.pvePlayerWinsHard++;
                    break;
            }
        }

        StatsManager.Instance.SaveStats();
    }

    private bool DidPlayerWin(string result)
    {
        string playerSymbol = currentRoundStarter ? "X" : "O";
        return result.Contains(playerSymbol);
    }

    public void LoadSceneWithDelay(string message, float delay)
    {
        StartCoroutine(LoadSceneCoroutine(message, delay));
    }

    private IEnumerator LoadSceneCoroutine(string message, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        UpdateGameStats(message);
        gameSceneUIManager?.OpenEndGameScreen();
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

    public void ResetAllFlags()
    {
        swapNextStart = false;
        overrideStarterNextGame = false;
        gameEnded = false;
        IsPlayerOneTurn = IsPlayerOneStarting;
        CancelInvoke();
        StopAllCoroutines();
    }
}
