using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerData : ScriptableObject
{
    string hackerName;
    int id = 0;
        // id = different classes of hackers
            // 0: Base first hacker

    HackerLoadout hackerLoadout;

    public void CreateNewHackerByClassId(int newId)
    {
        switch(newId)
        {
            case 0:
                string newHackerName = "FirstHacker";
                SetupHacker(newHackerName);
                break;
        }
    }

    private void SetupHacker(string newHackerName)
    {
        hackerName = newHackerName;

        hackerLoadout = CreateInstance<HackerLoadout>();
        hackerLoadout.SetupInitialLoadout(id);
    }

    public string GetName()
    {
        return hackerName;
    }

    public void LogHackerData()
    {
        Debug.Log("Hacker Name: " + hackerName);
    }

    public List<Item> GetListOfEquippedItems()
    {
        return hackerLoadout.GetAllEquippedModsAndChipsAsItems();
    }

    public HackerLoadout GetHackerLoadout()
    {
        return hackerLoadout;
    }

    public int GetId()
    {
        return id;
    }
}
