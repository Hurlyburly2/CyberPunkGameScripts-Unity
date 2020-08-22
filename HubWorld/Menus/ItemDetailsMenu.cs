using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetailsMenu : MonoBehaviour
{
    public enum ItemDetailMenuContextType { Inventory, Loadout, Shop };
    ItemDetailMenuContextType context;
    Item item;

    [SerializeField] GameObject runnerContextMenu;
    [SerializeField] GameObject hackerContextMenu;

    public void SetupItemDetailMenu(ItemDetailMenuContextType newContext, Item newItem)
    {
        Debug.Log(newItem.GetItemName());
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
        Debug.Log("Setup Runner Menu");
    }

    private void SetupHackerMenu()
    {
        Debug.Log("Setup Hacker Menu");
    }
}
