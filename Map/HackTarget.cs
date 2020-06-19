using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackTarget : ScriptableObject
{
    string mapType;
    string hackType;
        // options are fed to it by mapSquare:
        // "securityCamera", "combatServer", "database", "defenseSystem", "transportation", "medicalServer"

    public void SetupHackTarget(string newHackType)
    {
        hackType = newHackType;
        mapType = FindObjectOfType<MapData>().GetMapType();
    }
}
