﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerModChip : ScriptableObject
{
    string chipName;
    string type;
        // Software, Wetware, Chipset
    List<int> cardIds = new List<int>();

    public void SetupChip(string newChipName)
    {
        chipName = newChipName;
        SetChipProperties();
    }

    private void SetChipProperties()
    {
        switch(chipName)
        {
            case "Cheap Ghost":
                type = "Software";
                cardIds.Add(14);    // Hidden Trigger
                cardIds.Add(14);    // Hidden Trigger
                cardIds.Add(15);    // Too Obvious
                break;
            case "JuryRigged QwikThink":
                type = "Wetware";
                cardIds.Add(16);    // QwikThink
                cardIds.Add(17);    // Ad-Hoc Upgrade
                break;
            case "Salvaged Router":
                type = "Chipset";
                cardIds.Add(18);    // Failed Connection
                cardIds.Add(18);    // Failed Connection
                cardIds.Add(19);    // Cracked
                break;
            default:
                break;
        }
    }

    public string GetChipType()
    {
        return type;
    }

    public List<int> GetCardIds()
    {
        return cardIds;
    }

    public PassiveAbility SetupPassiveAbility()
    {
        PassiveAbility newAbility = CreateInstance<PassiveAbility>();
        Debug.Log("chip");
        switch(chipName)
        {
            case "Cheap Ghost":
                newAbility.SetupPassiveAbility("spikePointMultiplier", 2, "any", 2);
                // Your first 2 two-connection spikes are worth double points
                break;
            case "JuryRigged QwikThink":
                newAbility.SetupPassiveAbility("spikePointMultiplier", 2, "any", 3);
                // Your first 3 connection spike is worth double points
                break;
            case "Salvaged Router":
                newAbility.SetupPassiveAbility("dangerZoneBuffer", 1);
                // The first time you place a card outside the safe zone each hack, it does
                // not raise the security level
                break;
        }
        Debug.Log("chip2");

        return newAbility;
    }
}
