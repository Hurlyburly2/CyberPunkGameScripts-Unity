using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerMod : ScriptableObject
{
    string modName;
    string type;
        // Rig, Neural Implant, or Uplink
    int slots;
    int abilityUses;
    List<HackerModChip> modChips = new List<HackerModChip>();

    public void SetupMod(string newModName)
    {
        modName = newModName;
        GetModProperties();
    }

    private void GetModProperties()
    {
        switch(modName)
        {
            case "Basic Rig":
                slots = 1;
                type = "Rig";
                abilityUses = 1;
                break;
            case "Basic Cranial Dock":
                slots = 1;
                type = "Neural Implant";
                abilityUses = 1;
                break;
            case "Basic Uplink":
                slots = 1;
                type = "Uplink";
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
        string newChipType = newHackerModChip.GetChipType();
        switch(type)
        {
            case "Rig":
                if (newChipType != "Software")
                {
                    Debug.LogError("Can only install software in Rig");
                }
                break;
            case "Neural Implant":
                if (newChipType != "Wetware")
                {
                    Debug.LogError("Can only install wetware in Neural Implant");
                }
                break;
            case "Uplink":
                if (newChipType != "Chipset")
                {
                    Debug.LogError("Can only install chipsets in Uplink");
                }
                break;
        }
        modChips[slot] = newHackerModChip;
    }

    public void GetActiveAbility()
    {
        switch(modName)
        {
            case "Basic Rig":
                Debug.Log("1: Add a R connection and two R spikes to your active card");
                break;
            case "Basic Cranial Dock":
                Debug.Log("1: For your next action, pick from your top two cards. Discard the other.");
                break;
            case "Basic Uplink":
                Debug.Log("2: you may play the top card of your discard as if it was your active card");
                break;
            default:
                break;
        }
    }
}
