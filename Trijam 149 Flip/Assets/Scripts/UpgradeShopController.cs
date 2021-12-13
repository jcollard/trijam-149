using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopController : MonoBehaviour
{
    public Text CoinsEarned;
    public Text Backflips, MaxTime, BonusTime;
    public Text BackflipButtonText, MaxTimeButtonText, BonusTimeButtonText;

    public float BackflipCost = 1;
    public float MaxTimeCost = 1;
    public float BonusTimeCost = 1;
    private float MaxCost = 10;
    public int CoinsSpent = 0;
    public int TotalCoins
    {
        get
        {
            int TotalCoins = 0;
            PlayerController p = PlayerController.Instance;
            if (p == null)
            {
                return 0;
            }
            foreach (LevelData ld in p.Levels)
            {
                TotalCoins += ld.Coins;
            }
            return TotalCoins;
        }
    }

    public int RemainingCoins => TotalCoins - CoinsSpent;

    public void OnEnable()
    {
        DrawText();
    }

    public void DrawText()
    {
        PlayerController p = PlayerController.Instance;
        if (p == null)
        {
            Debug.Log("PlayerInstancew as null");
            return;
        }
        Debug.Log("Updating Text");
        Debug.Log($"Total Coins? {TotalCoins}");
        CoinsEarned.text = $"{RemainingCoins}/{TotalCoins}";
        Backflips.text = $"{p.MaxBackflips}";
        BackflipButtonText.text = $"{(int)BackflipCost} Coins";

        MaxTime.text = $"{p.MaxTime:0.00}";
        MaxTimeButtonText.text = $"{(int)MaxTimeCost} Coins";

        BonusTime.text = $"{p.BonusTime:0.00}";
        BonusTimeButtonText.text = $"{(int)BonusTimeCost} Coins";

    }

    public void UpgradeBackflips()
    {
        if (RemainingCoins >= (int)BackflipCost)
        {
            CoinsSpent += (int)BackflipCost;
            PlayerController.Instance.MaxBackflips++;
            PlayerController.Instance.Backflips = PlayerController.Instance.MaxBackflips;
            BackflipCost += 0.5f;
            BackflipCost = Mathf.Min(BackflipCost, MaxCost);
            DrawText();
        }
    }

    public void UpgradeMaxTime()
    {
        if (RemainingCoins >= (int)MaxTimeCost)
        {
            CoinsSpent += (int)MaxTimeCost;
            PlayerController.Instance.MaxTime += 0.10f;
            MaxTimeCost += 0.5f;
            MaxTimeCost = Mathf.Min(MaxTimeCost, MaxCost);
            DrawText();
        }
    }

    public void UpgradeBonusTime()
    {
        if (RemainingCoins >= (int)BonusTimeCost)
        {
            CoinsSpent += (int)BonusTimeCost;
            PlayerController.Instance.BonusTime += 0.10f;
            BonusTimeCost += 0.5f;
            BonusTimeCost = Mathf.Min(BonusTimeCost, MaxCost);
            DrawText();
        }
    }

    public void RefundCoins()
    {
        CoinsSpent = 0;
        PlayerController p = PlayerController.Instance;
        p.MaxTime = 5;
        p.BonusTime = 1f;
        p.MaxBackflips = 1;
        p.Backflips = 1;
        BackflipCost = 1;
        MaxTimeCost = 1;
        BonusTimeCost = 1;
        DrawText();
    }
}
