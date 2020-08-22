using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryList : MonoBehaviour
{
    [SerializeField] ItemDetailsMenu itemDetailsMenu;

    [SerializeField] TextMeshProUGUI listHeaderOne;
    [SerializeField] TextMeshProUGUI listHeaderTwo;
    [SerializeField] TextMeshProUGUI listHeaderThree;
    [SerializeField] Button detailsButton;

    [SerializeField] InventoryListControl inventoryListControl;

    string[] headers;
    List<Item> items;
    List<InventoryListItem> itemsInList = new List<InventoryListItem>();

    public void SetupInventoryList(string[] newHeaders, List<Item> itemsToList)
    {
        headers = newHeaders;
        items = itemsToList;
        RefreshHeaders();
        ListItemData();
        detailsButton.interactable = false;
    }

    public void handleSelectAndDeselect(InventoryListItem selectedItem)
    {
        bool isAnItemHighlighted = false;
        foreach (InventoryListItem listItem in itemsInList)
        {
            if (listItem != selectedItem)
            {
                listItem.SetIsHighlighted(false);
            } else
            {
                if (listItem.GetIsHighlighted())
                {
                    isAnItemHighlighted = true;
                }
            }
        }

        if (isAnItemHighlighted)
        {
            detailsButton.interactable = true;
        } else
        {
            detailsButton.interactable = false;
        }
    }

    public void DestroyListItems()
    {
        if (itemsInList.Count == 0)
            return;

        for (int i = 0; i < itemsInList.Count; i++)
        {
            Destroy(itemsInList[i].gameObject);
        }
    }

    private void ListItemData()
    {
        itemsInList = new List<InventoryListItem>();
        // add list items to the viewer, and store references to them here for destroying later
        foreach (Item item in items)
        {
            itemsInList.Add(inventoryListControl.AddItemToList(item));
        }
    }

    private void RefreshHeaders()
    {
        listHeaderOne.text = headers[0];
        listHeaderTwo.text = headers[1];
        listHeaderThree.text = headers[2];
    }

    private Item GetSelectedItem()
    {
        foreach (InventoryListItem listItem in itemsInList)
        {
            if (listItem.GetIsHighlighted())
            {
                return listItem.GetItem();
            }
        }
        return itemsInList[0].GetItem();
    }

    public void OpenDetailsMenu()
    {
        itemDetailsMenu.gameObject.SetActive(true);
        itemDetailsMenu.SetupItemDetailMenu(ItemDetailsMenu.ItemDetailMenuContextType.Inventory, GetSelectedItem());
    }
}
