using System.Collections.Generic;
using UnityEngine;

public class QLearningAI : MonoBehaviour
{
    [Range(0f, 1f)] public float epsilon = 0.1f;
    [Range(0f, 1f)] public float alpha = 0.5f;
    [Range(0f, 1f)] public float gamma = 0.9f;

    private Dictionary<string, float[]> qTable = new Dictionary<string, float[]>();

    public void SaveQTable()
    {
        QTableIO.Save(qTable);
    }

    public void LoadQTable()
    {
        qTable = QTableIO.Load();
    }

    private void EnsureStateExists(string stateKey)
    {
        if (!qTable.ContainsKey(stateKey))
        {
            qTable[stateKey] = new float[boardSize * boardSize];
            for (int i = 0; i < boardSize * boardSize; i++)
                qTable[stateKey][i] = 0f;
        }
    }

    private List<(string state, int action)> episodeHistory = new List<(string, int)>();
    private readonly int boardSize = 5;
    private readonly int winLength = 4;

    private string aiSymbol = "O";

    private string lastState = null;
    private int lastAction = -1;

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


    public void SetAISymbol(string symbol)
    {
        aiSymbol = symbol;
    }

    public int GetNextMove(Tile[,] board, bool isAITurn)
    {
        string state = BuildStateString(board);

        int action = ChooseAction(state, board);

        lastState = state;
        lastAction = action;

        return action;
    }

    public string BuildStateString(Tile[,] board)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int y = 0; y < board.GetLength(1); y++)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                string symbol = board[x, y].GetSymbol();
                sb.Append(string.IsNullOrEmpty(symbol) ? "_" : symbol);
            }
        }

        return sb.ToString();
    }

    private int ChooseAction(string state, Tile[,] board)
    {
        List<int> availableActions = new List<int>();

        for (int y = 0; y < board.GetLength(1); y++)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                if (board[x, y].IsEmpty())
                {
                    int index = y * 5 + x;
                    availableActions.Add(index);
                }
            }
        }

        if (!qTable.ContainsKey(state))
        {
            qTable[state] = new float[25];
        }

        if (UnityEngine.Random.value < epsilon)
        {
            return availableActions[UnityEngine.Random.Range(0, availableActions.Count)];
        }

        float[] qValues = qTable[state];
        float maxQ = float.NegativeInfinity;
        int bestAction = -1;

        foreach (int a in availableActions)
        {
            if (qValues[a] > maxQ)
            {
                maxQ = qValues[a];
                bestAction = a;
            }
        }

        return bestAction != -1 ? bestAction : availableActions[UnityEngine.Random.Range(0, availableActions.Count)];
    }

    public void LearnFromEpisode(string gameResult)
    {
        float finalReward = (gameResult == "Draw") ? 0f :
                            (gameResult == aiSymbol ? 1f : -1f);

        float discountedReward = finalReward;

        for (int i = episodeHistory.Count - 1; i >= 0; i--)
        {
            var (state, action) = episodeHistory[i];
            UpdateQ(state, action, discountedReward, null);

            discountedReward *= gamma;
        }

        episodeHistory.Clear();
        SaveQTable();
    }


    public void RecordStep(string state, int action)
    {
        episodeHistory.Add((state, action));
    }

    private void UpdateQ(string stateKey, int action, float reward, string nextStateKey)
    {
        EnsureStateExists(stateKey);

        float[] qValues = qTable[stateKey];
        float qSA = qValues[action];
        float maxQNext = 0f;

        if (!string.IsNullOrEmpty(nextStateKey))
        {
            EnsureStateExists(nextStateKey);
            float[] qNext = qTable[nextStateKey];
            maxQNext = float.MinValue;

            foreach (float v in qNext)
            {
                if (v > maxQNext)
                    maxQNext = v;
            }

            if (maxQNext == float.MinValue)
                maxQNext = 0f;
        }

        qValues[action] = qSA + alpha * (reward + gamma * maxQNext - qSA);
    }
}