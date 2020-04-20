using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerData : ScriptableObject
{
    public string hackerName;

    public void SetupHacker(string newHackerName)
    {
        hackerName = newHackerName;
    }

    public string GetName()
    {
        return hackerName;
    }
}
