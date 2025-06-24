using System;
using System.Collections.Generic;

/// <summary>
/// Represents a single entry in the Q-learning table.
/// Each state is mapped to an array of Q-values for possible actions.
/// </summary>
[Serializable]
public class QEntry
{
    // Serialized board state as a unique string key
    public string state;

    // Q-values for each possible move (typically 25 for a 5x5 board)
    public float[] qValues;
}

/// <summary>
/// Serializable container for saving or loading the full Q-table.
/// Used for JSON serialization of all Q-learning data.
/// </summary>
[Serializable]
public class QTableWrapper
{
    // List of all learned state-action entries
    public List<QEntry> entries = new List<QEntry>();
}