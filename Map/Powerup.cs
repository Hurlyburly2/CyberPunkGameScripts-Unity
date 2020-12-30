using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : ScriptableObject
{
    public enum PowerUpType {
        DrawExtraCardChance
    }

    PowerUpType powerUpType;
    int amount;
    string description = "";

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
            case PowerUpType.DrawExtraCardChance:
                amount = 25;
                description = "Combat: 25% chance to draw an extra card at the beginning of each turn";
                break;
        }
    }

    private PowerUpType GetRandomPowerUpType()
    {
        PowerUpType[] powerUpTypes = { PowerUpType.DrawExtraCardChance };
        return powerUpTypes[Random.Range(0, powerUpTypes.Length)];
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
}
