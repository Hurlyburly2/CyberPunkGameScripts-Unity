using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : ScriptableObject
{
    string mapType;
    string mapObjectType;
        // options are fed to it by mapSquare:
        // "Trap", "Reward", "PowerUp", "Shop", "Upgrade", "First Aid Station"
    bool isActive;
        // IF IT IS A TRAP AND IT IS NOT ACTIVE IT SHOULDN'T TRIGGER

    string trapType;
    int damageDealt = 0;

    public void SetupMapObject(string newMapObjectType)
    {
        isActive = true;
        mapObjectType = newMapObjectType;
        mapType = FindObjectOfType<MapData>().GetMapType();
        SetupTestObject();

        if (mapObjectType == "Trap")
        {
            SetupTrap();
        }
    }

    private void SetupTestObject()
    {
        mapObjectType = "First Aid Station";
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
            case "Shop":
                return Shop();
            case "Upgrade":
                return GainUpgrade();
            case "First Aid Station":
                return GainFirstAidStation();

        }
        return "Something went wrong";
    }

    private string GainReward()
    {
        string mapType = FindObjectOfType<MapGrid>().GetMapType();
        int minRange = 0;
        int maxRange = 0;
        switch (mapType)
        {
            case "slums":
                minRange = 100;
                maxRange = 300;
                break;
        }

        int rewardAmount = Mathf.FloorToInt(Random.Range(minRange, maxRange));
        FindObjectOfType<MapData>().ChangeMoney(rewardAmount);
        return "Gained " + rewardAmount.ToString() + " Credits";
    }

    private string GainPowerUp()
    {
        int random = Mathf.FloorToInt(Random.Range(1, 2));
        string returnString = "";
        switch (random)
        {
            case 1:
                returnString = "Combat: Chance each turn to draw an extra card";
                FindObjectOfType<MapData>().RaiseHandSizeBoostChance();
                break;
        }
        return returnString;
    }

    private string Shop()
    {
        return "SHOP NOT YET IMPLEMENTED";
    }

    private string GainUpgrade()
    {
        return "PERMANENTLY GAIN NEW MOD: NOT IMPLEMENTED. RARE.";
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

    public void TriggerTrap()
    {
        switch (trapType)
        {
            case "DirectDamage":
                CharacterData runner = FindObjectOfType<MapData>().GetRunner();
                float maxHealth = runner.GetMaximumHealth();
                int damageToTake = Mathf.FloorToInt(maxHealth * 0.2f);
                if (damageToTake < 1)
                    damageToTake = 1;
                damageDealt = damageToTake;
                runner.TakeDamageFromMap(damageToTake);
                isActive = false;
                break;
        }
    }

    public string GetTrapTextFromSquare()
    {
        switch (trapType)
        {
            case "DirectDamage":
                return "Took " + damageDealt.ToString() + " damage.";
        }
        return "";
    }

    private void SetupTrap()
    {
        // TODO: EXPAND TRAP TYPES HERE
        trapType = "DirectDamage";
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
}
