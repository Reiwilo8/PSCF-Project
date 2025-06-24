using UnityEngine;
using TMPro;

/// <summary>
/// Displays time-based statistics in the Time sub-scene of the Statistics screen.
/// </summary>
public class StatisticsTimeSceneUI : MonoBehaviour
{
    // UI labels assigned via Inspector
    [SerializeField] private TMP_Text totalTimeInAppLabel;
    [SerializeField] private TMP_Text totalTimeInGamesLabel;

    /// <summary>
    /// Initializes the time labels using data from the StatsManager.
    /// </summary>
    private void Start()
    {
        var stats = StatsManager.Instance.Stats;

        if (totalTimeInAppLabel != null)
            totalTimeInAppLabel.text = FormatTime(stats.totalTimeInApp);

        if (totalTimeInGamesLabel != null)
            totalTimeInGamesLabel.text = FormatTime(stats.totalTimeInRounds);
    }

    /// <summary>
    /// Converts time from seconds to a formatted HH:MM:SS string.
    /// </summary>
    /// <param name="timeInSeconds">Time value in seconds.</param>
    /// <returns>Formatted time string.</returns>
    private static string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}