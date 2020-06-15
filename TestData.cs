using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    public static CharacterData SetTestCharacterOne()
    {
        CharacterData newCharacter = ScriptableObject.CreateInstance<CharacterData>();

        string runnerName = "Runner";

        newCharacter.SetupCharacter(runnerName, 30, 30, 10, 0, 3);

        return newCharacter;
    }

    public static HackerData SetTestHackerOne()
    {
        HackerData newHacker = ScriptableObject.CreateInstance<HackerData>();
        string hackerName = "Hacker";

        newHacker.SetupHacker(hackerName);

        return newHacker;
    }
}
