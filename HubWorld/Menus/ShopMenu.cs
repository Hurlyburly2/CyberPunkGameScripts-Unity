using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentMoneyField;
    [SerializeField] Image mainBackImage;
    [SerializeField] InventoryList inventoryList;
    [SerializeField] CardCarosel cardCarosel;
    [SerializeField] TextMeshProUGUI abilityTypeText;
    [SerializeField] TextMeshProUGUI abilityDescriptionText;

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

    PlayerData playerData;
    InventoryMenu.InventoryFields[] fields = { InventoryMenu.InventoryFields.Name, InventoryMenu.InventoryFields.Type, InventoryMenu.InventoryFields.Lvl };

    string currentMode = null; // possibilities are buy, sell, and upgrade (taken from consts belows)
    const string BUYMODE = "buy";
    const string SELLMODE = "sell";
    const string UPGRADEMODE = "upgrade";

    // used for making the tab buttons appear active/inactive
    Color inactiveTabColor = new Color(255, 255, 255, 0.5f);
    Color activeTabColor = new Color(255, 255, 255, 1f);

    public void SetupShopMenu()
    {
        SelectNothing();
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

        buyButton.gameObject.SetActive(true);
        sellButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        currentMode = BUYMODE;

        // TODO: CARD CAROSEL AND INVENTORY LIST
    }

    private void InitializeSellScreen()
    {
        if (currentMode == SELLMODE)
            return;
        mainBackImage.sprite = backImageSell;
        buyTabButton.color = inactiveTabColor;
        sellTabButton.color = activeTabColor;
        upgradeTabButton.color = inactiveTabColor;

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
        if (currentMode == UPGRADEMODE)
            return;
        mainBackImage.sprite = backImageUpgrade;
        buyTabButton.color = inactiveTabColor;
        sellTabButton.color = inactiveTabColor;
        upgradeTabButton.color = activeTabColor;

        buyButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(true);
        currentMode = UPGRADEMODE;

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
            SetupCardCarosel(selectedItem);
            priceLabelField.gameObject.SetActive(true);
            priceAmountField.gameObject.SetActive(true);
            priceAmountField.text = GetPrice(selectedItem).ToString();
        } else
        {
            SelectNothing();
        }
        // DO SOMETHING TO ACCOUNT FOR ITEMS WITH CARDS VS ITEMS WITHOUT CARDS
    }

    public int GetPrice(Item item)
    {
        switch (currentMode)
        {
            case BUYMODE:
                // TODO: THIS
                return 9999999;
            case SELLMODE:
                // TODO: THIS
                return 9999999;
            case UPGRADEMODE:
                switch (item.GetItemLevel())
                {
                    case 1:
                        return 1000;
                    case 2:
                        return 2500;
                    case 3:
                        return 5000;
                    case 4:
                        return 100000;
                    default:
                        return 99999999;
                }
            default:
                return 99999999;
        }
    }

    private void SelectNothing()
    {
        cardCarosel.ClearCardList();
        abilityTypeText.text = "";
        abilityDescriptionText.text = "";
        priceAmountField.gameObject.SetActive(false);
        priceLabelField.gameObject.SetActive(false);
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
        Debug.Log("Clicked Buy Button");
    }

    public void SellButtonClick()
    {
        Debug.Log("Clicked Sell Button");
    }

    public void UpgradeButtonClick()
    {
        Debug.Log("Clicked Upgrade Button");
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

    public string GetOpenShopTab()
    {
        return currentMode;
    }
}
