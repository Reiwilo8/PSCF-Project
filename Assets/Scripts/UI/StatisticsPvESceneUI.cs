using UnityEngine;
using TMPro;

public class StatisticsPvESceneUI : MonoBehaviour
{
    [SerializeField] private TMP_Text totalPvEGamesLabel;

    [SerializeField] private TMP_Text totalEasyGamesLabel;
    [SerializeField] private TMP_Text totalEasyWinsLabel;
    [SerializeField] private TMP_Text totalEasyDrawsLabel;

    [SerializeField] private TMP_Text totalMediumGamesLabel;
    [SerializeField] private TMP_Text totalMediumWinsLabel;
    [SerializeField] private TMP_Text totalMediumDrawsLabel;

    [SerializeField] private TMP_Text totalHardGamesLabel;
    [SerializeField] private TMP_Text totalHardWinsLabel;
    [SerializeField] private TMP_Text totalHardDrawsLabel;

    private void Start()
    {
        var stats = StatsManager.Instance.Stats;

        totalPvEGamesLabel.text = stats.pveGames.ToString();

        totalEasyGamesLabel.text = stats.pveGamesEasy.ToString();
        totalEasyWinsLabel.text = stats.pvePlayerWinsEasy.ToString();
        totalEasyDrawsLabel.text = stats.pveDrawsEasy.ToString();

        totalMediumGamesLabel.text = stats.pveGamesMedium.ToString();
        totalMediumWinsLabel.text = stats.pvePlayerWinsMedium.ToString();
        totalMediumDrawsLabel.text = stats.pveDrawsMedium.ToString();

        totalHardGamesLabel.text = stats.pveGamesHard.ToString();
        totalHardWinsLabel.text = stats.pvePlayerWinsHard.ToString();
        totalHardDrawsLabel.text = stats.pveDrawsHard.ToString();
    }
}
