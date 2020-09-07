using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutEquipmentMenu : MonoBehaviour
{
    [SerializeField] GameObject runnerContext;
    [SerializeField] GameObject hackerContext;
    [SerializeField] List<LoadoutSlotBtn> loadoutSlotBtns;

    Item.HackerRunner hackerOrRunner;

    CharacterData runner;
    HackerData hacker;

    private void DoSetup()
    {
        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                hackerContext.SetActive(false);
                runnerContext.SetActive(true);
                break;
            case Item.HackerRunner.Hacker:
                runnerContext.SetActive(false);
                hackerContext.SetActive(true);
                break;
        }
        foreach (LoadoutSlotBtn button in loadoutSlotBtns)
        {
            button.SetupButton();
        }
    }

    public void HandlePressedSlotButton(Item.ItemTypes itemTypeOnButton, Loadout.LeftOrRight leftOrRight)
    {
        foreach (LoadoutSlotBtn button in loadoutSlotBtns)
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
