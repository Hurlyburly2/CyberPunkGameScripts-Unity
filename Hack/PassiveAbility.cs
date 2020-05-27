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
    int connectionType; // 2, 3, 4

    public void SetupPassiveAbility(string newAbilityType, int newMaxUses)
    {
        abilityType = newAbilityType;
        maxUses = newMaxUses;
        remainingUses = maxUses;
    }

    public void SetupPassiveAbility(string newAbilityType, int newMaxUses, string newColor, int newConnectionType)
    {
        abilityType = newAbilityType;
        maxUses = newMaxUses;
        remainingUses = maxUses;
        color = newColor;
        connectionType = newConnectionType;
    }
}
