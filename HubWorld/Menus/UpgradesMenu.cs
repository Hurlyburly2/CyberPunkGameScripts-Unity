using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradesMenu : MonoBehaviour
{
    ItemDetailsMenu.ItemDetailMenuContextType context;
    Item item;

    // General
    [SerializeField] TextMeshProUGUI itemLvlField;
    CardCaroselMultiple currentCardCarosel;

    // Hacker
    [SerializeField] GameObject hackerContext;
    [SerializeField] GameObject hackerModContext;
    // Hacker Mod
    [SerializeField] Image hackerModChipsetSlots;
    [SerializeField] Image hackerModSoftwareSlots;
    [SerializeField] Image hackerModWetwareSlots;
    [SerializeField] TextMeshProUGUI slotCountText;
    [SerializeField] UpgradeMenuActiveAbilityHolder activeAbilityHolder1;
    [SerializeField] UpgradeMenuActiveAbilityHolder activeAbilityHolder2;
    [SerializeField] UpgradeMenuActiveAbilityHolder activeAbilityHolder3;
    [SerializeField] UpgradeMenuActiveAbilityHolder activeAbilityHolder4;
    [SerializeField] UpgradeMenuActiveAbilityHolder activeAbilityHolder5;
    [SerializeField] List<UpgradesMenuUpgradeBtn> hackerModUpgradeButtons;
    // Hacker Install
    [SerializeField] GameObject hackerInstallContext;
    [SerializeField] CardCaroselMultiple hackerInstallCardCarosel;
    [SerializeField] GameObject hackerLevel1Marker;
    [SerializeField] Image hackerLevel1MarkerSelected;
    [SerializeField] GameObject hackerLevel2Marker;
    [SerializeField] Image hackerLevel2MarkerSelected;
    [SerializeField] GameObject hackerLevel3Marker;
    [SerializeField] Image hackerLevel3MarkerSelected;
    [SerializeField] TextMeshProUGUI hackerPassiveAbilityText;
    [SerializeField] GameObject hackerModShopContext;
    [SerializeField] GameObject hackerInstallShopContext;
    [SerializeField] List<UpgradesMenuUpgradeBtn> hackerInstallUpgradeButtons;

    // Runner
    [SerializeField] GameObject runnerContext;
    [SerializeField] GameObject runnerShopInventoryContext;
    [SerializeField] GameObject runnerUpgradeContext;
    [SerializeField] CardCaroselMultiple runnerCardCarosel;
    [SerializeField] GameObject runnerLevel1Marker;
    [SerializeField] Image runnerLevel1MarkerSelected;
    [SerializeField] GameObject runnerLevel2Marker;
    [SerializeField] Image runnerLevel2MarkerSelected;
    [SerializeField] GameObject runnerLevel3Marker;
    [SerializeField] Image runnerLevel3MarkerSelected;
    [SerializeField] GameObject runnerLevel4Marker;
    [SerializeField] Image runnerLevel4MarkerSelected;
    [SerializeField] GameObject runnerLevel5Marker;
    [SerializeField] Image runnerLevel5MarkerSelected;
    [SerializeField] List<UpgradesMenuUpgradeBtn> runnerUpgradeButtons;

    public void SetupUpgradesMenu(ItemDetailsMenu.ItemDetailMenuContextType newContext, Item newItem)
    {
        context = newContext;
        item = newItem;
        if (item.GetHackerOrRunner() == Item.HackerRunner.Runner)
        {
            SetupRunnerMenu();
        }
        else
        {
            SetupHackerMenu();
        }
    }

    private void SetupRunnerMenu()
    {
        runnerContext.SetActive(true);
        hackerContext.SetActive(false);
        currentCardCarosel = runnerCardCarosel;
        switch (context)
        {
            case ItemDetailsMenu.ItemDetailMenuContextType.Inventory:
                runnerShopInventoryContext.SetActive(true);
                runnerUpgradeContext.SetActive(false);
                break;
            case ItemDetailsMenu.ItemDetailMenuContextType.Loadout:
                runnerShopInventoryContext.SetActive(true);
                runnerUpgradeContext.SetActive(false);
                break;
            case ItemDetailsMenu.ItemDetailMenuContextType.Shop:
                runnerUpgradeContext.SetActive(true);
                runnerShopInventoryContext.SetActive(false);
                SetupUpgradeButtons(runnerUpgradeButtons);
                break;
        }
        SetupGeneralInfo();
        SetupLevelMarkers();
        SetupCardCarosels();
    }

    private void SetupUpgradeButtons(List<UpgradesMenuUpgradeBtn> currentUpgradeButtons)
    {
        for (int i = 0; i < currentUpgradeButtons.Count; i++)
        {
            if (item.GetCurrentItemLevel() < i + 2)
            {
                currentUpgradeButtons[i].gameObject.SetActive(true);
            } else
            {
                currentUpgradeButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetupCardCarosels()
    {
        currentCardCarosel.ClearCardLists();
        currentCardCarosel.InitializeToggle();
        currentCardCarosel.SetHackerOrRunner(item.GetHackerOrRunner());

        if (item.GetHackerOrRunner() == Item.HackerRunner.Runner)
        {
            RunnerMod runnerMod = item as RunnerMod;
            AddCardIdsToCaroselHolder(runnerMod.GetLevelOneCardIds(), 1);
            AddCardIdsToCaroselHolder(runnerMod.GetLevelTwoCardIds(), 2);
            AddCardIdsToCaroselHolder(runnerMod.GetLevelThreeCardIds(), 3);
            AddCardIdsToCaroselHolder(runnerMod.GetLevelFourCardIds(), 4);
            AddCardIdsToCaroselHolder(runnerMod.GetLevelFiveCardIds(), 5);
        }
        else
        {
            //HackerModChip hackerInstall = item as HackerModChip;
            //lvl1CardIds.AddRange(hackerInstall.GetCardIds());
            HackerModChip hackerInstall = item as HackerModChip;
            AddCardIdsToCaroselHolder(hackerInstall.GetLevelOneCardIds(), 1);
            AddCardIdsToCaroselHolder(hackerInstall.GetLevelTwoCardIds(), 2);
            AddCardIdsToCaroselHolder(hackerInstall.GetLevelThreeCardIds(), 3);
        }
        currentCardCarosel.GenerateListItems(item.GetCurrentItemLevel());
    }

    private void SetupLevelMarkers()
    {
        List<GameObject> levelMarkers = GetActiveLevelMarkers();
        int counter = 1;
        foreach (GameObject marker in levelMarkers)
        {
            marker.SetActive(true);
            if (counter == item.GetCurrentItemLevel())
            {
                ActiveOrDeactivateLevelMarker(counter, true);
            } else
            {
                ActiveOrDeactivateLevelMarker(counter, false);
            }
            counter++;
        }
        if (counter <= 5)
        {
            for (int i = counter; i < 5; i++)
            {
                ActiveOrDeactivateLevelMarker(i, false);
            }
        }
    }

    private void ActiveOrDeactivateLevelMarker(int count, bool activate)
    {
        switch (count)
        {
            case 1:
                if (item.GetHackerOrRunner() == Item.HackerRunner.Runner)
                    runnerLevel1MarkerSelected.gameObject.SetActive(activate);
                else
                    hackerLevel1MarkerSelected.gameObject.SetActive(activate);
                break;
            case 2:
                if (item.GetHackerOrRunner() == Item.HackerRunner.Runner)
                    runnerLevel2MarkerSelected.gameObject.SetActive(activate);
                else
                    hackerLevel2MarkerSelected.gameObject.SetActive(activate);
                break;
            case 3:
                if (item.GetHackerOrRunner() == Item.HackerRunner.Runner)
                    runnerLevel3MarkerSelected.gameObject.SetActive(activate);
                else
                    hackerLevel3MarkerSelected.gameObject.SetActive(activate);
                break;
            case 4:
                if (item.GetHackerOrRunner() == Item.HackerRunner.Runner)
                    runnerLevel4MarkerSelected.gameObject.SetActive(activate);
                break;
            case 5:
                if (item.GetHackerOrRunner() == Item.HackerRunner.Runner)
                    runnerLevel5MarkerSelected.gameObject.SetActive(activate);
                break;
        }
    }

    private List<GameObject> GetActiveLevelMarkers()
    {
        List<GameObject> levelMarkers = new List<GameObject>();
        switch (item.GetHackerOrRunner())
        {
            case Item.HackerRunner.Runner:
                GameObject[] findMarkers = { runnerLevel1Marker, runnerLevel2Marker, runnerLevel3Marker, runnerLevel4Marker, runnerLevel5Marker };
                levelMarkers.AddRange(findMarkers);
                break;
            case Item.HackerRunner.Hacker:
                // TODO: FILL IN THIS NONSENSE
                GameObject[] findMarkers2 = { hackerLevel1Marker, hackerLevel2Marker, hackerLevel3Marker };
                levelMarkers.AddRange(findMarkers2);
                break;
        }

        List<GameObject> activeLevelMarkers = new List<GameObject>();
        if (activeLevelMarkers.Count > 0)
        {
            for (int i = 0; i < item.GetItemMaxLevel(); i++)
            {
                Debug.Log(i);
                activeLevelMarkers.Add(levelMarkers[i]);
            }
        }
        return levelMarkers;
    }

    private void AddCardIdsToCaroselHolder(List<int> cardIds, int whichHolder)
    {
        foreach (int id in cardIds)
        {
            Card card = Resources.Load<Card>("CardPrefabs/Player/Card" + id);
            currentCardCarosel.AddCardToList(card, whichHolder);
        }
    }

    private void SetupHackerMenu()
    {
        runnerContext.SetActive(false);
        hackerContext.SetActive(true);
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
                case ItemDetailsMenu.ItemDetailMenuContextType.Inventory:
                    hackerModShopContext.SetActive(false);
                    break;
                case ItemDetailsMenu.ItemDetailMenuContextType.Loadout:
                    hackerModShopContext.SetActive(false);
                    break;
                case ItemDetailsMenu.ItemDetailMenuContextType.Shop:
                    hackerModShopContext.SetActive(true);
                    SetupUpgradeButtons(hackerModUpgradeButtons);
                    break;
            }
            switch (item.GetItemType())
            {
                case Item.ItemTypes.NeuralImplant:
                    hackerModChipsetSlots.gameObject.SetActive(false);
                    hackerModSoftwareSlots.gameObject.SetActive(false);
                    hackerModWetwareSlots.gameObject.SetActive(true);
                    break;
                case Item.ItemTypes.Rig:
                    hackerModChipsetSlots.gameObject.SetActive(false);
                    hackerModSoftwareSlots.gameObject.SetActive(true);
                    hackerModWetwareSlots.gameObject.SetActive(false);
                    break;
                case Item.ItemTypes.Uplink:
                    hackerModChipsetSlots.gameObject.SetActive(true);
                    hackerModSoftwareSlots.gameObject.SetActive(false);
                    hackerModWetwareSlots.gameObject.SetActive(false);
                    break;
            }
            slotCountText.text = GenerateSlotDisplayString();
            SetupActiveAbilityHolders();
        } else
        {
            currentCardCarosel = hackerInstallCardCarosel;
            switch (context)
            {
                case ItemDetailsMenu.ItemDetailMenuContextType.Inventory:
                    hackerInstallShopContext.SetActive(false);
                    break;
                case ItemDetailsMenu.ItemDetailMenuContextType.Loadout:
                    hackerInstallShopContext.SetActive(false);
                    break;
                case ItemDetailsMenu.ItemDetailMenuContextType.Shop:
                    hackerInstallShopContext.SetActive(true);
                    SetupUpgradeButtons(hackerInstallUpgradeButtons);
                    break;
            }
            // Is an install
            hackerInstallContext.SetActive(true);
            hackerModContext.SetActive(false);
            SetupCardCarosels();
            SetupLevelMarkers();
            HackerModChip chip = item as HackerModChip;
            hackerPassiveAbilityText.text = chip.GetPassiveAbilityStringByLevel(item.GetItemLevel());
        }
    }

    private void SetupActiveAbilityHolders()
    {
        UpgradeMenuActiveAbilityHolder[] abilities = { activeAbilityHolder1, activeAbilityHolder2, activeAbilityHolder3, activeAbilityHolder4, activeAbilityHolder5 };
        List<UpgradeMenuActiveAbilityHolder> abilityHolders = new List<UpgradeMenuActiveAbilityHolder>();
        abilityHolders.AddRange(abilities);

        int counter = 0;
        HackerMod hackerMod = item as HackerMod;
        foreach (UpgradeMenuActiveAbilityHolder abilityHolder in abilityHolders)
        {
            counter++; // start off with #1
            string abilityDescription = "";
            int abilityUses = 0;

            bool isCurrentLevel = false;
            if (counter == hackerMod.GetCurrentItemLevel())
                isCurrentLevel = true;

            switch (counter)
            {
                case 1:
                    abilityDescription = hackerMod.GetLevelOneItemAbilityDescription();
                    abilityUses = hackerMod.GetLevel1AbilityUses();
                    break;
                case 2:
                    abilityDescription = hackerMod.GetLevelTwoItemAbilityDescription();
                    abilityUses = hackerMod.GetLevel2AbilityUses();
                    break;
                case 3:
                    abilityDescription = hackerMod.GetLevelThreeItemAbilityDescription();
                    abilityUses = hackerMod.GetLevel3AbilityUses();
                    break;
                case 4:
                    abilityDescription = hackerMod.GetLevelFourItemAbilityDescription();
                    abilityUses = hackerMod.GetLevel4AbilityUses();
                    break;
                case 5:
                    abilityDescription = hackerMod.GetLevelFiveItemAbilityDescription();
                    abilityUses = hackerMod.GetLevel5AbilityUses();
                    break;
            }

            string path = "Icons/ActiveAbilityIcons/Ability" + hackerMod.GetActiveAbilityId().ToString();
            Sprite hackerAbilityIcon = Resources.Load<Sprite>(path);

            abilityHolder.SetupAbilityHolder(hackerAbilityIcon, item.GetCurrentItemLevel(), counter, isCurrentLevel, abilityDescription, abilityUses);
        }
    }

    public string GenerateSlotDisplayString()
    {
        string displayString = "";
        HackerMod mod = item as HackerMod;

        if (item.GetItemLevel() == 1)
            displayString += "<color=#FFFFFF>" + mod.GetLevel1SlotCount() + "</color>";
        else
            displayString += mod.GetLevel1SlotCount();

        if (item.GetItemLevel() == 2)
            displayString += "   <color=#FFFFFF>" + mod.GetLevel2SlotCount() + "</color>";
        else
            displayString += "   " + mod.GetLevel2SlotCount();

        if (item.GetItemLevel() == 3)
            displayString += "   <color=#FFFFFF>" + mod.GetLevel3SlotCount() + "</color>";
        else
            displayString += "   " + mod.GetLevel3SlotCount();

        if (item.GetItemLevel() == 4)
            displayString += "   <color=#FFFFFF>" + mod.GetLevel4SlotCount() + "</color>";
        else
            displayString += "   " + mod.GetLevel4SlotCount();

        if (item.GetItemLevel() == 5)
            displayString += "   <color=#FFFFFF>" + mod.GetLevel5SlotCount() + "</color>";
        else
            displayString += "   " + mod.GetLevel5SlotCount();

        return displayString;
    }

    private void SetupGeneralInfo()
    {
        itemLvlField.text = item.GetItemLevel() + "/" + item.GetItemMaxLevel();
    }

    public void CloseUpgradesMenu()
    {
        // Turn off all level markers
        List<GameObject> turnOffMarkers = GetActiveLevelMarkers();
        foreach (GameObject marker in turnOffMarkers)
        {
            marker.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
