using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : ScriptableObject
{
    string mapType;
    string type;
    // options are fed to it by mapSquare:
    // "trap", "reward", "powerUp", "shop", "upgrade", "firstAidStation"

    public void SetupMapObject(string newMapObjectType)
    {
        type = newMapObjectType;
        mapType = FindObjectOfType<MapData>().GetMapType();
    }
}
