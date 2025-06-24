using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Displays PvE game statistics by difficulty and adjusts layout font sizes uniformly.
/// </summary>
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

    /// <summary>
    /// Initializes PvE statistic values and synchronizes font sizes for grid labels.
    /// </summary>
    private void Start()
    {
        var stats = StatsManager.Instance.Stats;

        if (totalPvEGamesLabel != null)
            totalPvEGamesLabel.text = stats.pveGames.ToString();

        if (totalEasyGamesLabel != null)
            totalEasyGamesLabel.text = stats.pveGamesEasy.ToString();
        if (totalEasyWinsLabel != null)
            totalEasyWinsLabel.text = stats.pvePlayerWinsEasy.ToString();
        if (totalEasyDrawsLabel != null)
            totalEasyDrawsLabel.text = stats.pveDrawsEasy.ToString();

        if (totalMediumGamesLabel != null)
            totalMediumGamesLabel.text = stats.pveGamesMedium.ToString();
        if (totalMediumWinsLabel != null)
            totalMediumWinsLabel.text = stats.pvePlayerWinsMedium.ToString();
        if (totalMediumDrawsLabel != null)
            totalMediumDrawsLabel.text = stats.pveDrawsMedium.ToString();

        if (totalHardGamesLabel != null)
            totalHardGamesLabel.text = stats.pveGamesHard.ToString();
        if (totalHardWinsLabel != null)
            totalHardWinsLabel.text = stats.pvePlayerWinsHard.ToString();
        if (totalHardDrawsLabel != null)
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

    /// <summary>
    /// Normalizes font size across a group of TMP_Text elements by setting all to the smallest auto-sized font.
    /// </summary>
    /// <param name="texts">Array of text elements to process.</param>
    private IEnumerator SyncFontSize(TMP_Text[] texts)
    {
        yield return null; // Wait for TMP auto-sizing to apply

        float smallestSize = float.MaxValue;

        foreach (var text in texts)
        {
            if (text != null && text.enableAutoSizing)
                smallestSize = Mathf.Min(smallestSize, text.fontSize);
        }

        foreach (var text in texts)
        {
            if (text != null)
            {
                text.enableAutoSizing = false;
                text.fontSize = smallestSize;
            }
        }
    }
}