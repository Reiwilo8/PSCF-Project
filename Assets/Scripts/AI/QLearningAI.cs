using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Q-learning AI agent for a grid-based game (typically 5x5).
/// Maintains and updates a Q-table based on game outcomes.
/// </summary>
public class QLearningAI : MonoBehaviour
{
    // --- Q-learning parameters ---

    [Range(0f, 1f)] public float epsilon = 0.1f; // Exploration rate
    [Range(0f, 1f)] public float alpha = 0.5f;   // Learning rate
    [Range(0f, 1f)] public float gamma = 0.9f;   // Discount factor

    // Maps string-encoded board states to Q-value arrays (size: boardSize * boardSize)
    private Dictionary<string, float[]> qTable = new Dictionary<string, float[]>();

    // History of steps (state-action pairs) taken in current episode
    private List<(string state, int action)> episodeHistory = new List<(string, int)>();

    // Current board size retrieved from GameManager
    private int boardSize => GameManager.Instance.GetGridSize();

    // Symbol used by this AI ("X" or "O")
    private string aiSymbol = "O";

    /// <summary>
    /// Saves the Q-table to disk.
    /// </summary>
    public void SaveQTable()
    {
        QTableIO.Save(qTable);
    }

    /// <summary>
    /// Loads the Q-table from disk.
    /// </summary>
    public void LoadQTable()
    {
        qTable = QTableIO.Load();
    }

    /// <summary>
    /// Generates heuristic initial Q-values for a new state.
    /// Center, edges, and corners are weighted differently to reflect their value.
    /// </summary>
    private float[] GetHeuristicQValues()
    {
        float[] heuristic = new float[boardSize * boardSize];

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                int index = y * boardSize + x;

                bool isCenter = x == boardSize / 2 && y == boardSize / 2;
                bool isCorner = (x == 0 || x == boardSize - 1) && (y == 0 || y == boardSize - 1);
                bool isEdgeCenter = (x == boardSize / 2 || y == boardSize / 2);

                if (isCenter)
                    heuristic[index] = 0.4f;
                else if (isEdgeCenter)
                    heuristic[index] = 0.2f;
                else if (isCorner)
                    heuristic[index] = 0.1f;
                else
                    heuristic[index] = 0.0f;
            }
        }

        return heuristic;
    }

    /// <summary>
    /// Ensures that the Q-table contains an entry for the given state.
    /// If missing, initializes with heuristic Q-values.
    /// </summary>
    private void EnsureStateExists(string stateKey)
    {
        if (!qTable.ContainsKey(stateKey))
        {
            qTable[stateKey] = GetHeuristicQValues();
        }
    }

    /// <summary>
    /// Initializes the AI by setting hyperparameters and loading the Q-table.
    /// </summary>
    public void InitAI()
    {
        switch (GameManager.Instance.SelectedDifficulty)
        {
            case Difficulty.Easy:
                epsilon = 0.9f;
                alpha = 0.3f;
                gamma = 0.5f;
                break;
            case Difficulty.Medium:
                epsilon = 0.5f;
                alpha = 0.5f;
                gamma = 0.7f;
                break;
            case Difficulty.Hard:
                epsilon = 0.1f;
                alpha = 0.7f;
                gamma = 0.9f;
                break;
            case Difficulty.Custom:
                epsilon = GameManager.Instance.CustomEpsilon;
                alpha = GameManager.Instance.CustomAlpha;
                gamma = GameManager.Instance.CustomGamma;
                break;
        }

        LoadQTable();
        episodeHistory.Clear();
    }

    /// <summary>
    /// Sets the symbol the AI plays with ("X" or "O").
    /// </summary>
    public void SetAISymbol(string symbol)
    {
        aiSymbol = symbol;
    }

    /// <summary>
    /// Returns the AI's chosen move for the current board.
    /// Uses epsilon-greedy policy to balance exploration and exploitation.
    /// </summary>
    /// <summary>
    /// Returns the AI's selected move index for the given board state,
    /// or -1 if it's not the AI's turn.
    /// </summary>
    /// <param name="board">The current game board tiles.</param>
    /// <param name="isPlayerOneTurn">True if it's player one's turn.</param>
    /// <returns>Move index, or -1 if this AI should not move.</returns>
    public int GetNextMove(Tile[,] board, bool isPlayerOneTurn)
    {
        // Skip move if it's not this AI's turn
        if (isPlayerOneTurn)
            return -1;

        string currentState = BuildStateString(board);
        EnsureStateExists(currentState);

        float[] qValues = qTable[currentState];
        int size = boardSize;

        if (Random.value < epsilon)
        {
            // Explore: choose a random valid move
            List<int> validActions = new List<int>();

            for (int i = 0; i < qValues.Length; i++)
            {
                int x = i % size;
                int y = i / size;

                if (!board[x, y].IsOccupied)
                    validActions.Add(i);
            }

            if (validActions.Count > 0)
                return validActions[Random.Range(0, validActions.Count)];
        }

        // Exploit: choose the valid move with the highest Q-value
        float maxQ = float.NegativeInfinity;
        int bestAction = -1;

        for (int i = 0; i < qValues.Length; i++)
        {
            int x = i % size;
            int y = i / size;

            if (!board[x, y].IsOccupied && qValues[i] > maxQ)
            {
                maxQ = qValues[i];
                bestAction = i;
            }
        }

        return bestAction;
    }

    /// <summary>
    /// Builds a string representation of the current board state.
    /// Empty cells are represented with underscores.
    /// </summary>
    public string BuildStateString(Tile[,] board)
    {
        string state = "";

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                string symbol = board[x, y].symbolText.text;
                state += string.IsNullOrEmpty(symbol) ? "_" : symbol;
            }
        }

        return state;
    }

    /// <summary>
    /// Performs backward Q-learning updates on all recorded steps in the episode,
    /// based on the final game result.
    /// </summary>
    /// <param name="result">Final result string: "Draw", "X", or "O".</param>
    public void LearnFromEpisode(string result)
    {
        float reward;

        if (result == "Draw")
            reward = 0.5f;
        else if (result == aiSymbol)
            reward = 1f;
        else
            reward = 0f;

        for (int i = episodeHistory.Count - 1; i >= 0; i--)
        {
            var (state, action) = episodeHistory[i];
            EnsureStateExists(state);

            float[] qValues = qTable[state];
            float oldQ = qValues[action];
            float newQ = oldQ + alpha * (reward - oldQ);
            qValues[action] = newQ;

            reward *= gamma;
        }

        episodeHistory.Clear();
        SaveQTable();
    }

    /// <summary>
    /// Records the selected state-action pair for later learning updates.
    /// </summary>
    /// <param name="state">Board state string.</param>
    /// <param name="action">Action index taken in that state.</param>
    public void RecordStep(string state, int action)
    {
        episodeHistory.Add((state, action));
    }
}