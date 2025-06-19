using System;

[Serializable]
public class GameStats
{
    public float totalTimeInApp = 0f;
    public float totalTimeInRounds = 0f;

    public int pvpGames = 0;
    public int pvpXWins = 0;
    public int pvpOWins = 0;
    public int pvpDraws = 0;

    public int pveGames = 0;

    public int pveGamesEasy = 0;
    public int pvePlayerWinsEasy = 0;
    public int pveDrawsEasy = 0;

    public int pveGamesMedium = 0;
    public int pvePlayerWinsMedium = 0;
    public int pveDrawsMedium = 0;

    public int pveGamesHard = 0;
    public int pvePlayerWinsHard = 0;
    public int pveDrawsHard = 0;
}
