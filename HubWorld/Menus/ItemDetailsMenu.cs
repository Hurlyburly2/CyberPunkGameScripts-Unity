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
        switch(context)
        {
            case ItemDetailMenuContextType.Inventory:
                break;
            case ItemDetailMenuContextType.Loadout:
                break;
            case ItemDetailMenuContextType.Shop:
                break;
        }
    }

    private void SetupCardCarosel()
    {
        currentCardCarosel.ClearCardList();
        currentCardCarosel.InitializeToggle();
        RunnerMod runnerMod = item as RunnerMod;
        List<int> cardIds = runnerMod.GetCardIds();
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
