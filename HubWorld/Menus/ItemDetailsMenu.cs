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
        SetupGeneralInfo();
        runnerContextMenu.SetActive(true);
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
    }

    private void SetupHackerMenu()
    {
        SetupGeneralInfo();
        Debug.Log("Setup Hacker Menu");
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
