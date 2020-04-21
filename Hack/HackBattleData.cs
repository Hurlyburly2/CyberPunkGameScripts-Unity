using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackBattleData : MonoBehaviour
{
    HackerData hacker;

    public void SetHackerData(HackerData newHacker)
    {
        hacker = newHacker;
    }

    public void SetupHack()
    {
        Debug.Log("SETUP HACK");
    }

    public HackerData GetHacker()
    {
        return hacker;
    }
}
