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
        //SetupTestObject();

        if (mapObjectType == "Trap")
        {
            SetupTrap();
        }
    }

    private void SetupTestObject()
    {
        mapObjectType = "Trap";
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
