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
    [SerializeField] Image hackerModChipsetSlots;
    [SerializeField] Image hackerModSoftwareSlots;
    [SerializeField] Image hackerModWetwareSlots;
    [SerializeField] TextMeshProUGUI slotCountText;
    [SerializeField] GameObject hackerInstallContext;

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
                break;
        }
        SetupGeneralInfo();
        SetupLevelMarkers();
        SetupCardCarosels();
    }

    private void SetupCardCarosels()
    {
        currentCardCarosel.ClearCardLists();
        currentCardCarosel.InitializeToggle();

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
        }
        currentCardCarosel.GenerateListItems();
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
                runnerLevel1MarkerSelected.gameObject.SetActive(activate);
                break;
            case 2:
                runnerLevel2MarkerSelected.gameObject.SetActive(activate);
                break;
            case 3:
                runnerLevel3MarkerSelected.gameObject.SetActive(activate);
                break;
            case 4:
                runnerLevel4MarkerSelected.gameObject.SetActive(activate);
                break;
            case 5:
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
        } else
        {
            // Is an install
            hackerInstallContext.SetActive(true);
            hackerModContext.SetActive(false);
            // TODO THIS STUFF
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
