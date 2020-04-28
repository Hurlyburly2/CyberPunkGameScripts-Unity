using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerData : ScriptableObject
{
    string hackerName;

    HackerLoadout hackerLoadout;

    public void SetupHacker(string newHackerName)
    {
        hackerName = newHackerName;

        hackerLoadout = CreateInstance<HackerLoadout>();
        hackerLoadout.SetupInitialLoadout(hackerName);
    }

    public string GetName()
    {
        return hackerName;
    }

    public void LogHackerData()
    {
        Debug.Log("Hacker Name: " + hackerName);
    }

    public HackerLoadout GetHackerLoadout()
    {
        return hackerLoadout;
    }
}
