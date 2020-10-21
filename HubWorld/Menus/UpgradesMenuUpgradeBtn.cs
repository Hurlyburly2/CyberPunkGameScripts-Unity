using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesMenuUpgradeBtn : MonoBehaviour
{
    [SerializeField] UpgradesMenu upgradesMenu;
    [SerializeField] TextMeshProUGUI priceField;

    int price;
    int targetLevel;

    public void SetupButton(int newPrice, int newTargetLevel)
    {
        price = newPrice;
        targetLevel = newTargetLevel;
        priceField.text = price.ToString();
        int playerCredits = FindObjectOfType<PlayerData>().GetCreditsAmount();
        if (playerCredits >= price)
        {
            GetComponentInChildren<Button>().interactable = true;
        } else
        {
            GetComponentInChildren<Button>().interactable = false;
        }
    }

    public void ClickUpgradeButton()
    {
        upgradesMenu.DoUpgrades(price, targetLevel);
    }
}
