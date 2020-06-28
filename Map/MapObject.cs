using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : ScriptableObject
{
    string mapType;
    string type;
    // options are fed to it by mapSquare:
    // "Trap", "Reward", "PowerUp", "Shop", "Upgrade", "First Aid Station"
    bool isActive;
    // IF IT IS A TRAP AND IT IS NOT ACTIVE IT SHOULDN'T TRIGGER

    public void SetupMapObject(string newMapObjectType)
    {
        isActive = true;
        type = newMapObjectType;
        mapType = FindObjectOfType<MapData>().GetMapType();
    }

    public void SetIsActive(bool state)
    {
        isActive = false;
    }

    public string GetObjectType()
    {
        return type;
    }

    public bool GetIsActive()
    {
        return isActive;
    }
}
