using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] TextMeshProUGUI itemLevel;
    [SerializeField] Image runnerOrHackerIcon;

    [SerializeField] Sprite runnerIcon;
    [SerializeField] Sprite hackerIcon;

    public void SetText(Item item)
    {
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

    public void ClickButton()
    {

    }
}
