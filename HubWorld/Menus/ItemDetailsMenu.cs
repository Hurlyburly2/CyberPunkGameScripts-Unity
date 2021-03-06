﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailsMenu : MonoBehaviour
{
    [SerializeField] UpgradesMenu upgradesMenu;
    [SerializeField] ShopMenu shopMenu;

    public enum ItemDetailMenuContextType { Inventory, Loadout, Shop, JobSelect };
    ItemDetailMenuContextType context;
    Item item;

    [SerializeField] CardCarosel runnerCardCarosel;
    [SerializeField] CardCarosel hackerCardCarosel;
    CardCarosel currentCardCarosel;

    // General Context
    [SerializeField] TextMeshProUGUI itemNameField;
    [SerializeField] TextMeshProUGUI itemDescriptionField;
    [SerializeField] TextMeshProUGUI itemLvlField;

    // Runner Context
    [SerializeField] GameObject runnerContextMenu;
    // Runner shop/loadout context
    [SerializeField] GameObject runnerShopLoadoutContext;
    [SerializeField] GameObject runnerShopContext;
    [SerializeField] GameObject runnerLoadoutContext;
    [SerializeField] GameObject runnerShopContextBuyBtn;
    [SerializeField] GameObject runnerShopContextSellBtn;
    [SerializeField] GameObject runnerShopContextUpgradeBtn;
    [SerializeField] TextMeshProUGUI runnerShopPriceAmountField;
    [SerializeField] TextMeshProUGUI runnerShopPriceLabel;
    // Runner inventory context
    [SerializeField] GameObject runnerInventoryContext;

    // Hacker Context
    [SerializeField] GameObject hackerContextMenu;
    // Hacker Mod
    [SerializeField] GameObject hackerModContext;
    [SerializeField] GameObject hackerModInventoryContext;
    [SerializeField] GameObject hackerModLoadoutContext;
    [SerializeField] GameObject hackerModShopContext;
    [SerializeField] TextMeshProUGUI hackerModAbilityDescription;
    [SerializeField] Image hackerModAbilityIcon;
    [SerializeField] TextMeshProUGUI hackerModAbilityUseCount;
    [SerializeField] TextMeshProUGUI slotsField;
    [SerializeField] GameObject hackerModShopContextBuyBtn;
    [SerializeField] GameObject hackerModShopContextSellBtn;
    [SerializeField] GameObject hackerModShopContextUpgradeBtn;
    [SerializeField] TextMeshProUGUI hackerModShopPriceAmountField;
    [SerializeField] TextMeshProUGUI hackerModPriceLabel;
    // Hacker Install
    [SerializeField] GameObject hackerInstallContext;
    [SerializeField] GameObject hackerInstallInventoryContext;
    [SerializeField] GameObject hackerInstallLoadoutContext;
    [SerializeField] GameObject hackerInstallShopContext;
    [SerializeField] TextMeshProUGUI hackerInstallSlotType;
    [SerializeField] TextMeshProUGUI hackerInstallPassiveAbilityDescription;
    [SerializeField] GameObject hackerInstallShopContextBuyBtn;
    [SerializeField] GameObject hackerInstallShopContextSellBtn;
    [SerializeField] GameObject hackerInstallShopContextUpgradeBtn;
    [SerializeField] TextMeshProUGUI hackerInstallShopPriceAmountField;
    [SerializeField] TextMeshProUGUI hackerInstallPriceLabel;

    HubWorldSFX hubWorldSFX;

    private void Awake()
    {
        hubWorldSFX = FindObjectOfType<HubWorldSFX>();
    }

    public void SetupItemDetailMenu(ItemDetailMenuContextType newContext, Item newItem)
    {
        context = newContext;
        item = newItem;
        if (item.GetHackerOrRunner() == Item.HackerRunner.Runner)
        {
            SetupRunnerMenu();
        } else
        {
            SetupHackerMenu();
        }
    }

    private void SetupRunnerMenu()
    {
        hackerContextMenu.SetActive(false);
        runnerContextMenu.SetActive(true);
        currentCardCarosel = runnerCardCarosel;
        SetupGeneralInfo();
        switch(context)
        {
            case ItemDetailMenuContextType.Inventory:
                runnerInventoryContext.SetActive(true);
                runnerShopLoadoutContext.SetActive(false);
                break;
            case ItemDetailMenuContextType.Loadout:
                runnerInventoryContext.SetActive(false);
                runnerShopLoadoutContext.SetActive(true);
                runnerLoadoutContext.SetActive(true);
                runnerShopContext.SetActive(false);
                break;
            case ItemDetailMenuContextType.Shop:
                runnerInventoryContext.SetActive(false);
                runnerShopLoadoutContext.SetActive(true);
                runnerLoadoutContext.SetActive(false);
                runnerShopContext.SetActive(true);
                SetupShopDetails(Item.HackerRunner.Runner);
                break;
        }
        SetupCardCarosel();
    }

    private void SetupHackerMenu()
    {
        hackerContextMenu.SetActive(true);
        runnerContextMenu.SetActive(true);
        currentCardCarosel = hackerCardCarosel;
        SetupGeneralInfo();
        Item.ItemTypes[] modTypes = { Item.ItemTypes.NeuralImplant, Item.ItemTypes.Rig, Item.ItemTypes.Uplink };
        List<Item.ItemTypes> modTypesList = new List<Item.ItemTypes>();
        modTypesList.AddRange(modTypes);
        if (modTypesList.Contains(item.GetItemType()))
        {
            // Is a mod
            hackerModContext.SetActive(true);
            hackerInstallContext.SetActive(false);
            switch (context)
            {
                case ItemDetailMenuContextType.Inventory:
                    hackerModInventoryContext.SetActive(true);
                    hackerModLoadoutContext.SetActive(false);
                    hackerModShopContext.SetActive(false);
                    break;
                case ItemDetailMenuContextType.Loadout:
                    hackerModInventoryContext.SetActive(false);
                    hackerModLoadoutContext.SetActive(true);
                    hackerModShopContext.SetActive(false);
                    break;
                case ItemDetailMenuContextType.Shop:
                    hackerModInventoryContext.SetActive(false);
                    hackerModLoadoutContext.SetActive(false);
                    hackerModShopContext.SetActive(true);
                    SetupShopDetails(Item.HackerRunner.Hacker);
                    break;
            }
            SetupModActiveAbility();
        } else
        {
            // Is an install
            hackerInstallContext.SetActive(true);
            hackerModContext.SetActive(false);
            switch (context)
            {
                case ItemDetailMenuContextType.Inventory:
                    hackerInstallInventoryContext.SetActive(true);
                    hackerInstallLoadoutContext.SetActive(false);
                    hackerInstallShopContext.SetActive(false);
                    break;
                case ItemDetailMenuContextType.Loadout:
                    hackerInstallInventoryContext.SetActive(false);
                    hackerInstallLoadoutContext.SetActive(true);
                    hackerInstallShopContext.SetActive(false);
                    break;
                case ItemDetailMenuContextType.Shop:
                    hackerInstallInventoryContext.SetActive(false);
                    hackerInstallLoadoutContext.SetActive(false);
                    hackerInstallShopContext.SetActive(true);
                    SetupShopDetails(Item.HackerRunner.Hacker, true);
                    break;
            }
            SetupInstallPassiveAbility();
            SetupCardCarosel();
        }
    }

    private void SetupShopDetails(Item.HackerRunner hackerOrRunner, bool isInstall=false)
    {
        switch (shopMenu.GetOpenShopTab())
        {
            case "buy":
                GameObject activeBuyButton = runnerShopContextBuyBtn;
                TextMeshProUGUI currentBuyPriceAmountField = runnerShopPriceAmountField;
                TextMeshProUGUI currentBuyPriceLabel = runnerShopPriceLabel;
                if (hackerOrRunner == Item.HackerRunner.Runner)
                {
                    runnerShopContextBuyBtn.SetActive(true);
                    runnerShopContextSellBtn.SetActive(false);
                    runnerShopContextUpgradeBtn.SetActive(false);
                    runnerShopPriceAmountField.text = item.GetItemPrice().ToString();
                    activeBuyButton = runnerShopContextBuyBtn;
                    currentBuyPriceAmountField = runnerShopPriceAmountField;
                    currentBuyPriceLabel = runnerShopPriceLabel;
                } else if (hackerOrRunner == Item.HackerRunner.Hacker && !isInstall)
                {
                    hackerModShopContextBuyBtn.SetActive(true);
                    hackerModShopContextSellBtn.SetActive(false);
                    hackerModShopContextUpgradeBtn.SetActive(false);
                    hackerModShopPriceAmountField.text = item.GetItemPrice().ToString();
                    activeBuyButton = hackerModShopContextBuyBtn;
                    currentBuyPriceAmountField = hackerModShopPriceAmountField;
                    currentBuyPriceLabel = hackerModPriceLabel;
                } else if (hackerOrRunner == Item.HackerRunner.Hacker && isInstall)
                {
                    hackerInstallShopContextBuyBtn.SetActive(true);
                    hackerInstallShopContextSellBtn.SetActive(false);
                    hackerInstallShopContextUpgradeBtn.SetActive(false);
                    hackerInstallShopPriceAmountField.text = item.GetItemPrice().ToString();
                    activeBuyButton = hackerInstallShopContextBuyBtn;
                    currentBuyPriceAmountField = hackerInstallShopPriceAmountField;
                    currentBuyPriceLabel = hackerInstallPriceLabel;
                }

                PlayerData playerData1 = FindObjectOfType<PlayerData>();
                if (item.GetItemPrice() > playerData1.GetCreditsAmount())
                {
                    // If the item is unaffordable, disable the buy button but show the price fields
                    activeBuyButton.GetComponent<Button>().interactable = false;
                    currentBuyPriceAmountField.gameObject.SetActive(true);
                    currentBuyPriceLabel.gameObject.SetActive(true);
                }
                break;
            case "sell":
                break;
            case "upgrade":
                GameObject activeUpgradeButton = runnerShopContextUpgradeBtn;
                TextMeshProUGUI currentUpgradePriceAmountField = runnerShopPriceAmountField;
                TextMeshProUGUI currentUpgradePriceLabel = runnerShopPriceLabel;
                if (hackerOrRunner == Item.HackerRunner.Runner)
                {
                    runnerShopContextBuyBtn.SetActive(false);
                    runnerShopContextSellBtn.SetActive(false);
                    runnerShopContextUpgradeBtn.SetActive(true);
                    runnerShopPriceAmountField.text = shopMenu.GetPrice(item).ToString();
                    activeUpgradeButton = runnerShopContextUpgradeBtn;
                    currentUpgradePriceAmountField = runnerShopPriceAmountField;
                    currentUpgradePriceLabel = runnerShopPriceLabel;
                } else if (hackerOrRunner == Item.HackerRunner.Hacker && !isInstall)
                {
                    hackerModShopContextBuyBtn.SetActive(false);
                    hackerModShopContextSellBtn.SetActive(false);
                    hackerModShopContextUpgradeBtn.SetActive(true);
                    hackerModShopPriceAmountField.text = shopMenu.GetPrice(item).ToString();
                    activeUpgradeButton = hackerModShopContextUpgradeBtn;
                    currentUpgradePriceAmountField = hackerModShopPriceAmountField;
                    currentUpgradePriceLabel = hackerModPriceLabel;
                } else if (hackerOrRunner == Item.HackerRunner.Hacker && isInstall)
                {
                    hackerInstallShopContextBuyBtn.SetActive(false);
                    hackerInstallShopContextSellBtn.SetActive(false);
                    hackerInstallShopContextUpgradeBtn.SetActive(true);
                    hackerInstallShopPriceAmountField.text = shopMenu.GetPrice(item).ToString();
                    activeUpgradeButton = hackerInstallShopContextUpgradeBtn;
                    currentUpgradePriceAmountField = hackerInstallShopPriceAmountField;
                    currentUpgradePriceLabel = hackerInstallPriceLabel;
                }

                PlayerData playerData = FindObjectOfType<PlayerData>();
                if (item.GetCurrentItemLevel() >= item.GetItemMaxLevel())
                {
                    // If the item is max level: Disable the button and hide the price fields
                    activeUpgradeButton.GetComponent<Button>().interactable = false;
                    currentUpgradePriceAmountField.gameObject.SetActive(false);
                    currentUpgradePriceLabel.gameObject.SetActive(false);
                } else if (shopMenu.GetPrice(item) > playerData.GetCreditsAmount()) {
                    // If the item just is unaffordable, disable the button but display the price
                    activeUpgradeButton.GetComponent<Button>().interactable = false;
                    currentUpgradePriceAmountField.gameObject.SetActive(true);
                    currentUpgradePriceLabel.gameObject.SetActive(true);
                } else
                {
                    // if the item is affordable and not max level, enable the button and show the price
                    activeUpgradeButton.GetComponent<Button>().interactable = true;
                    currentUpgradePriceAmountField.gameObject.SetActive(true);
                    currentUpgradePriceLabel.gameObject.SetActive(true);
                }
                break;
        }
    }

    private void SetupInstallPassiveAbility()
    {
        hackerInstallPassiveAbilityDescription.text = item.GetItemAbilityDescription();
        hackerInstallSlotType.text = item.GetItemType().ToString();
    }

    private void SetupModActiveAbility()
    {
        hackerModAbilityDescription.text = item.GetItemAbilityDescription();
        HackerMod hackerMod = item as HackerMod;

        string slotString = hackerMod.GetMaxSlotCount().ToString();
        switch (hackerMod.GetItemType())
        {
            case Item.ItemTypes.NeuralImplant:
                slotString += " Wetware";
                break;
            case Item.ItemTypes.Rig:
                slotString += " Software";
                break;
            case Item.ItemTypes.Uplink:
                slotString += " Chipsets";
                break;
        }
        slotsField.text = slotString;

        string path = "Icons/ActiveAbilityIcons/Ability" + hackerMod.GetActiveAbilityId().ToString();
        hackerModAbilityIcon.sprite = Resources.Load<Sprite>(path);
        string uses = " use";
        if (hackerMod.GetActiveAbilityUses() > 1)
            uses = " uses";
        hackerModAbilityUseCount.text = hackerMod.GetActiveAbilityUses().ToString() + uses;
    }

    private void SetupCardCarosel()
    {
        currentCardCarosel.ClearCardList();
        currentCardCarosel.InitializeToggle();
        List<int> cardIds = new List<int>();

        if (item.GetHackerOrRunner() == Item.HackerRunner.Runner)
        {
            RunnerMod runnerMod = item as RunnerMod;
            cardIds.AddRange(runnerMod.GetCardIds());
        } else
        {
            HackerModChip hackerInstall = item as HackerModChip;
            cardIds.AddRange(hackerInstall.GetCardIds());
        }
        foreach (int id in cardIds)
        {
            Card card = Resources.Load<Card>("CardPrefabs/Player/Card" + id);
            currentCardCarosel.AddCardToList(card);
        }
        currentCardCarosel.GenerateListItems();
    }

    private void SetupGeneralInfo()
    {
        itemNameField.text = item.GetItemName();
        itemDescriptionField.text = item.GetItemDescription();
        itemLvlField.text = item.GetItemLevel() + "/" + item.GetItemMaxLevel();
    }

    public void CloseItemDetailsMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        runnerShopLoadoutContext.SetActive(false);
        runnerInventoryContext.SetActive(false);

        runnerContextMenu.SetActive(false);
        hackerContextMenu.SetActive(false);

        gameObject.SetActive(false);
    }

    public void OpenUpgradesMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        upgradesMenu.gameObject.SetActive(true);
        upgradesMenu.SetupUpgradesMenu(context, item);
    }

    public void ClickBuyUpgradeButton()
    {
        shopMenu.UpgradeButtonClick();
        SetupItemDetailMenu(context, item);
    }

    public void ClickBuyButton()
    {
        shopMenu.BuyButtonClick();
        CloseItemDetailsMenu();
    }
}
