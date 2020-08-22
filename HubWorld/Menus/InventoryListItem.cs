using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryListItem : MonoBehaviour
{
    [SerializeField] InventoryList parentInventoryList;

    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] TextMeshProUGUI itemLevel;
    [SerializeField] Image runnerOrHackerIcon;
    [SerializeField] Image highlight;

    [SerializeField] Sprite runnerIcon;
    [SerializeField] Sprite hackerIcon;

    bool isHighlighted = false;
    Item item;

    public void SetText(Item newItem)
    {
        item = newItem;
        itemName.text = item.GetItemName();
        itemType.text = item.GetItemTypeForDisplay();
        itemLevel.text = item.GetItemLevel() + "/" + item.GetItemMaxLevel();
        if (item.GetHackerOrRunner() == Item.HackerRunner.Hacker)
        {
            runnerOrHackerIcon.sprite = hackerIcon;
        } else
        {
            runnerOrHackerIcon.sprite = runnerIcon;
        }
    }

    public void SelectListItem()
    {
        if (isHighlighted)
        {
            isHighlighted = false;
        } else
        {
            isHighlighted = true;
        }
        UpdateHighlight();
        parentInventoryList.handleSelectAndDeselect(this);
    }

    public void SetIsHighlighted(bool newIsHighlighted)
    {
        isHighlighted = newIsHighlighted;
        UpdateHighlight();
    }

    private void UpdateHighlight()
    {
        if (isHighlighted)
        {
            highlight.gameObject.SetActive(true);
        } else
        {
            highlight.gameObject.SetActive(false);
        }
    }

    public bool GetIsHighlighted()
    {
        return isHighlighted;
    }

    public Item GetItem()
    {
        return item;
    }
}
