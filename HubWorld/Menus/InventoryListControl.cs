﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryListControl : MonoBehaviour
{
    [SerializeField] InventoryListItem itemTemplate;

    public InventoryListItem AddItemToList(Item item)
    {
        InventoryListItem listItem = Instantiate(itemTemplate);
        listItem.gameObject.SetActive(true);

        listItem.GetComponent<InventoryListItem>().SetText(item);
        listItem.transform.SetParent(itemTemplate.transform.parent, false);

        return listItem;
    }
}