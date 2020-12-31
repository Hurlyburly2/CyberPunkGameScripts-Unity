using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : ScriptableObject
{
    public enum PowerUpType {
        Agility,
        DeadEye,
        DireWound,
        ElementOfSurprise,
        Strength,
        TheBestDefense,
    }

    PowerUpType powerUpType;
    int amount;
    string powerUpName = "";
    string description = "";

    private PowerUpType GetRandomPowerUpType()
    {
        PowerUpType[] powerUpTypes = {
            PowerUpType.Agility,
            PowerUpType.DeadEye,
            PowerUpType.DireWound,
            PowerUpType.ElementOfSurprise,
            PowerUpType.Strength,
            PowerUpType.TheBestDefense,
        };
        return powerUpTypes[Random.Range(0, powerUpTypes.Length)];
    }

    public string SetupNewPowerUp()
    {
        powerUpType = GetRandomPowerUpType();
        SetupSpecifics();
        return description;
    }

    private void SetupSpecifics()
    {
        switch (powerUpType)
        {
            case PowerUpType.Agility:
                amount = 25;
                powerUpName = "Agility";
                description = "25% chance to draw an extra card at the beginning of each turn";
                break;
            case PowerUpType.DeadEye:
                amount = 15;
                powerUpName = "DeadEye";
                description = "Base Critical Hit Chance increased by 15%";
                break;
            case PowerUpType.DireWound:
                amount = 50;
                powerUpName = "Dire Wound";
                description = "Your Critical Hits deal an extra 50% Damage";
                break;
            case PowerUpType.ElementOfSurprise:
                amount = 2;
                powerUpName = "Element of Surprise";
                description = "Begin each battle with 2 Momentum";
                break;
            case PowerUpType.Strength:
                amount = 1;
                powerUpName = "Strength";
                description = "When gaining Momentum, gain an extra stack";
                break;
            case PowerUpType.TheBestDefense:
                amount = 1;
                powerUpName = "The Best Defense";
                description = "Every time you gain Damage Resist or Dodge, gain 1 Momentum";
                break;
        }
    }

    public PowerUpType GetPowerUpType()
    {
        return powerUpType;
    }

    public string GetPowerUpDescription()
    {
        return description;
    }

    public int GetAmount()
    {
        return amount;
    }

    public string GetName()
    {
        return powerUpName;
    }
}
