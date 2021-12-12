using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopController : MonoBehaviour
{
    public Text CoinsEarned;
    public Text Backflips, MaxTime, BonusTime;
    public Text BackflipButtonText, MaxTimeButtonText, BonusTimeButtonText;

    public int BackflipCost = 1;
    public int MaxTimeCost = 1;
    public int BonusTimeCost = 1;
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
        BackflipButtonText.text = $"{BackflipCost} Coins";

        MaxTime.text = $"{p.MaxTime:0.00}";
        MaxTimeButtonText.text = $"{MaxTimeCost} Coins";

        BonusTime.text = $"{p.BonusTime:0.00}";
        BonusTimeButtonText.text = $"{BonusTimeCost} Coins";

    }

    public void UpgradeBackflips()
    {
        if (RemainingCoins >= BackflipCost)
        {
            CoinsSpent += BackflipCost;
            PlayerController.Instance.MaxBackflips++;
            PlayerController.Instance.Backflips = PlayerController.Instance.MaxBackflips;
            BackflipCost++;
            DrawText();
        }
    }

    public void UpgradeMaxTime()
    {
        if (RemainingCoins >= MaxTimeCost)
        {
            CoinsSpent += MaxTimeCost;
            PlayerController.Instance.MaxTime += 0.10f;
            MaxTimeCost++;
            DrawText();
        }
    }

    public void UpgradeBonusTime()
    {
        if (RemainingCoins >= BonusTimeCost)
        {
            CoinsSpent += BonusTimeCost;
            PlayerController.Instance.BonusTime += 0.10f;
            BonusTimeCost++;
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
