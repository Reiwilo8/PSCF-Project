using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Static utility for saving, loading, and resetting Q-learning data from persistent storage.
/// </summary>
public static class QTableIO
{
    // File path where the Q-table is stored
    private static readonly string filePath = Application.persistentDataPath + "/qtable.json";

    /// <summary>
    /// Saves the current Q-table to disk as JSON.
    /// </summary>
    public static void Save(Dictionary<string, float[]> qTable)
    {
        QTableWrapper wrapper = new QTableWrapper();

        foreach (var entry in qTable)
        {
            wrapper.entries.Add(new QEntry
            {
                state = entry.Key,
                qValues = entry.Value
            });
        }

        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Loads a Q-table from disk, or returns a new empty table if none exists or if loading fails.
    /// </summary>
    public static Dictionary<string, float[]> Load()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                QTableWrapper wrapper = JsonUtility.FromJson<QTableWrapper>(json);

                Dictionary<string, float[]> qTable = new Dictionary<string, float[]>();
                foreach (QEntry entry in wrapper.entries)
                {
                    qTable[entry.state] = entry.qValues;
                }

                return qTable;
            }
            catch
            {
                // In case of JSON error or file corruption, fallback to new table
                return new Dictionary<string, float[]>();
            }
        }

        return new Dictionary<string, float[]>();
    }

    /// <summary>
    /// Deletes the saved Q-table file from disk.
    /// </summary>
    public static void Reset()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}