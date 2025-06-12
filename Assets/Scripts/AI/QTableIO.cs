using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class QTableIO
{
    private static readonly string SavePath = Application.persistentDataPath + "/qtable.json";

    public static void Save(Dictionary<string, float[]> qTable)
    {
        QTableWrapper wrapper = new QTableWrapper();
        foreach (var pair in qTable)
        {
            wrapper.entries.Add(new QEntry { state = pair.Key, qValues = pair.Value });
        }

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(SavePath, json);
    }

    public static Dictionary<string, float[]> Load()
    {
        if (!File.Exists(SavePath))
        {
            return new Dictionary<string, float[]>();
        }

        string json = File.ReadAllText(SavePath);
        QTableWrapper wrapper = JsonUtility.FromJson<QTableWrapper>(json);

        Dictionary<string, float[]> qTable = new Dictionary<string, float[]>();
        foreach (var entry in wrapper.entries)
        {
            qTable[entry.state] = entry.qValues;
        }

        return qTable;
    }

    public static void Reset()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }
        else
        {
        }
    }
}
