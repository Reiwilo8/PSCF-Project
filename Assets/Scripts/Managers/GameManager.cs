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

    public Tile[,] board = new Tile[GridSize, GridSize];
    private Vector2Int winStart, winEnd;

    private bool swapNextStart = false;
    private bool currentRoundStarter = true;
    private bool overrideStarterNextGame = false;
    private bool overrideStarterValue = true;

    private bool gameEnded = false;

    private const int GridSize = 5;
    public int GetGridSize() => GridSize;

    /// <summary>
    /// Ensures singleton instance and registers scene load callback.
    /// </summary>
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

    /// <summary>
    /// Unsubscribes from scene load callback.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Initializes references and game state when the game scene loads.
    /// </summary>
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

    /// <summary>
    /// Sets the current game mode.
    /// </summary>
    public void SetGameMode(GameMode mode) => SelectedGameMode = mode;

    /// <summary>
    /// Sets the current AI difficulty.
    /// </summary>
    public void SetDifficulty(Difficulty diff) => SelectedDifficulty = diff;

    /// <summary>
    /// Ends the round and disables all tile interaction.
    /// </summary>
    public void EndRound()
    {
        gameEnded = true;
        BlockAllTiles();
    }

    /// <summary>
    /// Flags that the starter should be swapped for the next round only.
    /// </summary>
    public void SwapNextStarterOnce() => swapNextStart = true;

    /// <summary>
    /// Forces the current round starter to start the next round.
    /// </summary>
    public void RestartWithCurrentStarter() => SetStarterOverride(currentRoundStarter);

    /// <summary>
    /// Forces the next round to be started by the other player.
    /// </summary>
    public void RestartWithSwappedStarter() => SetStarterOverride(!currentRoundStarter);

    /// <summary>
    /// Sets starter override for the next round.
    /// </summary>
    private void SetStarterOverride(bool value)
    {
        overrideStarterNextGame = true;
        overrideStarterValue = value;
    }

    /// <summary>
    /// Initializes the turn order and triggers AI move if needed.
    /// </summary>
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

    /// <summary>
    /// Registers a player or AI move, checks for win or draw, and handles transition.
    /// </summary>
    public void RegisterMove(int x, int y, string symbol)
    {
        if (gameEnded || board[x, y].IsOccupied) return;

        board[x, y].SetSymbol(symbol);

        if (SelectedGameMode == GameMode.PvE && qLearningAI != null && !gameEnded)
        {
            string state = qLearningAI.BuildStateString(board);
            int action = y * GridSize + x;
            qLearningAI.RecordStep(state, action);
        }

        if (CheckForWin(x, y, symbol))
        {
            EndRound();
            statusUI?.ShowWin(symbol);
            qLearningAI?.LearnFromEpisode(symbol);
            LoadSceneWithDelay($"{symbol} Wins", 1f);
        }
        else if (IsBoardFull())
        {
            EndRound();
            statusUI?.ShowDraw();
            qLearningAI?.LearnFromEpisode("Draw");
            LoadSceneWithDelay("Draw", 1f);
        }
        else
        {
            SwitchTurn();
        }
    }

    /// <summary>
    /// Switches to the next player's turn and triggers AI if needed.
    /// </summary>
    private void SwitchTurn()
    {
        IsPlayerOneTurn = !IsPlayerOneTurn;
        statusUI?.UpdateStatus();

        if (!gameEnded && SelectedGameMode == GameMode.PvE && !IsPlayerOneTurn)
        {
            StartCoroutine(DelayedAIMove());
        }
    }

    /// <summary>
    /// Returns true if all tiles are occupied.
    /// </summary>
    private bool IsBoardFull()
    {
        foreach (var t in board)
            if (!t.IsOccupied) return false;
        return true;
    }

    /// <summary>
    /// Waits briefly before executing AI's move.
    /// </summary>
    private IEnumerator DelayedAIMove()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        MakeAiMove();
    }

    /// <summary>
    /// Requests the next move from the AI and registers it.
    /// </summary>
    private void MakeAiMove()
    {
        if (qLearningAI == null) return;

        int actionIndex = qLearningAI.GetNextMove(board, IsPlayerOneTurn);
        int ax = actionIndex % GridSize;
        int ay = actionIndex / GridSize;
        string aiSymbol = IsPlayerOneTurn ? "X" : "O";

        RegisterMove(ax, ay, aiSymbol);
    }

    /// <summary>
    /// Checks in all directions for a four-in-a-row win from the given tile.
    /// </summary>
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

            while (InBounds(x, y) && board[x, y].symbolText != null && board[x, y].symbolText.text == sym)
            {
                end = new Vector2Int(x, y);
                count++; x += d.x; y += d.y;
            }

            x = sx - d.x; y = sy - d.y;
            while (InBounds(x, y) && board[x, y].symbolText != null && board[x, y].symbolText.text == sym)
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

    /// <summary>
    /// Returns true if the given position is within the grid bounds.
    /// </summary>
    private bool InBounds(int x, int y)
    {
        return x >= 0 && x < GridSize && y >= 0 && y < GridSize;
    }

    /// <summary>
    /// Updates win/loss/draw stats based on result.
    /// </summary>
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

    /// <summary>
    /// Determines if the human player won based on the result.
    /// </summary>
    private bool DidPlayerWin(string result)
    {
        string playerSymbol = currentRoundStarter ? "X" : "O";
        return result.Contains(playerSymbol);
    }

    /// <summary>
    /// Triggers a delayed scene load with end-of-game message.
    /// </summary>
    public void LoadSceneWithDelay(string message, float delay)
    {
        StartCoroutine(LoadSceneCoroutine(message, delay));
    }

    /// <summary>
    /// Waits, updates stats, and opens the end game screen.
    /// </summary>
    private IEnumerator LoadSceneCoroutine(string message, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        UpdateGameStats(message);
        gameSceneUIManager?.OpenEndGameScreen();
    }

    /// <summary>
    /// Resets and reinitializes all tiles on the board.
    /// </summary>
    private void ResetBoard()
    {
        for (int x = 0; x < GridSize; x++)
            for (int y = 0; y < GridSize; y++)
                board[x, y].Init(x, y);
    }

    /// <summary>
    /// Disables interaction with all board tiles.
    /// </summary>
    private void BlockAllTiles()
    {
        for (int x = 0; x < GridSize; x++)
            for (int y = 0; y < GridSize; y++)
                board[x, y].button.interactable = false;
    }

    /// <summary>
    /// Resets all round-related state flags and stops running coroutines.
    /// </summary>
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