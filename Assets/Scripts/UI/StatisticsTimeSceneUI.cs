using UnityEngine;
using TMPro;

public class StatisticsTimeSceneUI : MonoBehaviour
{
    [SerializeField] private TMP_Text totalTimeInAppLabel;
    [SerializeField] private TMP_Text totalTimeInGamesLabel;

    private void Start()
    {
        var stats = StatsManager.Instance.Stats;

        totalTimeInAppLabel.text = FormatTime(stats.totalTimeInApp);
        totalTimeInGamesLabel.text = FormatTime(stats.totalTimeInRounds);
    }

    private string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}