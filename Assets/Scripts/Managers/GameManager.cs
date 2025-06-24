using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode { PvP, PvE }
public enum Difficulty { Easy, Medium, Hard, Custom }

/// <summary>
/// Central controller for game state, flow, UI coordination, player turn handling,
/// win logic, and AI integration in both PvP and PvE modes.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance;

    // UI references (assigned via scene lookup)
    public GameSceneUI gameSceneUIManager;
    public GameStatusUI statusUI;
    public WinLineDrawer winLineDrawer;

    // AI logic handler (used in PvE mode)
    public QLearningAI qLearningAI;

    // Game configuration
    public GameMode SelectedGameMode { get; set; }
    public Difficulty SelectedDifficulty { get; set; }

    // Custom difficulty parameters (used only if Difficulty.Custom is selected)
    public float CustomEpsilon { get; set; } = 0.1f;
    public float CustomAlpha { get; set; } = 0.5f;
    public float CustomGamma { get; set; } = 0.9f;

    // Player state
    public bool IsPlayerOneStarting { get; private set; } = true;
    public bool IsPlayerOneTurn { get; private set; }

    // Game board reference
    public Tile[,] board = new Tile[GridSize, GridSize];

    // Internal state tracking
    private Vector2Int winStart, winEnd;
    private bool swapNextStart = false;
    private bool currentRoundStarter = true;
    private bool overrideStarterNextGame = false;
    private bool overrideStarterValue = true;
    private bool gameEnded = false;

    // Grid size for consistent board dimensions
    private const int GridSize = 5;
    public int GetGridSize() => GridSize;

    /// <summary>
    /// Initializes the singleton and registers scene load event.
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
    /// Ensures proper cleanup by unregistering scene events.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Called when a new scene is loaded. Initializes references and resets state if entering game scene.
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
    /// Sets the current game mode (PvP or PvE).
    /// </summary>
    public void SetGameMode(GameMode mode) => SelectedGameMode = mode;

    /// <summary>
    /// Sets the selected difficulty (used only in PvE).
    /// </summary>
    public void SetDifficulty(Difficulty diff) => SelectedDifficulty = diff;

    /// <summary>
    /// Ends the current round and disables further tile interaction.
    /// </summary>
    public void EndRound()
    {
        gameEnded = true;
        BlockAllTiles();
    }

    /// <summary>
    /// Swaps the starter player for the next round only.
    /// </summary>
    public void SwapNextStarterOnce() => swapNextStart = true;

    /// <summary>
    /// Forces the next round to start with the same player as the current round.
    /// </summary>
    public void RestartWithCurrentStarter() => SetStarterOverride(currentRoundStarter);

    /// <summary>
    /// Forces the next round to start with the opposite player.
    /// </summary>
    public void RestartWithSwappedStarter() => SetStarterOverride(!currentRoundStarter);

    /// <summary>
    /// Internally flags the starter override and its value.
    /// </summary>
    private void SetStarterOverride(bool value)
    {
        overrideStarterNextGame = true;
        overrideStarterValue = value;
    }

    /// <summary>
    /// Resets turn order, configures AI if needed, and starts the round timer.
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
    /// Handles a move being made on the board, by player or AI.
    /// Checks for win/draw conditions and manages state progression.
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
    /// Switches turn to the next player and triggers AI if applicable.
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
    /// Checks whether the board is completely filled.
    /// </summary>
    private bool IsBoardFull()
    {
        foreach (var tile in board)
        {
            if (!tile.IsOccupied)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Delays AI move slightly to simulate thinking and allow UI update.
    /// </summary>
    private IEnumerator DelayedAIMove()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        MakeAiMove();
    }

    /// <summary>
    /// Retrieves the next move from AI and registers it as a board move.
    /// </summary>
    private void MakeAiMove()
    {
        if (qLearningAI == null) return;

        int actionIndex = qLearningAI.GetNextMove(board, IsPlayerOneTurn);
        int x = actionIndex % GridSize;
        int y = actionIndex / GridSize;
        string symbol = IsPlayerOneTurn ? "X" : "O";

        RegisterMove(x, y, symbol);
    }

    /// <summary>
    /// Checks in all four directions (horizontal, vertical, diagonals)
    /// to determine if the current move completes a winning sequence.
    /// </summary>
    private bool CheckForWin(int sx, int sy, string symbol)
    {
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),   // Horizontal
            new Vector2Int(0, 1),   // Vertical
            new Vector2Int(1, 1),   // Diagonal down-right
            new Vector2Int(1, -1)   // Diagonal up-right
        };

        foreach (var dir in directions)
        {
            int count = 1;
            int x = sx + dir.x, y = sy + dir.y;
            Vector2Int start = new Vector2Int(sx, sy);
            Vector2Int end = new Vector2Int(sx, sy);

            // Check forward direction
            while (InBounds(x, y) && board[x, y].symbolText != null && board[x, y].symbolText.text == symbol)
            {
                end = new Vector2Int(x, y);
                count++;
                x += dir.x;
                y += dir.y;
            }

            // Check backward direction
            x = sx - dir.x;
            y = sy - dir.y;
            while (InBounds(x, y) && board[x, y].symbolText != null && board[x, y].symbolText.text == symbol)
            {
                start = new Vector2Int(x, y);
                count++;
                x -= dir.x;
                y -= dir.y;
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
    /// Returns true if (x, y) is inside the bounds of the grid.
    /// </summary>
    private bool InBounds(int x, int y)
    {
        return x >= 0 && x < GridSize && y >= 0 && y < GridSize;
    }

    /// <summary>
    /// Updates session statistics based on the final game result.
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
    /// Determines whether the human player was the winner.
    /// </summary>
    private bool DidPlayerWin(string result)
    {
        string playerSymbol = currentRoundStarter ? "X" : "O";
        return result.Contains(playerSymbol);
    }

    /// <summary>
    /// Triggers a scene transition with a short delay after round end.
    /// </summary>
    public void LoadSceneWithDelay(string message, float delay)
    {
        StartCoroutine(LoadSceneCoroutine(message, delay));
    }

    /// <summary>
    /// Coroutine that handles end-of-round delay, stats update, and screen change.
    /// </summary>
    private IEnumerator LoadSceneCoroutine(string message, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        UpdateGameStats(message);
        gameSceneUIManager?.OpenEndGameScreen();
    }

    /// <summary>
    /// Resets and reinitializes the board tiles for a new round.
    /// </summary>
    private void ResetBoard()
    {
        for (int x = 0; x < GridSize; x++)
            for (int y = 0; y < GridSize; y++)
                board[x, y].Init(x, y);
    }

    /// <summary>
    /// Disables interaction for all board tiles.
    /// </summary>
    private void BlockAllTiles()
    {
        for (int x = 0; x < GridSize; x++)
            for (int y = 0; y < GridSize; y++)
                board[x, y].button.interactable = false;
    }

    /// <summary>
    /// Resets internal flags and cancels any active timers or coroutines.
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