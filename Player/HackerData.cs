using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerData : ScriptableObject
{
    string hackerName;

    HackerLoadout hackoutLoadout;

    public void SetupHacker(string newHackerName)
    {
        hackerName = newHackerName;
    }

    public string GetName()
    {
        return hackerName;
    }

    public void LogHackerData()
    {
        Debug.Log("Hacker Name: " + hackerName);
    }
}
