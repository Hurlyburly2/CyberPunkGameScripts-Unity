using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : ScriptableObject
{
    string mapType;
    string type;
    // options are fed to it by mapSquare:
    // "Trap", "Reward", "PowerUp", "Shop", "Upgrade", "First Aid Station"

    public void SetupMapObject(string newMapObjectType)
    {
        type = newMapObjectType;
        mapType = FindObjectOfType<MapData>().GetMapType();
    }

    public string GetObjectType()
    {
        return type;
    }
}
