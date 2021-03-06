﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] Button detailsButton;
    [SerializeField] TextMeshProUGUI currentMoneyField;
    [SerializeField] Image mainBackImage;
    [SerializeField] InventoryList inventoryList;
    [SerializeField] CardCarosel cardCarosel;
    [SerializeField] TextMeshProUGUI abilityTypeText;
    [SerializeField] TextMeshProUGUI abilityDescriptionText;
    [SerializeField] TextMeshProUGUI moreInfoLabel;
    [SerializeField] TextMeshProUGUI moreInfoAmount;
    [SerializeField] TextMeshProUGUI abilityUsesField;

    [SerializeField] TextMeshProUGUI priceLabelField;
    [SerializeField] TextMeshProUGUI priceAmountField;

    [SerializeField] Sprite backImageBuy;
    [SerializeField] Image buyTabButton;
    [SerializeField] Sprite backImageSell;
    [SerializeField] Image sellTabButton;
    [SerializeField] Sprite backImageUpgrade;
    [SerializeField] Image upgradeTabButton;

    [SerializeField] Button buyButton;
    [SerializeField] Button sellButton;
    [SerializeField] Button upgradeButton;

    [SerializeField] GameObject nothingForSaleMessage;
    [SerializeField] Sprite[] shopKeepImages;
        // 0 = MECH, 1 = TECH, 2 = CYBER, 3 = BIO
    [SerializeField] Image shopKeepImageHolder;

    PlayerData playerData;
    InventoryMenu.InventoryFields[] fields = { InventoryMenu.InventoryFields.Name, InventoryMenu.InventoryFields.Type, InventoryMenu.InventoryFields.Lvl };

    public enum ShopForSaleType { Mech, Tech, Cyber, Bio };

    string currentMode = null; // possibilities are buy, sell, and upgrade (taken from consts belows)
    const string BUYMODE = "buy";
    const string SELLMODE = "sell";
    const string UPGRADEMODE = "upgrade";

    // used for making the tab buttons appear active/inactive
    Color inactiveTabColor = new Color(255, 255, 255, 0.5f);
    Color activeTabColor = new Color(255, 255, 255, 1f);

    const int MAX_LEVEL_5_UPGRADE_LEVEL_2_PRICE = 1000;
    const int MAX_LEVEL_5_UPGRADE_LEVEL_3_PRICE = 2500;
    const int MAX_LEVEL_5_UPGRADE_LEVEL_4_PRICE = 5000;
    const int MAX_LEVEL_5_UPGRADE_LEVEL_5_PRICE = 10000;

    const int MAX_LEVEL_3_UPGRADE_LEVEL_2_PRICE = 4000;
    const int MAX_LEVEL_3_UPGRADE_LEVEL_3_PRICE = 12000;

    HubWorldSFX hubWorldSFX;

    private void Awake()
    {
        hubWorldSFX = FindObjectOfType<HubWorldSFX>();
    }

    public void SetupShopMenu()
    {
        currentMode = null;
        playerData = FindObjectOfType<PlayerData>();
        SelectNothing();
        SetShopKeepImage();
        InitializeBuyScreen();
        currentMoneyField.text = playerData.GetCreditsAmount().ToString();
    }

    private void InitializeBuyScreen()
    {
        inventoryList.DestroyListItems();
        if (currentMode == BUYMODE)
            return;
        mainBackImage.sprite = backImageBuy;
        buyTabButton.color = activeTabColor;
        sellTabButton.color = inactiveTabColor;
        upgradeTabButton.color = inactiveTabColor;

        buyButton.gameObject.SetActive(true);
        buyButton.interactable = false;
        sellButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        currentMode = BUYMODE;

        // TODO: CARD CAROSEL AND INVENTORY LIST
        if (playerData.GetItemsForSale().Count < 1)
            nothingForSaleMessage.SetActive(true);
        else
            nothingForSaleMessage.SetActive(false);
        SetupInventoryList(playerData.GetItemsForSale());
    }

    private void InitializeSellScreen()
    {
        inventoryList.DestroyListItems();
        if (currentMode == SELLMODE)
            return;
        mainBackImage.sprite = backImageSell;
        buyTabButton.color = inactiveTabColor;
        sellTabButton.color = activeTabColor;
        upgradeTabButton.color = inactiveTabColor;

        nothingForSaleMessage.SetActive(false);
        buyButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(true);
        upgradeButton.gameObject.SetActive(false);
        currentMode = SELLMODE;

        // TODO: CARD CAROSEL AND INVENTORY LIST
        // INVENTORY LIST SHOULD BE POPULATED ONLY BY UNEQUIPPED ITEMS
        // ITEMS MUST BE GIVEN A PRICE VARIABLE THAT INCREASES EACH UPGRADE LEVEL
        // SELLING AN UPGRADED ITEM SHOULD COME AT A LOSS, HOWEVER
    }

    private void InitializeUpgradeScreen()
    {
        inventoryList.DestroyListItems();
        if (currentMode == UPGRADEMODE)
            return;
        mainBackImage.sprite = backImageUpgrade;
        buyTabButton.color = inactiveTabColor;
        sellTabButton.color = inactiveTabColor;
        upgradeTabButton.color = activeTabColor;

        nothingForSaleMessage.SetActive(false);
        buyButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(true);
        currentMode = UPGRADEMODE;

        detailsButton.interactable = false;
        List<Item> items = FindObjectOfType<PlayerData>().GetPlayerItems();
        List<Item> itemsBelowMaxLevel = new List<Item>();
        foreach (Item item in items)
        {
            if (item.GetCurrentItemLevel() < item.GetItemMaxLevel())
                itemsBelowMaxLevel.Add(item);
        }
        SetupInventoryList(itemsBelowMaxLevel);
    }

    private void SetupInventoryList(List<Item> items)
    {
        inventoryList.DestroyListItems();
        inventoryList.SetupInventoryList(fields, items, ItemDetailsMenu.ItemDetailMenuContextType.Shop);
    }

    public void HandleSelectItem(Item selectedItem, bool isHighlighted)
    {
        if (selectedItem != null && isHighlighted)
        {
            detailsButton.interactable = true;
            SetupCardCarosel(selectedItem);
            priceLabelField.gameObject.SetActive(true);
            priceAmountField.gameObject.SetActive(true);
            priceAmountField.text = GetPrice(selectedItem).ToString();
            switch (selectedItem.GetItemType())
            {
                case Item.ItemTypes.Arm:
                case Item.ItemTypes.Exoskeleton:
                case Item.ItemTypes.Head:
                case Item.ItemTypes.Leg:
                case Item.ItemTypes.Torso:
                case Item.ItemTypes.Weapon:
                    moreInfoLabel.gameObject.SetActive(false);
                    moreInfoAmount.gameObject.SetActive(false);
                    abilityUsesField.gameObject.SetActive(false);
                    break;
                case Item.ItemTypes.NeuralImplant:
                    moreInfoLabel.gameObject.SetActive(true);
                    moreInfoLabel.text = "Wetware Slots:";
                    moreInfoAmount.gameObject.SetActive(true);
                    moreInfoAmount.text = (selectedItem as HackerMod).GetCurrentLevelSlotCount().ToString();
                    abilityUsesField.gameObject.SetActive(true);
                    abilityUsesField.text = (selectedItem as HackerMod).GetActiveAbilityUses().ToString() + " Uses";
                    break;
                case Item.ItemTypes.Rig:
                    moreInfoLabel.gameObject.SetActive(true);
                    moreInfoLabel.text = "Software Slots:";
                    moreInfoAmount.gameObject.SetActive(true);
                    moreInfoAmount.text = (selectedItem as HackerMod).GetCurrentLevelSlotCount().ToString();
                    abilityUsesField.text = (selectedItem as HackerMod).GetActiveAbilityUses().ToString() + " Uses";
                    break;
                case Item.ItemTypes.Uplink:
                    moreInfoLabel.gameObject.SetActive(true);
                    moreInfoLabel.text = "Chipset Slots:";
                    moreInfoAmount.gameObject.SetActive(true);
                    moreInfoAmount.text = (selectedItem as HackerMod).GetCurrentLevelSlotCount().ToString();
                    abilityUsesField.text = (selectedItem as HackerMod).GetActiveAbilityUses().ToString() + " Uses";
                    break;
                case Item.ItemTypes.Chipset:
                case Item.ItemTypes.Software:
                case Item.ItemTypes.Wetware:
                    moreInfoLabel.gameObject.SetActive(false);
                    moreInfoAmount.gameObject.SetActive(false);
                    break;
            }
            switch (currentMode)
            {
                case BUYMODE:
                    if (selectedItem.GetItemPrice() <= playerData.GetCreditsAmount())
                        buyButton.interactable = true;
                    else
                        buyButton.interactable = false;
                    break;
                case SELLMODE:
                    // DO SOME THINGS IN HERE
                    break;
                case UPGRADEMODE:
                    if (selectedItem.GetItemLevel() < selectedItem.GetItemMaxLevel() && GetPrice(selectedItem) <= playerData.GetCreditsAmount())
                    {
                        // UPGRADEABLE AND CAN AFFORD
                        upgradeButton.interactable = true;
                    } else if (selectedItem.GetItemLevel() < selectedItem.GetItemMaxLevel())
                    {
                        // UPGRADABLE BUT CANNOT AFFORD
                        upgradeButton.interactable = false;
                    } else
                    {
                        upgradeButton.interactable = false;
                        priceLabelField.gameObject.SetActive(false);
                        priceAmountField.gameObject.SetActive(false);
                    }
                    break;
            }
        } else
        {
            SelectNothing();
        }
    }

    public int GetPrice(Item item)
    {
        switch (currentMode)
        {
            case BUYMODE:
                return item.GetItemPrice();
            case SELLMODE:
                // TODO: THIS
                return 9999999;
            case UPGRADEMODE:
                if (item.GetItemMaxLevel() == 5)
                {
                    switch (item.GetItemLevel())
                    {
                        case 1:
                            return MAX_LEVEL_5_UPGRADE_LEVEL_2_PRICE;
                        case 2:
                            return MAX_LEVEL_5_UPGRADE_LEVEL_3_PRICE;
                        case 3:
                            return MAX_LEVEL_5_UPGRADE_LEVEL_4_PRICE;
                        case 4:
                            return MAX_LEVEL_5_UPGRADE_LEVEL_5_PRICE;
                        default:
                            return 99999999;
                    }
                } else if (item.GetItemMaxLevel() == 3)
                {
                    switch (item.GetItemLevel())
                    {
                        case 1:
                            return MAX_LEVEL_3_UPGRADE_LEVEL_2_PRICE;
                        case 2:
                            return MAX_LEVEL_3_UPGRADE_LEVEL_3_PRICE;
                        default:
                            return 99999999;
                    }
                } else
                {
                    return 99999999;
                }
            default:
                return 99999999;
        }
    }

    public int GetTotalUpgradePrice(int currentItemLevel, int maxLevel, int targetLevel)
    {
        int totalPrice = 0;
        switch (maxLevel)
        {
            case 3:
                int[] prices3 = { MAX_LEVEL_3_UPGRADE_LEVEL_2_PRICE, MAX_LEVEL_3_UPGRADE_LEVEL_3_PRICE };
                for (int i = currentItemLevel - 1; i < targetLevel - 1; i++)
                {
                    totalPrice += prices3[i];
                }
                break;
            case 5:
                int[] prices5 = { MAX_LEVEL_5_UPGRADE_LEVEL_2_PRICE, MAX_LEVEL_5_UPGRADE_LEVEL_3_PRICE, MAX_LEVEL_5_UPGRADE_LEVEL_4_PRICE, MAX_LEVEL_5_UPGRADE_LEVEL_5_PRICE };
                for (int i = currentItemLevel - 1; i < targetLevel - 1; i++)
                {
                    totalPrice += prices5[i];
                }
                break;
        }
        return totalPrice;
    }

    private void SelectNothing()
    {
        detailsButton.interactable = false;
        cardCarosel.ClearCardList();
        abilityTypeText.text = "";
        abilityDescriptionText.text = "";
        priceAmountField.gameObject.SetActive(false);
        priceLabelField.gameObject.SetActive(false);
        upgradeButton.interactable = false;
        moreInfoLabel.gameObject.SetActive(false);
        moreInfoAmount.gameObject.SetActive(false);
        abilityUsesField.gameObject.SetActive(false);
    }

    private void SetupCardCarosel(Item item)
    {
        cardCarosel.ClearCardList();
        List<int> cardIds = new List<int>();

        switch (item.GetItemType())
        {
            case Item.ItemTypes.Arm:
            case Item.ItemTypes.Exoskeleton:
            case Item.ItemTypes.Head:
            case Item.ItemTypes.Leg:
            case Item.ItemTypes.Torso:
            case Item.ItemTypes.Weapon:
                RunnerMod runnerMod = item as RunnerMod;
                cardIds.AddRange(runnerMod.GetCardIds());
                abilityTypeText.text = "";
                abilityDescriptionText.text = "";
                break;
            case Item.ItemTypes.NeuralImplant:
            case Item.ItemTypes.Rig:
            case Item.ItemTypes.Uplink:
                HackerMod hackerMod = item as HackerMod;
                abilityTypeText.text = "Active Ability:";
                abilityDescriptionText.text = hackerMod.GetCurrentLevelAbilityDescription();
                break;
            case Item.ItemTypes.Chipset:
            case Item.ItemTypes.Software:
            case Item.ItemTypes.Wetware:
                HackerModChip hackerModChip = item as HackerModChip;
                cardIds.AddRange(hackerModChip.GetCardIds());
                abilityTypeText.text = "Passive Ability:";
                abilityDescriptionText.text = hackerModChip.GetCurrentLevelAbilityString();
                break;
        }

        foreach (int id in cardIds)
        {
            Card card = Resources.Load<Card>("CardPrefabs/Player/Card" + id);
            cardCarosel.AddCardToList(card);
        }
        cardCarosel.GenerateListItems();
    }

    public void BuyButtonClick()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.BuyItem);
        buyButton.interactable = false;
        BuyItem();
    }

    public void SellButtonClick()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.BuyItem);
        Debug.Log("Clicked Sell Button");
    }

    public void UpgradeButtonClick()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.UpgradeItem);
        Item selectedItem = inventoryList.GetSelectedItem();
        playerData.CreditsSpend(GetPrice(selectedItem));
        selectedItem.UpgradeItem();
        UpdateAfterUpgrade(selectedItem);
    }

    public void UpdateAfterUpgrade(Item selectedItem)
    {
        inventoryList.UpdateListedItem(selectedItem);
        currentMoneyField.text = playerData.GetCreditsAmount().ToString();
    }

    private void SetShopKeepImage()
    {
        int index = 5;
        // 0 = MECH, 1 = TECH, 2 = CYBER, 3 = BIO
        switch (playerData.GetShopType())
        {
            case ShopForSaleType.Mech:
                index = 0;
                break;
            case ShopForSaleType.Tech:
                index = 1;
                break;
            case ShopForSaleType.Cyber:
                index = 2;
                break;
            case ShopForSaleType.Bio:
                index = 3;
                break;
        }
        shopKeepImageHolder.sprite = shopKeepImages[index];
    }

    public void ClickBuyTab()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        InitializeBuyScreen();
    }

    public void ClickSellTab()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        InitializeSellScreen();
    }

    public void ClickUpgradeTab()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        InitializeUpgradeScreen();
    }

    public void CloseShopMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        gameObject.SetActive(false);
    }

    public string GetOpenShopTab()
    {
        return currentMode;
    }

    public void BuyItem()
    {
        Item selectedItem = inventoryList.GetSelectedItem();
        playerData.CreditsSpend(selectedItem.GetItemPrice());
        currentMoneyField.text = playerData.GetCreditsAmount().ToString();
        playerData.GainItem(selectedItem);
        playerData.RemoveItemFromForSale(selectedItem);
        SetupInventoryList(playerData.GetItemsForSale());
        SelectNothing();
    }
}
