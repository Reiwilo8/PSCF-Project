using UnityEngine;
using TMPro;

/// <summary>
/// Displays PvP game statistics in the PvP sub-scene of the Statistics screen.
/// </summary>
public class StatisticsPvPSceneUI : MonoBehaviour
{
    // UI labels assigned via Inspector
    [SerializeField] private TMP_Text totalPvPGamesLabel;
    [SerializeField] private TMP_Text totalPvPXWinsLabel;
    [SerializeField] private TMP_Text totalPvPOWinsLabel;
    [SerializeField] private TMP_Text totalPvPDrawsLabel;

    /// <summary>
    /// Initializes the PvP statistics labels using data from the StatsManager.
    /// </summary>
    private void Start()
    {
        var stats = StatsManager.Instance.Stats;

        if (totalPvPGamesLabel != null)
            totalPvPGamesLabel.text = stats.pvpGames.ToString();

        if (totalPvPXWinsLabel != null)
            totalPvPXWinsLabel.text = stats.pvpXWins.ToString();

        if (totalPvPOWinsLabel != null)
            totalPvPOWinsLabel.text = stats.pvpOWins.ToString();

        if (totalPvPDrawsLabel != null)
            totalPvPDrawsLabel.text = stats.pvpDraws.ToString();
    }
}