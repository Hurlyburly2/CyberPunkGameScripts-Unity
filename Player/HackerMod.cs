using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerMod : Item
{
    int slots;
    int abilityUses;
    List<HackerModChip> modChips = new List<HackerModChip>();

    public void SetupMod(string newModName)
    {
        itemName = newModName;
        GetModProperties();
        itemLevel = 1;
        itemMaxLevel = 5;
        hackerOrRunner = HackerRunner.Hacker;
    }

    private void GetModProperties()
    {
        switch(itemName)
        {
            case "Basic Rig":
                slots = 1;
                itemType = ItemTypes.Rig;
                abilityUses = 1;
                break;
            case "Basic Cranial Dock":
                slots = 1;
                itemType = ItemTypes.NeuralImplant;
                abilityUses = 1;
                break;
            case "Basic Uplink":
                slots = 1;
                itemType = ItemTypes.Uplink;
                abilityUses = 2;
                break;
            default:
                break;
        }
        SetupEmptyMods();
    }

    public List<int> GetCardIds()
    {
        List<int> cardIds = new List<int>();
        foreach(HackerModChip modChip in modChips)
        {
            cardIds.AddRange(modChip.GetCardIds());
        }
        return cardIds;
    }

    private void SetupEmptyMods()
    {
        for (int i = 0; i < slots; i++)
        {
            HackerModChip emptyMod = CreateInstance<HackerModChip>();
            emptyMod.SetupChip("Empty");
            modChips.Add(emptyMod);
        }
    }

    public void InstallChip(HackerModChip newHackerModChip, int slot)
    {
        // check we're installing the right kind of chip in the mod
        ItemTypes newChipType = newHackerModChip.GetItemType();
        switch(itemType)
        {
            case ItemTypes.Rig:
                if (newChipType != ItemTypes.Software)
                {
                    Debug.LogError("Can only install software in Rig");
                }
                break;
            case ItemTypes.NeuralImplant:
                if (newChipType != ItemTypes.Wetware)
                {
                    Debug.LogError("Can only install wetware in Neural Implant");
                }
                break;
            case ItemTypes.Uplink:
                if (newChipType != ItemTypes.Chipset)
                {
                    Debug.LogError("Can only install chipsets in Uplink");
                }
                break;
        }
        modChips[slot] = newHackerModChip;
    }

    public int GetActiveAbilityId()
    {
        switch(itemName)
        {
            case "Basic Rig":
                return 0;
                // 1: Add a R connection your active card
            case "Basic Cranial Dock":
                return 1;
                // 1: For your next action, pick from your top two cards. Discard the other
            case "Basic Uplink":
                return 2;
                // 2: add the top card of your discard to the top of your deck
            default:
                return -1;
        }
    }

    public int GetActiveAbilityUses()
    {
        switch (itemName)
        {
            case "Basic Rig":
                return 1;
                // 1: Add a R connection to your active card
            case "Basic Cranial Dock":
                return 1;
                // 1: For your next action, pick from your top two cards. Discard the other
            case "Basic Uplink":
                return 2;
                // 2: add the top card of your discard to the top of your deck
            default:
                return -1;
        }
    }

    public List<HackerModChip> GetAttachedChips()
    {
        return modChips;
    }
}
