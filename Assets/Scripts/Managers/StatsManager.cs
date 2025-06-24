using UnityEngine;
using System.IO;

/// <summary>
/// Singleton manager for tracking, saving, and loading persistent game statistics.
/// Accumulates app and round time, and delegates stat reset control.
/// </summary>
public class StatsManager : MonoBehaviour
{
    // Singleton reference
    public static StatsManager Instance;

    // Runtime statistics container
    public GameStats Stats = new GameStats();

    // File path for serialized stats
    private string filePath;

    // Flag to determine if round time should be accumulating
    private bool isCountingRoundTime = false;

    /// <summary>
    /// Initializes singleton instance and loads stats from file.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            filePath = Application.persistentDataPath + "/stats.json";
            LoadStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Adds to total time counters each frame (unscaled).
    /// </summary>
    private void Update()
    {
        Stats.totalTimeInApp += Time.unscaledDeltaTime;

        if (isCountingRoundTime)
        {
            Stats.totalTimeInRounds += Time.unscaledDeltaTime;
        }
    }

    /// <summary>
    /// Saves current stats to disk as JSON.
    /// </summary>
    public void SaveStats()
    {
        File.WriteAllText(filePath, JsonUtility.ToJson(Stats));
    }

    /// <summary>
    /// Loads stats from file, or initializes new if unreadable.
    /// </summary>
    public void LoadStats()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                Stats = JsonUtility.FromJson<GameStats>(json);
            }
            catch
            {
                Stats = new GameStats(); // fallback if file is corrupted
            }
        }
    }

    /// <summary>
    /// Clears all stats and saves fresh instance.
    /// </summary>
    public void ResetStats()
    {
        Stats = new GameStats();
        SaveStats();
    }

    /// <summary>
    /// Begins tracking round playtime.
    /// </summary>
    public void StartRoundTime()
    {
        isCountingRoundTime = true;
    }

    /// <summary>
    /// Stops tracking round playtime.
    /// </summary>
    public void StopRoundTime()
    {
        isCountingRoundTime = false;
    }
}