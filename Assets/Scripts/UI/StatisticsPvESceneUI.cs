using UnityEngine;
using TMPro;
using System.Collections;

public class StatisticsPvESceneUI : MonoBehaviour
{
    [SerializeField] private TMP_Text totalPvEGamesLabel;

    [Header("Column Headers")]
    [SerializeField] private TMP_Text easyLabel;
    [SerializeField] private TMP_Text mediumLabel;
    [SerializeField] private TMP_Text hardLabel;

    [Header("Row Labels")]
    [SerializeField] private TMP_Text gamesRowLabel;
    [SerializeField] private TMP_Text winsRowLabel;
    [SerializeField] private TMP_Text drawsRowLabel;

    [Header("Easy Stats")]
    [SerializeField] private TMP_Text totalEasyGamesLabel;
    [SerializeField] private TMP_Text totalEasyWinsLabel;
    [SerializeField] private TMP_Text totalEasyDrawsLabel;

    [Header("Medium Stats")]
    [SerializeField] private TMP_Text totalMediumGamesLabel;
    [SerializeField] private TMP_Text totalMediumWinsLabel;
    [SerializeField] private TMP_Text totalMediumDrawsLabel;

    [Header("Hard Stats")]
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

        TMP_Text[] gridTexts = new TMP_Text[]
        {
            easyLabel, mediumLabel, hardLabel,
            gamesRowLabel, winsRowLabel, drawsRowLabel,

            totalEasyGamesLabel, totalEasyWinsLabel, totalEasyDrawsLabel,
            totalMediumGamesLabel, totalMediumWinsLabel, totalMediumDrawsLabel,
            totalHardGamesLabel, totalHardWinsLabel, totalHardDrawsLabel
        };

        StartCoroutine(SyncFontSize(gridTexts));
    }

    private IEnumerator SyncFontSize(TMP_Text[] texts)
    {
        yield return null;

        float smallestSize = float.MaxValue;

        foreach (var text in texts)
        {
            if (text.enableAutoSizing)
                smallestSize = Mathf.Min(smallestSize, text.fontSize);
        }

        foreach (var text in texts)
        {
            text.enableAutoSizing = false;
            text.fontSize = smallestSize;
        }
    }
}
