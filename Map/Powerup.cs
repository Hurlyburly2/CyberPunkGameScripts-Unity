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
        HackedTerminal,
        MalwareExposure,
        GrindingGears,
        NetworkPenetration,
        PersonalShield,
        PowerCoil,
        SlowedMetabolism,
        Strength,
        TankMode,
        Technologist,
        TheBestDefense,
        TheBestOffense
    }

    PowerUpType powerUpType;
    int amount;
    int amount2;
    int maxAmount;
    int level;
    int levelUpAmount;
    string powerUpName = "";

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
            PowerUpType.HackedTerminal,
            PowerUpType.MalwareExposure,
            PowerUpType.GrindingGears,
            PowerUpType.NetworkPenetration,
            PowerUpType.PersonalShield,
            PowerUpType.PowerCoil,
            PowerUpType.SlowedMetabolism,
            PowerUpType.Strength,
            PowerUpType.TankMode,
            PowerUpType.Technologist,
            PowerUpType.TheBestDefense,
            PowerUpType.TheBestOffense
        };
        return powerUpTypes[Random.Range(0, powerUpTypes.Length)];
    }

    public string SetupNewPowerUp()
    {
        level = 1;
        powerUpType = GetRandomPowerUpType();
        SetupSpecifics();
        return GetDescription();
    }

    private void SetupSpecifics()
    {
        switch (powerUpType)
        {
            case PowerUpType.Agility:
                amount = 25;
                maxAmount = 100;
                levelUpAmount = 15;
                powerUpName = "Agility";
                break;
            case PowerUpType.BioElectricField:
                amount = 50;
                maxAmount = 100;
                levelUpAmount = 15;
                amount2 = 1;
                powerUpName = "Bioelectric Field";
                break;
            case PowerUpType.Counterattack:
                amount = 3;
                maxAmount = 10;
                levelUpAmount = 1;
                powerUpName = "Counterattack";
                break;
            case PowerUpType.DeadEye:
                amount = 15;
                maxAmount = 100;
                levelUpAmount = 5;
                powerUpName = "DeadEye";
                break;
            case PowerUpType.DireWound:
                amount = 50;
                maxAmount = 250;
                levelUpAmount = 25;
                powerUpName = "Dire Wound";
                break;
            case PowerUpType.ElementOfSurprise:
                amount = 2;
                maxAmount = 10;
                levelUpAmount = 1;
                powerUpName = "Element of Surprise";
                break;
            case PowerUpType.EnergySiphon:
                amount = 1;
                maxAmount = 10;
                levelUpAmount = 1;
                powerUpName = "Energy Siphon";
                break;
            case PowerUpType.HackedTerminal:
                amount = 50;
                amount2 = 20;
                maxAmount = 80;
                levelUpAmount = 20;
                powerUpName = "Hacked Terminal";
                break;
            case PowerUpType.GrindingGears:
                amount = 25;
                levelUpAmount = 15;
                maxAmount = 100;
                powerUpName = "Grinding Gears";
                break;
            case PowerUpType.MalwareExposure:
                amount = 25;
                levelUpAmount = 15;
                maxAmount = 100;
                powerUpName = "Malware Exposure";
                break;
            case PowerUpType.NetworkPenetration:
                amount = 25;
                amount2 = 1;
                maxAmount = 100;
                levelUpAmount = 15;
                powerUpName = "Network Penetration";
                break;
            case PowerUpType.PersonalShield:
                amount = 1;
                maxAmount = 5;
                powerUpName = "Personal Shield";
                break;
            case PowerUpType.PowerCoil:
                amount = 15;
                maxAmount = 100;
                amount2 = 100;
                levelUpAmount = 10;
                powerUpName = "Power Coil";
                break;
            case PowerUpType.SlowedMetabolism:
                amount = 25;
                amount2 = 1;
                maxAmount = 100;
                levelUpAmount = 15;
                powerUpName = "Slowed Metabolism";
                break;
            case PowerUpType.Strength:
                amount = 1;
                levelUpAmount = 1;
                maxAmount = 10;
                powerUpName = "Strength";
                break;
            case PowerUpType.TankMode:
                amount = 1;
                maxAmount = 10;
                levelUpAmount = 1;
                powerUpName = "Tank Mode";
                break;
            case PowerUpType.Technologist:
                amount = 25;
                maxAmount = 100;
                levelUpAmount = 15;
                powerUpName = "Technologist";
                break;
            case PowerUpType.TheBestDefense:
                amount = 1;
                maxAmount = 10;
                levelUpAmount = 1;
                powerUpName = "The Best Defense";
                break;
            case PowerUpType.TheBestOffense:
                amount = 1;
                maxAmount = 10;
                levelUpAmount = 1;
                powerUpName = "The Best Offense";
                break;
        }
    }

    private string GetDescription()
    {
        switch (powerUpType)
        {
            case PowerUpType.Agility:
                return amount + "% chance to draw an extra card at the beginning of each turn";
            case PowerUpType.BioElectricField:
                return "Every time you play a Bio card: " + amount + "% chance to deal " + amount2 + " Damage";
            case PowerUpType.Counterattack:
                return "Every time you dodge an attack, deal " + amount + " damage";
            case PowerUpType.DeadEye:
                return "Base Critical Hit Chance increased by " + amount + "%";
            case PowerUpType.DireWound:
                return "Your Critical Hits deal an extra " + amount + "% Damage";
            case PowerUpType.ElementOfSurprise:
                return "Begin each battle with " + amount + " Momentum";
            case PowerUpType.EnergySiphon:
                return "Gain " + amount + " energy at the start of each of your turns";
            case PowerUpType.HackedTerminal:
                return "Half the current security level. Security level will rise slower";
            case PowerUpType.MalwareExposure:
                return "Every time you play a Cyber card: " + amount + "% Chance to inflict Vulnerable";
            case PowerUpType.GrindingGears:
                return "Every time you play a Mech card: " + amount + "% chance to remove a random enemy buff. If no buffs were removed, inflict Weakness.";
            case PowerUpType.NetworkPenetration:
                return "Every time you inflict a Debuff: " + amount + "% chance to increase the duration by " + amount2;
            case PowerUpType.PersonalShield:
                return "Take no damage when you are hit by an enemy attack. Functions " + amount + " time per combat.";
            case PowerUpType.PowerCoil:
                return amount + "% Chance every time you play a card: Refund " + amount2 + "% of that card's energy cost";
            case PowerUpType.SlowedMetabolism:
                return "Every time you gain a buff: " + amount + "% chance to increase the duration by " + amount2;
            case PowerUpType.Strength:
                return "When gaining Momentum, gain " + amount + " extra stack";
            case PowerUpType.TankMode:
                return "Every time you gain Damage Resist, Gain " + amount + " Retaliate";
            case PowerUpType.Technologist:
                return "Every time you play a Tech card: " + amount + "% Chance to Draw 1 card";
            case PowerUpType.TheBestDefense:
                return "Every time you gain Damage Resist or Dodge, gain " + amount + " Momentum";
            case PowerUpType.TheBestOffense:
                return "Permanent Retaliate 1 Buff";
        }
        return "";
    }

    public void UpgradePowerUp()
    {
        level++;
        switch (powerUpType)
        {
            case PowerUpType.PersonalShield:
            case PowerUpType.Strength:
            case PowerUpType.TheBestOffense:
                if ((float)level % 2 != 0)
                    amount = Mathf.Clamp(amount + levelUpAmount, 0, maxAmount);
                break;
            case PowerUpType.HackedTerminal:
                amount2 = Mathf.Clamp(amount2 + levelUpAmount, 0, maxAmount);
                break;
            default:
                amount = Mathf.Clamp(amount += levelUpAmount, 0, maxAmount);
                break;
        }
    }

    public PowerUpType GetPowerUpType()
    {
        return powerUpType;
    }

    public string GetPowerUpDescription()
    {
        return GetDescription();
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

    public int GetLevel()
    {
        return level;
    }
}
