using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : ScriptableObject
{
    public enum PowerUpType {
        Agility,
        BioElectricField,
        Counterattack,
        DeadEye,
        DireWound,
        ElementOfSurprise,
        EnergySiphon,
        MalwareExposure,
        GrindingGears,
        NetworkPenetration,
        PersonalShield,
        SlowedMetabolism,
        Strength,
        TankMode,
        Technologist,
        TheBestDefense,
    }

    PowerUpType powerUpType;
    int amount;
    int amount2;
    string powerUpName = "";
    string description = "";

    private PowerUpType GetRandomPowerUpType()
    {
        PowerUpType[] powerUpTypes = {
            PowerUpType.Agility,
            PowerUpType.BioElectricField,
            PowerUpType.Counterattack,
            PowerUpType.DeadEye,
            PowerUpType.DireWound,
            PowerUpType.ElementOfSurprise,
            PowerUpType.EnergySiphon,
            PowerUpType.MalwareExposure,
            PowerUpType.GrindingGears,
            PowerUpType.NetworkPenetration,
            PowerUpType.PersonalShield,
            PowerUpType.SlowedMetabolism,
            PowerUpType.Strength,
            PowerUpType.TankMode,
            PowerUpType.Technologist,
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
                description = amount + "% chance to draw an extra card at the beginning of each turn";
                break;
            case PowerUpType.BioElectricField:
                amount = 50;
                amount2 = 1;
                powerUpName = "Bioelectric Field";
                description = "Every time you play a Bio card: " + amount + "% chance to deal " + amount2 + " Damage";
                break;
            case PowerUpType.Counterattack:
                amount = 3;
                powerUpName = "Counterattack";
                description = "Every time you dodge an attack, deal " + amount + " damage";
                break;
            case PowerUpType.DeadEye:
                amount = 15;
                powerUpName = "DeadEye";
                description = "Base Critical Hit Chance increased by " + amount + "%";
                break;
            case PowerUpType.DireWound:
                amount = 50;
                powerUpName = "Dire Wound";
                description = "Your Critical Hits deal an extra " + amount + "% Damage";
                break;
            case PowerUpType.ElementOfSurprise:
                amount = 2;
                powerUpName = "Element of Surprise";
                description = "Begin each battle with " + amount + " Momentum";
                break;
            case PowerUpType.EnergySiphon:
                amount = 1;
                powerUpName = "Energy Siphon";
                description = "Gain " + amount + " energy at the start of each of your turns";
                break;
            case PowerUpType.MalwareExposure:
                amount = 25;
                powerUpName = "Malware Exposure";
                description = "Every time you play a Cyber card: " + amount + "% Chance to inflict Vulnerable";
                break;
            case PowerUpType.GrindingGears:
                amount = 25;
                powerUpName = "Grinding Gears";
                description = "Every time you play a Mech card: " + amount + "% chance to remove a random enemy buff. If no buffs were removed, inflict Weakness.";
                break;
            case PowerUpType.NetworkPenetration:
                amount = 25;
                amount2 = 1;
                powerUpName = "Network Penetration";
                description = "Every time you inflict a Debuff: " + amount + "% chance to increase the duration by " + amount2;
                break;
            case PowerUpType.PersonalShield:
                amount = 1;
                powerUpName = "Personal Shield";
                description = "Take no damage when you are hit by an enemy attack. Functions " + amount + " time per combat.";
                break;
            case PowerUpType.SlowedMetabolism:
                amount = 25;
                amount2 = 1;
                powerUpName = "Slowed Metabolism";
                description = "Every time you gain a buff: " + amount + "% chance to increase the duration by " + amount2;
                break;
            case PowerUpType.Strength:
                amount = 1;
                powerUpName = "Strength";
                description = "When gaining Momentum, gain " + amount + " extra stack";
                break;
            case PowerUpType.TankMode:
                amount = 1;
                powerUpName = "Tank Mode";
                description = "Every time you gain Damage Resist, Gain " + amount + " Retaliate";
                break;
            case PowerUpType.Technologist:
                amount = 25;
                powerUpName = "Technologist";
                description = "Every time you play a Tech card: " + amount + "% Chance to Draw 1 card";
                break;
            case PowerUpType.TheBestDefense:
                amount = 1;
                powerUpName = "The Best Defense";
                description = "Every time you gain Damage Resist or Dodge, gain " + amount + " Momentum";
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

    public int GetAmount2()
    {
        return amount2;
    }
}
