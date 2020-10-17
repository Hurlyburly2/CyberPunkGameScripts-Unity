using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryList : MonoBehaviour
{
    [SerializeField] ItemDetailsMenu itemDetailsMenu;
    [SerializeField] LoadoutEquipmentMenu loadoutEquipmentMenu;
    [SerializeField] ShopMenu shopMenu;

    [SerializeField] TextMeshProUGUI listHeaderOne;
    [SerializeField] TextMeshProUGUI listHeaderTwo;
    [SerializeField] TextMeshProUGUI listHeaderThree;

    InventoryMenu.InventoryFields sortBy = InventoryMenu.InventoryFields.None;
    bool sortAscending = true;
    [SerializeField] Button detailsButton;

    [SerializeField] InventoryListControl inventoryListControl;

    InventoryMenu.InventoryFields[] headers;
    List<Item> items;
    List<InventoryListItem> itemsInList = new List<InventoryListItem>();
    ItemDetailsMenu.ItemDetailMenuContextType context;

    public void SetupInventoryList(InventoryMenu.InventoryFields[] newHeaders, List<Item> itemsToList, ItemDetailsMenu.ItemDetailMenuContextType newContext)
    {
        context = newContext;
        if (sortBy == InventoryMenu.InventoryFields.None)
            sortBy = newHeaders[0];

        headers = newHeaders;
        items = itemsToList;
        RefreshHeaders();
        items.Sort(SortComparator);
        ListItemData();
        detailsButton.interactable = false;
    }

    private void AdjustSort(InventoryMenu.InventoryFields fieldToSortBy)
    {
        if (sortBy == fieldToSortBy)
        {
            sortAscending = !sortAscending;
        }
        else
        {
            sortBy = fieldToSortBy;
        }
        items.Sort(SortComparator);
        DestroyListItems();
        ListItemData();
        // todo: DO SOMETHING TO REMEMBER WHICH ITEM IS SELECTED TO AVOID THIS
        detailsButton.interactable = false;
    }

    private int SortComparator(Item item1, Item item2)
    {
        if (sortAscending)
        {
            if (sortBy == InventoryMenu.InventoryFields.Name)
                return item1.GetItemName().CompareTo(item2.GetItemName());
            else if (sortBy == InventoryMenu.InventoryFields.Type)
                return item1.GetItemType().ToString().CompareTo(item2.GetItemType().ToString());
            else if (sortBy == InventoryMenu.InventoryFields.Lvl)
                return item1.GetItemLevel().CompareTo(item2.GetItemLevel());
            else
                return 0;
        }
        else
        {
            if (sortBy == InventoryMenu.InventoryFields.Name)
                return item2.GetItemName().CompareTo(item1.GetItemName());
            else if (sortBy == InventoryMenu.InventoryFields.Type)
                return item2.GetItemType().ToString().CompareTo(item1.GetItemType().ToString());
            else if (sortBy == InventoryMenu.InventoryFields.Lvl)
                return item2.GetItemLevel().CompareTo(item1.GetItemLevel());
            else
                return 0;
        }
    }

    public void handleSelectAndDeselect(InventoryListItem selectedItem)
    {
        bool isAnItemHighlighted = false;
        foreach (InventoryListItem listItem in itemsInList)
        {
            if (listItem != selectedItem)
            {
                listItem.SetIsHighlighted(false);
            }
            else
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
        }
        else
        {
            detailsButton.interactable = false;
        }

        switch (context)
        {
            case ItemDetailsMenu.ItemDetailMenuContextType.Loadout:
                loadoutEquipmentMenu.HandleSelectedItem(selectedItem.GetItem(), selectedItem.GetIsHighlighted());
                break;
            case ItemDetailsMenu.ItemDetailMenuContextType.Shop:
                shopMenu.HandleSelectItem(selectedItem.GetItem(), selectedItem.GetIsHighlighted());
                break;
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
        itemsInList = new List<InventoryListItem>();
    }

    private void ListItemData()
    {
        itemsInList = new List<InventoryListItem>();
        // add list items to the viewer, and store references to them here for destroying later
        foreach (Item item in items)
        {
            itemsInList.Add(inventoryListControl.AddItemToList(item, context));
        }
    }

    private void RefreshHeaders()
    {
        listHeaderOne.text = headers[0].ToString();
        listHeaderTwo.text = headers[1].ToString();
        listHeaderThree.text = headers[2].ToString();
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

    public void SelectParticularItem(Item itemToSelect)
    {
        foreach (InventoryListItem listItem in itemsInList)
        {
            if (listItem.GetItem().GetInstanceID() == itemToSelect.GetInstanceID())
            {
                listItem.SelectListItem();
            }
        }
    }

    public void OpenDetailsMenu()
    {
        itemDetailsMenu.gameObject.SetActive(true);
        itemDetailsMenu.SetupItemDetailMenu(context, GetSelectedItem());
    }

    public void ClickHeaderOne()
    {
        AdjustSort(headers[0]);
    }

    public void ClickHeaderTwo()
    {
        AdjustSort(headers[1]);
    }

    public void ClickHeaderThree()
    {
        AdjustSort(headers[2]);
    }
}
