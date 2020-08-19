using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryList : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI listHeaderOne;
    [SerializeField] TextMeshProUGUI listHeaderTwo;
    [SerializeField] TextMeshProUGUI listHeaderThree;

    [SerializeField] InventoryListControl inventoryListControl;

    string[] headers;
    List<Item> items;
    List<InventoryListItem> itemsInList;

    public void SetupInventoryList(string[] newHeaders, List<Item> itemsToList)
    {
        headers = newHeaders;
        items = itemsToList;
        RefreshHeaders();
        ListItemData();
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
}
