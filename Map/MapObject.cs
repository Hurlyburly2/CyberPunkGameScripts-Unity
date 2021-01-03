using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : ScriptableObject
{
    Job.JobArea mapType;
    string mapObjectType;
        // options are fed to it by mapSquare:
        // "Trap", "Reward", "PowerUp", "Upgrade", "First Aid Station"

    bool isActive;
    // IF IT IS A TRAP AND IT IS NOT ACTIVE IT SHOULDN'T TRIGGER

    public enum TrapTypes { DirectDamage, FaradayCage, EMP };
    TrapTypes trapType;
    PowerUp powerup;
    string trapName = "";
    string trapDescription = "";
    int trapAmount;

    public void SetupMapObject(string newMapObjectType)
    {
        isActive = true;
        mapObjectType = newMapObjectType;
        mapType = FindObjectOfType<MapData>().GetMapType();

        // TODO: REMOVE THIS WHEN DONE WORKING ON TRAPS
        //mapObjectType = "Trap";

        if (mapObjectType == "Trap")
        {
            SetupTrap();
        }
    }

    public string DoObjectAction()
    {
        isActive = false;
        switch (mapObjectType)
        {
            case "Trap":
                return "This shouldn't have happened";
            case "Reward":
                return GainReward();
            case "PowerUp":
                return GainPowerUp();
            case "Upgrade":
                return GainUpgrade();
            case "First Aid Station":
                return GainFirstAidStation();

        }
        return "Something went wrong";
    }

    private string GainReward()
    {
        Job.JobArea mapType = FindObjectOfType<MapGrid>().GetMapType();
        int minRange = 0;
        int maxRange = 0;
        switch (mapType)
        {
            case Job.JobArea.Slums:
                minRange = 100;
                maxRange = 300;
                break;
            case Job.JobArea.Downtown:
                minRange = 300;
                maxRange = 600;
                break;
        }

        int rewardAmount = Mathf.FloorToInt(Random.Range(minRange, maxRange));
        FindObjectOfType<MapData>().ChangeMoney(rewardAmount);
        return "Gained " + rewardAmount.ToString() + " Credits";
    }

    private string GainPowerUp()
    {
        PowerUp newPowerUp = ScriptableObject.CreateInstance<PowerUp>();
        newPowerUp.SetupNewPowerUp();
        FindObjectOfType<MapData>().AddPowerUp(newPowerUp);
        powerup = newPowerUp;
        return newPowerUp.GetPowerUpDescription();
    }

    private string GainUpgrade()
    {
        return "UPGRADE YOUR POWERUPS";
    }

    private string GainFirstAidStation()
    {
        CharacterData runner = FindObjectOfType<MapData>().GetRunner();
        float maxHealth = runner.GetMaximumHealth();
        int healAmount = Mathf.FloorToInt(maxHealth * .2f);
        runner.GainHealthOnMap(healAmount);
        return "Gained "  + healAmount.ToString() + " health.";
    }

    public string GetObjectTypeNameForDisplay()
    {
        if (mapObjectType == "PowerUp")
        {
            return "Power Up";
        } else
        {
            return mapObjectType;
        }
    }

    public void TriggerTrap(MapSquare mapSquare)
    {
        switch (trapType)
        {
            case TrapTypes.DirectDamage:
                FindObjectOfType<MapData>().GetRunner().TakeDamageFromMap(trapAmount);
                isActive = false;
                break;
            case TrapTypes.FaradayCage:
                mapSquare.SetTriggeredTrapType(trapType);
                isActive = false;
                break;
            case TrapTypes.EMP:
                mapSquare.SetTriggeredTrapType(trapType, trapAmount);
                isActive = false;
                break;
        }
    }

    public string GetTrapDescription()
    {
        return trapDescription;
    }

    public string GetTrapName()
    {
        return trapName;
    }

    private void SetupTrap()
    {
        TrapTypes[] potentialTrapTypes = {
            TrapTypes.DirectDamage,
            TrapTypes.FaradayCage,
            TrapTypes.EMP
        };
        trapType = potentialTrapTypes[Random.Range(0, potentialTrapTypes.Length)];
        switch (trapType)
        {
            case TrapTypes.DirectDamage:
                // Deal 15% damage
                CharacterData runner = FindObjectOfType<MapData>().GetRunner();
                float maxHealth = runner.GetMaximumHealth();
                int damageToTake = Mathf.FloorToInt(maxHealth * 0.2f);
                if (damageToTake < 1)
                    damageToTake = 1;
                trapAmount = damageToTake;
                trapName = "Trap";

                trapDescription = "Took " + trapAmount.ToString() + " damage.";
                break;
            case TrapTypes.FaradayCage:
                trapName = "Faraday Cage";
                trapDescription = "On this location: Block use of Hacker cards";
                break;
            case TrapTypes.EMP:
                trapName = "EMP";
                trapAmount = 30;
                trapDescription = "On this location: Your Cyber and Tech cards have a " + trapAmount + "% chance to fizzle";
                break;
        }
    }

    public int GetTrapAmount()
    {
        return trapAmount;
    }

    public void SetIsActive(bool state)
    {
        isActive = false;
    }

    public string GetObjectType()
    {
        return mapObjectType;
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public string GetName()
    {
        switch (mapObjectType)
        {
            case "PowerUp":
                return powerup.GetName();
            case "First Aid Station":
                return "First Aid Station";
            case "Reward":
                return "Reward";
            case "Trap":
                return "TRAPNAME NOT IMPLMENTED";
            case "Upgrade":
                return "UPGRADE NOT IMPLEMENTED";
        }
        return "Something went wrong";
    }
}
