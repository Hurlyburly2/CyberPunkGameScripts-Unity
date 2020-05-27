using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : ScriptableObject
{
    // GENERAL CONFIG
    string abilityType;
        // spikePointMultiplier multiplies points from certain kinds/colors of spikes
        // dangerZoneBuffer = You get X chances to place cards in danger zone until security level raises
    int maxUses;
    int remainingUses;

    // spikePointMultiplier variables
    string color;
    int connectionType; // 1, 3, 9
    int multiplier;

    public void SetupPassiveAbility(string newAbilityType, int newMaxUses)
    {
        abilityType = newAbilityType;
        maxUses = newMaxUses;
        remainingUses = maxUses;
    }

    public void SetupPassiveAbility(string newAbilityType, int newMaxUses, string newColor, int newConnectionType, int newMultiplier)
    {
        abilityType = newAbilityType;
        maxUses = newMaxUses;
        remainingUses = maxUses;
        color = newColor;
        connectionType = newConnectionType;
        multiplier = newMultiplier;
    }

    public string GetAbilityType()
    {
        return abilityType;
    }

    public int GetRemainingUses()
    {
        return remainingUses;
    }

    public int GetConnectionType()
    {
        return connectionType;
    }

    public string GetColor()
    {
        return color;
    }

    public int GetMultiplier()
    {
        return multiplier;
    }

    public void UseOne()
    {
        remainingUses--;
    }
}
