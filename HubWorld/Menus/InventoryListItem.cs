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

    ItemDetailsMenu.ItemDetailMenuContextType context;

    HubWorldSFX hubWorldSFX;

    private void Awake()
    {
        hubWorldSFX = FindObjectOfType<HubWorldSFX>();
    }

    public void SetText(Item newItem, ItemDetailsMenu.ItemDetailMenuContextType newContext)
    {
        context = newContext;
        item = newItem;
        itemName.text = item.GetItemName();
        itemType.text = item.GetItemTypeForDisplay();
        itemLevel.text = item.GetItemLevel() + "/" + item.GetItemMaxLevel();

        switch (context)
        {
            default:
                if (IsItemEquipped())
                {
                    Color tempColor = runnerOrHackerIcon.color;
                    tempColor.a = 1f;
                    runnerOrHackerIcon.color = tempColor;
                    runnerOrHackerIcon.sprite = GetRunnerOrHackerIcon();
                } else
                {
                    Color tempColor = runnerOrHackerIcon.color;
                    tempColor.a = 0f;
                    runnerOrHackerIcon.color = tempColor;
                }
                break;
        }
    }

    private bool IsItemEquipped()
    {
        switch (item.GetHackerOrRunner())
        {
            case Item.HackerRunner.Runner:
                Loadout runnerLoadout = FindObjectOfType<PlayerData>().GetCurrentRunner().GetLoadout();
                RunnerMod currentMod = item as RunnerMod;
                return runnerLoadout.IsItemEquipped(currentMod);
            case Item.HackerRunner.Hacker:
                // TODO: FILL THIS SHIT IN FOR HACKER
                HackerLoadout hackerLoadout = FindObjectOfType<PlayerData>().GetCurrentHacker().GetHackerLoadout();
                return hackerLoadout.IsItemEquipped(item);
        }
        return false;
    }

    private Sprite GetRunnerOrHackerIcon()
    {
        if (item.GetHackerOrRunner() == Item.HackerRunner.Hacker)
        {
            return hackerIcon;
        }
        else
        {
            return runnerIcon;
        }
    }

    public void SelectListItem()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.Selecting);
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
