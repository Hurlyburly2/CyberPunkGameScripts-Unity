using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackBattleData : MonoBehaviour
{
    CharacterData runner;
    HackerData hacker;

    public void SetCharacterData(CharacterData newRunner, HackerData newHacker)
    {
        runner = newRunner;
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
