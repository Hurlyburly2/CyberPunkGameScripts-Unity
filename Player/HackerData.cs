using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerData : ScriptableObject
{
    string hackerName;
    string bio;
    int id = 0;
        // id = different classes of hackers
            // 0: Base first hacker
    bool locked = true;

    HackerLoadout hackerLoadout;

    public void CreateNewHackerByClassId(int newId)
    {
        id = newId;
        switch(newId)
        {
            case 0:
                string newHackerName = "FirstHacker";
                string newBio = "Risus commodo viverra maecenas. Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
                SetupHacker(newHackerName, newBio);
                break;
            case 1:
                newHackerName = "Locked Hacker1";
                newBio = "Not Implemented 1";
                SetupHacker(newHackerName, newBio);
                break;
            case 2:
                newHackerName = "Locked Hacker2";
                newBio = "Not Implemented 2";
                SetupHacker(newHackerName, newBio);
                break;
        }
    }

    private void SetupHacker(string newHackerName, string newBio)
    {
        hackerName = newHackerName;
        bio = newBio;

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

    public string GetBio()
    {
        return bio;
    }

    public void UnlockHacker()
    {
        locked = false;
    }
}
