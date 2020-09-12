using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadoutEquipmentMenu : MonoBehaviour
{
    [Header("Runner Stuff")]
    [SerializeField] GameObject runnerContext;
    [SerializeField] List<LoadoutSlotBtn> runnerLoadoutSlotBtns;
    [SerializeField] TextMeshProUGUI runnerName;
    [SerializeField] TextMeshProUGUI runnerDescription;
    [SerializeField] InventoryList runnerInventoryList;
    [SerializeField] CardCarosel runnerCardCarosel;

    [Header("Hacker Stuff")]
    [SerializeField] GameObject hackerContext;
    [SerializeField] TextMeshProUGUI hackerName;
    [SerializeField] TextMeshProUGUI hackerDescription;
    [SerializeField] Button hackerEquipButton;
    [SerializeField] CardCarosel hackerCardCarosel;
    [SerializeField] InventoryList hackerInventoryList;
    List<LoadoutSlotBtn> activeHackerSlotBtns;
    [SerializeField] GameObject wetware1SlotHolder;
    [SerializeField] List<LoadoutSlotBtn> wetware1Slots;
    [SerializeField] GameObject wetware2SlotHolder;
    [SerializeField] List<LoadoutSlotBtn> wetware2Slots;
    [SerializeField] GameObject wetware3SlotHolder;
    [SerializeField] List<LoadoutSlotBtn> wetware3Slots;
    [SerializeField] GameObject software1SlotHolder;
    [SerializeField] List<LoadoutSlotBtn> software1Slots;
    [SerializeField] GameObject software2SlotHolder;
    [SerializeField] List<LoadoutSlotBtn> software2Slots;
    [SerializeField] GameObject software3SlotHolder;
    [SerializeField] List<LoadoutSlotBtn> software3Slots;
    [SerializeField] GameObject chipset1SlotHolder;
    [SerializeField] List<LoadoutSlotBtn> chipset1Slots;
    [SerializeField] GameObject chipset2SlotHolder;
    [SerializeField] List<LoadoutSlotBtn> chipset2Slots;
    [SerializeField] GameObject chipset3SlotHolder;
    [SerializeField] List<LoadoutSlotBtn> chipset3Slots;
    [SerializeField] List<LoadoutSlotBtn> hackerModSlots;

    [Header("General Stuff")]
    Item.HackerRunner hackerOrRunner;
    [SerializeField] Button runnerEquipButton;

    CharacterData runner;
    HackerData hacker;
    CardCarosel currentCardCarosel;
    InventoryList currentInventoryList;
    InventoryMenu.InventoryFields[] fields = { InventoryMenu.InventoryFields.Name, InventoryMenu.InventoryFields.Type, InventoryMenu.InventoryFields.Lvl };
    List<Item.ItemTypes> currentFilters;

    Item selectedItem;
    bool recentlyEquippedItem = false;
    bool waitingToEquip = false;

    private void DoSetup()
    {
        InitialFilterSetup();
        runnerEquipButton.interactable = false;
        hackerEquipButton.interactable = false;
        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                hackerContext.SetActive(false);
                runnerContext.SetActive(true);
                runnerName.text = runner.GetRunnerName();
                runnerDescription.text = runner.GetBio();
                currentCardCarosel = runnerCardCarosel;
                currentInventoryList = runnerInventoryList;
                foreach (LoadoutSlotBtn button in runnerLoadoutSlotBtns)
                {
                    button.SetupButton();
                }
                break;
            case Item.HackerRunner.Hacker:
                runnerContext.SetActive(false);
                hackerContext.SetActive(true);
                hackerName.text = hacker.GetName();
                hackerDescription.text = hacker.GetBio();
                currentCardCarosel = hackerCardCarosel;
                SetupActiveHackerSlots();
                currentInventoryList = hackerInventoryList;
                break;
        }
        
        currentCardCarosel.InitializeToggle();
        SetupInventoryList();
    }

    private void ClearWaitingForInputOnButtons()
    {
        //List<LoadoutSlotBtn> currentButtons = new List<LoadoutSlotBtn>();
        //if (hackerOrRunner == Item.HackerRunner.Runner)
        //    currentButtons = runnerLoadoutSlotBtns;
        //else
        //    currentButtons = activeHackerSlotBtns;

        foreach (LoadoutSlotBtn button in runnerLoadoutSlotBtns)
        {
            button.ClearWaitingForInput();
        }
        waitingToEquip = false;
    }

    public void EquipItem()
    {
        ClearWaitingForInputOnButtons();
        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                Loadout runnerLoadout = FindObjectOfType<PlayerData>().GetCurrentRunner().GetLoadout();

                if (selectedItem.GetItemType() == Item.ItemTypes.Arm || selectedItem.GetItemType() == Item.ItemTypes.Leg)
                {
                    // Do we have an arm or leg slot selected? If so, equip to that one
                    foreach (LoadoutSlotBtn loadoutSlot in runnerLoadoutSlotBtns)
                    {
                        if (loadoutSlot.GetItemType() == selectedItem.GetItemType() && loadoutSlot.GetIsActive())
                        {
                            runnerLoadout.EquipItem(selectedItem as RunnerMod, loadoutSlot.GetLeftOrRight());
                            Debug.Log("Equip arm item...?");
                            recentlyEquippedItem = true;
                        }
                    }
                    if (recentlyEquippedItem == false)
                    {
                        // get and activate the appropriate buttons
                        foreach (LoadoutSlotBtn loadoutSlot in runnerLoadoutSlotBtns)
                        {
                            if (loadoutSlot.GetItemType() == selectedItem.GetItemType())
                            {
                                loadoutSlot.SetButtonToAskForInput();
                            }
                        }
                        waitingToEquip = true;
                        // TODO: If not disable other inputs, play an animation indicating one or the other should be clicked
                    }
                } else
                {
                    runnerLoadout.EquipItem(selectedItem as RunnerMod);
                    recentlyEquippedItem = true;
                }
                break;
            case Item.HackerRunner.Hacker:
                HackerLoadout hackerLoadout = FindObjectOfType<PlayerData>().GetCurrentHacker().GetHackerLoadout();

                if (selectedItem.IsHackerMod())
                {
                    hackerLoadout.EquipItem(selectedItem as HackerMod);
                    recentlyEquippedItem = true;
                    SetupActiveHackerSlots();
                } else if (selectedItem.IsHackerChipset())
                {
                    // TODO: equip a chipset (do the same thing for left/right arms up above ughhh
                    foreach (LoadoutSlotBtn loadoutSlot in activeHackerSlotBtns)
                    {
                        if (loadoutSlot.GetItemType() == selectedItem.GetItemType() && loadoutSlot.GetIsActive())
                        {
                            hackerLoadout.EquipItem(selectedItem as HackerModChip, loadoutSlot.GetSlotNumber() - 1);
                        }
                    }
                    if (recentlyEquippedItem == false)
                    {
                        // TODO: IF NOT DISABLE OTHER INPUTS PLAY AN ANIMATION INDICATING A SLOT SHOULD BE CLICKED
                    }
                }
                break;
        }
        SetupInventoryList();
    }

    private void SetupActiveHackerSlots()
    {
        activeHackerSlotBtns = new List<LoadoutSlotBtn>();
        activeHackerSlotBtns.AddRange(hackerModSlots); // Always need these three

        HackerLoadout hackerLoadout = FindObjectOfType<PlayerData>().GetCurrentHacker().GetHackerLoadout();
        HackerMod neuralImplant = hackerLoadout.GetNeuralImplantMod();
        switch (neuralImplant.GetCurrentLevelSlotCount())
        {
            case 1:
                wetware1SlotHolder.SetActive(true);
                wetware2SlotHolder.SetActive(false);
                wetware3SlotHolder.SetActive(false);
                activeHackerSlotBtns.AddRange(wetware1Slots);
                break;
            case 2:
                wetware1SlotHolder.SetActive(false);
                wetware2SlotHolder.SetActive(true);
                wetware3SlotHolder.SetActive(false);
                activeHackerSlotBtns.AddRange(wetware2Slots);
                break;
            case 3:
                wetware1SlotHolder.SetActive(false);
                wetware2SlotHolder.SetActive(false);
                wetware3SlotHolder.SetActive(true);
                activeHackerSlotBtns.AddRange(wetware3Slots);
                break;
        }

        HackerMod rig = hackerLoadout.GetRigMod();
        switch (rig.GetCurrentLevelSlotCount())
        {
            case 1:
                software1SlotHolder.SetActive(true);
                software2SlotHolder.SetActive(false);
                software3SlotHolder.SetActive(false);
                activeHackerSlotBtns.AddRange(software1Slots);
                break;
            case 2:
                software1SlotHolder.SetActive(false);
                software2SlotHolder.SetActive(true);
                software3SlotHolder.SetActive(false);
                activeHackerSlotBtns.AddRange(software2Slots);
                break;
            case 3:
                software1SlotHolder.SetActive(false);
                software2SlotHolder.SetActive(false);
                software3SlotHolder.SetActive(true);
                activeHackerSlotBtns.AddRange(software3Slots);
                break;
        }

        HackerMod uplink = hackerLoadout.GetUplinkMod();
        switch (uplink.GetCurrentLevelSlotCount())
        {
            case 1:
                chipset1SlotHolder.SetActive(true);
                chipset2SlotHolder.SetActive(false);
                chipset3SlotHolder.SetActive(false);
                activeHackerSlotBtns.AddRange(chipset1Slots);
                break;
            case 2:
                chipset1SlotHolder.SetActive(false);
                chipset2SlotHolder.SetActive(true);
                chipset3SlotHolder.SetActive(false);
                activeHackerSlotBtns.AddRange(chipset2Slots);
                break;
            case 3:
                chipset1SlotHolder.SetActive(false);
                chipset2SlotHolder.SetActive(false);
                chipset3SlotHolder.SetActive(true);
                activeHackerSlotBtns.AddRange(chipset3Slots);
                break;
        }

        foreach (LoadoutSlotBtn button in activeHackerSlotBtns)
        {
            button.SetupButton();
        }
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
        EnableOrDisableEquipButtonBasedOnSelection();
    }

    private void EnableOrDisableEquipButtonBasedOnSelection()
    {
        if (selectedItem == null)
        {
            runnerEquipButton.interactable = false;
            hackerEquipButton.interactable = false;
        } else
        {
            switch (selectedItem.GetHackerOrRunner())
            {
                case Item.HackerRunner.Runner:
                    Loadout runnerLoadout = FindObjectOfType<PlayerData>().GetCurrentRunner().GetLoadout();
                    RunnerMod mod = selectedItem as RunnerMod;
                    if (runnerLoadout.IsItemEquipped(mod))
                        runnerEquipButton.interactable = false;
                    else
                        runnerEquipButton.interactable = true;
                    break;
                case Item.HackerRunner.Hacker:
                    HackerLoadout hackerLoadout = FindObjectOfType<PlayerData>().GetCurrentHacker().GetHackerLoadout();
                    if (hackerLoadout.IsItemEquipped(selectedItem))
                        hackerEquipButton.interactable = false;
                    else
                        hackerEquipButton.interactable = true;
                    break;
            }
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
                if (item.IsHackerChipset())
                {
                    HackerModChip hackerModChip = item as HackerModChip;
                    cardIds.AddRange(hackerModChip.GetCardIds());
                }
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
        currentInventoryList.DestroyListItems();

        List<Item> items = FindObjectOfType<PlayerData>().GetPlayerItems();
        List<Item> filteredItems = new List<Item>();

        foreach (Item item in items)
        {
            if (currentFilters.Contains(item.GetItemType()))
            {
                filteredItems.Add(item);
            }
        }

        currentInventoryList.SetupInventoryList(fields, filteredItems, ItemDetailsMenu.ItemDetailMenuContextType.Loadout);

        if (!waitingToEquip)
        {
            SelectEquippedItemInList();
        } else
        {
            currentInventoryList.SelectParticularItem(selectedItem);
        }
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
                            currentInventoryList.SelectParticularItem(equippedMod);
                        }
                    }
                    break;
                case Item.HackerRunner.Hacker:
                    foreach (LoadoutSlotBtn button in activeHackerSlotBtns)
                    {
                        if (button.GetIsActive())
                        {
                            // do for mod
                            itemType = button.GetItemType();
                            if (button.GetItemType() == Item.ItemTypes.NeuralImplant || button.GetItemType() == Item.ItemTypes.Rig || button.GetItemType() == Item.ItemTypes.Uplink)
                            {
                                HackerMod equippedMod = playerData.GetCurrentHacker().GetHackerLoadout().GetEquippedModByItemType(itemType);
                                currentInventoryList.SelectParticularItem(equippedMod);
                            } else
                            {
                                // do for install
                                int slotNumber = button.GetSlotNumber();
                                HackerModChip hackerModChip = playerData.GetCurrentHacker().GetHackerLoadout().GetEquippedInstallByItemTypeAndSlotNumber(itemType, slotNumber);
                                currentInventoryList.SelectParticularItem(hackerModChip);
                            }
                        }
                    }
                    break;
            }
        } else if (recentlyEquippedItem == true)
        {
            recentlyEquippedItem = false;
            switch (hackerOrRunner)
            {
                case Item.HackerRunner.Runner:
                    currentInventoryList.SelectParticularItem(selectedItem);
                    break;
                case Item.HackerRunner.Hacker:
                    currentInventoryList.SelectParticularItem(selectedItem);
                    break;
            }
        }
    }

    private void InitialFilterSetup()
    {
        currentFilters = new List<Item.ItemTypes>();
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

    public void HandlePressedSlotButton(Item.ItemTypes itemTypeOnButton, Loadout.LeftOrRight leftOrRight, int slotNumber)
    {
        List<LoadoutSlotBtn> currentLoadoutSlotBtns = GetCurrentLoadoutSlotBtns();
        foreach (LoadoutSlotBtn button in currentLoadoutSlotBtns)
        {
            if (itemTypeOnButton != button.GetItemType() || leftOrRight != button.GetLeftOrRight() || slotNumber != button.GetSlotNumber())
            {
                button.SetInactive();
            }
        }
        if (currentFilters.Count != 1 || currentFilters.Count == 1 && currentFilters[0] != itemTypeOnButton)
        {
            UpdateFilters(itemTypeOnButton, leftOrRight, slotNumber);
            SetupInventoryList();
        } else
        {
            UpdateFilters(itemTypeOnButton, leftOrRight, slotNumber);
            SetupInventoryList();
            //SelectEquippedItemInList();
        }
    }

    private List<LoadoutSlotBtn> GetCurrentLoadoutSlotBtns()
    {
        if (hackerOrRunner == Item.HackerRunner.Runner)
            return runnerLoadoutSlotBtns;
        else
            return activeHackerSlotBtns;
    }

    private void UpdateFilters(Item.ItemTypes clickedBtnType, Loadout.LeftOrRight leftOrRight, int slotNumber)
    {
        List<LoadoutSlotBtn> currentButtons = GetCurrentLoadoutSlotBtns();
        bool foundActiveBtn = false;
        foreach (LoadoutSlotBtn button in currentButtons)
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
