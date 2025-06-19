using UnityEngine;
using TMPro;

public class StatisticsPvPSceneUI : MonoBehaviour
{
    [SerializeField] private TMP_Text totalPvPGamesLabel;
    [SerializeField] private TMP_Text totalPvPXWinsLabel;
    [SerializeField] private TMP_Text totalPvPOWinsLabel;
    [SerializeField] private TMP_Text totalPvPDrawsLabel;

    private void Start()
    {
        var stats = StatsManager.Instance.Stats;

        totalPvPGamesLabel.text = stats.pvpGames.ToString();
        totalPvPXWinsLabel.text = stats.pvpXWins.ToString();
        totalPvPOWinsLabel.text = stats.pvpOWins.ToString();
        totalPvPDrawsLabel.text = stats.pvpDraws.ToString();
    }
}
