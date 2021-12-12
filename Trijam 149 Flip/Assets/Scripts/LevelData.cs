using System;

[Serializable]
public class LevelData
{
    public int Coins;
    public int Kills;
    float TimeRemaining;

    public bool SetCoins(int Coins)
    {
        if (this.Coins < Coins)
        {
            this.Coins = Coins;
            return true;
        }
        return false;
    }

    public bool SetTimeRemaining(float TimeRemaining)
    {
        if (this.TimeRemaining < TimeRemaining)
        {
            this.TimeRemaining = TimeRemaining;
            return true;
        }
        return false;
    }

    public bool SetKills(int Kills)
    {
        if (this.Kills < Kills)
        {
            this.Kills = Kills;
            return true;
        }
        return false;
    }

}
