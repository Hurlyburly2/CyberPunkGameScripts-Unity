using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackTarget : ScriptableObject
{
    string mapType;
    string hackType;
    // options are fed to it by mapSquare:
    // "Security Camera", "Combat Server", "Database", "Defense System", "Transportation", "Medical Server"

    public void SetupHackTarget(string newHackType)
    {
        hackType = newHackType;
        mapType = FindObjectOfType<MapData>().GetMapType();
    }

    public string getHackType()
    {
        return hackType;
    }
}
