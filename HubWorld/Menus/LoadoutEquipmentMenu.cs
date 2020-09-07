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
    [SerializeField] InventoryList inventoryList;

    // Hacker Stuff
    [SerializeField] GameObject hackerContext;

    Item.HackerRunner hackerOrRunner;

    CharacterData runner;
    HackerData hacker;
    InventoryMenu.InventoryFields[] fields = { InventoryMenu.InventoryFields.Name, InventoryMenu.InventoryFields.Type, InventoryMenu.InventoryFields.Lvl };

    private void DoSetup()
    {
        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                hackerContext.SetActive(false);
                runnerContext.SetActive(true);
                runnerName.text = runner.GetRunnerName();
                runnerDescription.text = runner.GetBio();
                break;
            case Item.HackerRunner.Hacker:
                runnerContext.SetActive(false);
                hackerContext.SetActive(true);
                // setup hacker name
                // setup hacker description (if necessary)
                break;
        }
        foreach (LoadoutSlotBtn button in runnerLoadoutSlotBtns)
        {
            button.SetupButton();
        }
        SetupInventoryList();
    }

    private void SetupInventoryList()
    {
        inventoryList.DestroyListItems();

        List<Item> items = FindObjectOfType<PlayerData>().GetPlayerItems();
        //List<Item> filteredItems = new List<Item>();

        // Filter the items based on selection up above...

        //inventoryList.SetupInventoryList(fields, filteredItems);
        inventoryList.SetupInventoryList(fields, items);
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
