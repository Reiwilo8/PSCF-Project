using System;

/// <summary>
/// Serializable container for storing cumulative game statistics across sessions.
/// Used by StatsManager for tracking PvP and PvE performance.
/// </summary>
[Serializable]
public class GameStats
{
    // --- Total application time (in seconds) ---
    public float totalTimeInApp = 0;

    // --- Total time spent in active rounds (in seconds) ---
    public float totalTimeInRounds = 0;

    // --- PvP statistics ---
    public int pvpGames = 0;
    public int pvpXWins = 0;
    public int pvpOWins = 0;
    public int pvpDraws = 0;

    // --- PvE statistics (Easy) ---
    public int pveGamesEasy = 0;
    public int pvePlayerWinsEasy = 0;
    public int pveDrawsEasy = 0;

    // --- PvE statistics (Medium) ---
    public int pveGamesMedium = 0;
    public int pvePlayerWinsMedium = 0;
    public int pveDrawsMedium = 0;

    // --- PvE statistics (Hard) ---
    public int pveGamesHard = 0;
    public int pvePlayerWinsHard = 0;
    public int pveDrawsHard = 0;

    // --- Total PvE games played (sum of all difficulties) ---
    public int pveGames = 0;
}