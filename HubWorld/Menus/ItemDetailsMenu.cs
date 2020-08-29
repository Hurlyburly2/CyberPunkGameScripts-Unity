using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailsMenu : MonoBehaviour
{
    public enum ItemDetailMenuContextType { Inventory, Loadout, Shop };
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
    // Hacker Install
    [SerializeField] GameObject hackerInstallContext;
    [SerializeField] GameObject hackerInstallInventoryContext;
    [SerializeField] GameObject hackerInstallLoadoutContext;
    [SerializeField] GameObject hackerInstallShopContext;
    [SerializeField] TextMeshProUGUI hackerInstallSlotType;
    [SerializeField] TextMeshProUGUI hackerInstallPassiveAbilityDescription;
        
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
                break;
            case ItemDetailMenuContextType.Loadout:
                runnerShopLoadoutContext.SetActive(true);
                break;
            case ItemDetailMenuContextType.Shop:
                runnerShopLoadoutContext.SetActive(true);
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
                    break;
                case ItemDetailMenuContextType.Shop:
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
                    break;
                case ItemDetailMenuContextType.Shop:
                    break;
            }
            SetupInstallPassiveAbility();
            SetupCardCarosel();
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
        runnerShopLoadoutContext.SetActive(false);
        runnerInventoryContext.SetActive(false);

        runnerContextMenu.SetActive(false);
        hackerContextMenu.SetActive(false);

        gameObject.SetActive(false);
    }
}
