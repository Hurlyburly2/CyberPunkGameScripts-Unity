using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadoutEquipmentMenu : MonoBehaviour
{
    // Runner stuff
    [SerializeField] GameObject runnerContext;
    [SerializeField] List<LoadoutSlotBtn> runnerLoadoutSlotBtns;
    [SerializeField] TextMeshProUGUI runnerName;
    [SerializeField] TextMeshProUGUI runnerDescription;
    [SerializeField] InventoryList runnerInventoryList;
    [SerializeField] CardCarosel runnerCardCarosel;

    // Hacker Stuff
    [SerializeField] GameObject hackerContext;

    Item.HackerRunner hackerOrRunner;

    CharacterData runner;
    HackerData hacker;
    CardCarosel currentCardCarosel;
    InventoryMenu.InventoryFields[] fields = { InventoryMenu.InventoryFields.Name, InventoryMenu.InventoryFields.Type, InventoryMenu.InventoryFields.Lvl };
    List<Item.ItemTypes> currentFilters;

    Item selectedItem;

    private void DoSetup()
    {
        InitialFilterSetup();
        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                hackerContext.SetActive(false);
                runnerContext.SetActive(true);
                runnerName.text = runner.GetRunnerName();
                runnerDescription.text = runner.GetBio();
                currentCardCarosel = runnerCardCarosel;
                break;
            case Item.HackerRunner.Hacker:
                runnerContext.SetActive(false);
                hackerContext.SetActive(true);
                // setup hacker name
                // setup hacker description (if necessary)
                // setup hacker card carosel (if applicable)
                break;
        }
        foreach (LoadoutSlotBtn button in runnerLoadoutSlotBtns)
        {
            button.SetupButton();
        }
        currentCardCarosel.InitializeToggle();
        SetupInventoryList();
    }

    public void HandleSelectedItem(Item newItem, bool isSelected)
    {
        if (isSelected)
        {
            selectedItem = newItem;
        } else
        {
            selectedItem = null;
        }
        if (selectedItem != null)
        {
            SetupCardCarosel(selectedItem);
        }
    }

    private void SetupCardCarosel(Item item)
    {
        currentCardCarosel.ClearCardList();
        List<int> cardIds = new List<int>();

        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                RunnerMod runnerMod = item as RunnerMod;
                cardIds.AddRange(runnerMod.GetCardIds());
                break;
            case Item.HackerRunner.Hacker:
                // Fill this only if the appropriate type of hacker item is selected
                break;
        }

        foreach (int id in cardIds)
        {
            Card card = Resources.Load<Card>("CardPrefabs/Player/Card" + id);
            currentCardCarosel.AddCardToList(card);
        }
        currentCardCarosel.GenerateListItems();
    }

    private void SetupInventoryList()
    {
        runnerInventoryList.DestroyListItems();

        List<Item> items = FindObjectOfType<PlayerData>().GetPlayerItems();
        List<Item> filteredItems = new List<Item>();

        foreach (Item item in items)
        {
            if (currentFilters.Contains(item.GetItemType()))
            {
                filteredItems.Add(item);
            }
        }

        runnerInventoryList.SetupInventoryList(fields, filteredItems, ItemDetailsMenu.ItemDetailMenuContextType.Loadout);

        SelectEquippedItemInList();
    }

    private void SelectEquippedItemInList()
    {
        // To highlight currently equipped item, first we see if there is only one filter...
        if (currentFilters.Count == 1)
        {
            PlayerData playerData = FindObjectOfType<PlayerData>();
            switch (hackerOrRunner)
            {
                // Then we find if there are any active buttons and get the leftOrRight and itemType from them
                case Item.HackerRunner.Runner:
                    Loadout.LeftOrRight leftOrRight;
                    Item.ItemTypes itemType;
                    foreach (LoadoutSlotBtn button in runnerLoadoutSlotBtns)
                    {
                        if (button.GetIsActive())
                        {
                            itemType = button.GetItemType();
                            leftOrRight = button.GetLeftOrRight();
                            RunnerMod equippedMod = playerData.GetCurrentRunner().GetLoadout().GetEquippedModByItemType(itemType, leftOrRight);
                            runnerInventoryList.SelectParticularItem(equippedMod);
                        }
                    }
                    break;
                case Item.HackerRunner.Hacker:
                    break;
            }
        }
    }

    private void InitialFilterSetup()
    {
        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                Item.ItemTypes[] runnerResetFilters = {
                    Item.ItemTypes.Arm,
                    Item.ItemTypes.Exoskeleton,
                    Item.ItemTypes.Head,
                    Item.ItemTypes.Leg,
                    Item.ItemTypes.Torso,
                    Item.ItemTypes.Weapon
                };
                currentFilters = new List<Item.ItemTypes>();
                currentFilters.AddRange(runnerResetFilters);
                break;
            case Item.HackerRunner.Hacker:
                Item.ItemTypes[] hackerResetFilters = {
                    Item.ItemTypes.Chipset,
                    Item.ItemTypes.NeuralImplant,
                    Item.ItemTypes.Rig,
                    Item.ItemTypes.Software,
                    Item.ItemTypes.Uplink,
                    Item.ItemTypes.Wetware
                };
                currentFilters = new List<Item.ItemTypes>();
                currentFilters.AddRange(hackerResetFilters);
                // Get all hackermods and installs, think about it later
                break;
        }
    }

    public void HandlePressedSlotButton(Item.ItemTypes itemTypeOnButton, Loadout.LeftOrRight leftOrRight)
    {
        foreach (LoadoutSlotBtn button in runnerLoadoutSlotBtns)
        {
            if (itemTypeOnButton != button.GetItemType() || leftOrRight != button.GetLeftOrRight())
            {
                button.SetInactive();
            }
        }
        if (currentFilters.Count != 1 || currentFilters.Count == 1 && currentFilters[0] != itemTypeOnButton)
        {
            UpdateFilters(itemTypeOnButton, leftOrRight);
            SetupInventoryList();
        } else
        {
            SelectEquippedItemInList();
        }
    }

    private void UpdateFilters(Item.ItemTypes clickedBtnType, Loadout.LeftOrRight leftOrRight)
    {
        bool foundActiveBtn = false;
        foreach (LoadoutSlotBtn button in runnerLoadoutSlotBtns)
        {
            if (button.GetIsActive())
                foundActiveBtn = true;
        }
        if (foundActiveBtn)
        {
            currentFilters = new List<Item.ItemTypes>();
            currentFilters.Add(clickedBtnType);
        } else
        {
            InitialFilterSetup();
        }
    }

    public void SetupLoadoutEquipmentMenu(CharacterData newRunner)
    {
        runner = newRunner;
        hackerOrRunner = Item.HackerRunner.Runner;
        DoSetup();
    }

    public void SetupLoadoutEquipmentMenu(HackerData newHacker)
    {
        hacker = newHacker;
        hackerOrRunner = Item.HackerRunner.Hacker;
        DoSetup();
    }

    public void CloseLoadoutEquipmentMenu()
    {
        gameObject.SetActive(false);
    }
}
