using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradesMenu : MonoBehaviour
{
    ItemDetailsMenu.ItemDetailMenuContextType context;
    Item item;

    // General
    [SerializeField] TextMeshProUGUI itemLvlField;
    CardCaroselMultiple currentCardCarosel;

    // Hacker
    [SerializeField] GameObject hackerContext;

    // Runner
    [SerializeField] GameObject runnerContext;
    [SerializeField] GameObject runnerShopInventoryContext;
    [SerializeField] GameObject runnerUpgradeContext;
    [SerializeField] CardCaroselMultiple runnerCardCarosel;

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
        SetupCardCarosels();
    }

    private void SetupCardCarosels()
    {
        currentCardCarosel.ClearCardLists();
        currentCardCarosel.InitializeToggle();

        List<int> lvl1CardIds = new List<int>();
        if (item.GetHackerOrRunner() == Item.HackerRunner.Runner)
        {
            RunnerMod runnerMod = item as RunnerMod;
            lvl1CardIds.AddRange(runnerMod.GetCardIds());
        }
        else
        {
            HackerModChip hackerInstall = item as HackerModChip;
            lvl1CardIds.AddRange(hackerInstall.GetCardIds());
        }
        foreach (int id in lvl1CardIds)
        {
            Card card = Resources.Load<Card>("CardPrefabs/Player/Card" + id);
            currentCardCarosel.AddCardToList(card);
        }
        currentCardCarosel.GenerateListItems();
    }

    private void SetupHackerMenu()
    {
        runnerContext.SetActive(false);
        hackerContext.SetActive(true);
        Debug.Log("Setup Hacker Menu");
    }

    private void SetupGeneralInfo()
    {
        itemLvlField.text = item.GetItemLevel() + "/" + item.GetItemMaxLevel();
    }

    public void CloseUpgradesMenu()
    {
        gameObject.SetActive(false);
    }
}
