using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentMoneyField;
    [SerializeField] Image mainBackImage;

    [SerializeField] Sprite backImageBuy;
    [SerializeField] Image buyTabButton;
    [SerializeField] Sprite backImageSell;
    [SerializeField] Image sellTabButton;
    [SerializeField] Sprite backImageUpgrade;
    [SerializeField] Image upgradeTabButton;

    PlayerData playerData;

    string currentMode = null; // possibilities are buy, sell, and upgrade (taken from consts belows)
    const string BUYMODE = "buy";
    const string SELLMODE = "sell";
    const string UPGRADEMODE = "upgrade";

    // used for making the tab buttons appear active/inactive
    Color inactiveTabColor = new Color(255, 255, 255, 0.5f);
    Color activeTabColor = new Color(255, 255, 255, 1f);

    public void SetupShopMenu()
    {
        InitializeBuyScreen();
        playerData = FindObjectOfType<PlayerData>();
        currentMoneyField.text = playerData.GetCreditsAmount().ToString();
    }

    private void InitializeBuyScreen()
    {
        if (currentMode == BUYMODE)
            return;
        mainBackImage.sprite = backImageBuy;
        buyTabButton.color = activeTabColor;
        sellTabButton.color = inactiveTabColor;
        upgradeTabButton.color = inactiveTabColor;
        currentMode = BUYMODE;
    }

    private void InitializeSellScreen()
    {
        if (currentMode == SELLMODE)
            return;
        mainBackImage.sprite = backImageSell;
        buyTabButton.color = inactiveTabColor;
        sellTabButton.color = activeTabColor;
        upgradeTabButton.color = inactiveTabColor;
        currentMode = SELLMODE;
    }

    private void InitializeUpgradeScreen()
    {
        if (currentMode == UPGRADEMODE)
            return;
        mainBackImage.sprite = backImageUpgrade;
        buyTabButton.color = inactiveTabColor;
        sellTabButton.color = inactiveTabColor;
        upgradeTabButton.color = activeTabColor;
        currentMode = UPGRADEMODE;
    }

    public void ClickBuyTab()
    {
        InitializeBuyScreen();
    }

    public void ClickSellTab()
    {
        InitializeSellScreen();
    }

    public void ClickUpgradeTab()
    {
        InitializeUpgradeScreen();
    }

    public void CloseShopMenu()
    {
        gameObject.SetActive(false);
    }
}
