using UnityEngine;
using System.IO;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public GameStats Stats = new GameStats();

    private string filePath;

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

    private void Update()
    {
        Stats.totalTimeInApp += Time.unscaledDeltaTime;
    }

    public void SaveStats()
    {
        File.WriteAllText(filePath, JsonUtility.ToJson(Stats));
    }

    public void LoadStats()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Stats = JsonUtility.FromJson<GameStats>(json);
        }
    }

    public void ResetStats()
    {
        Stats = new GameStats();
        SaveStats();
    }
}
