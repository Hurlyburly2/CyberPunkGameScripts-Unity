using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    public static CharacterData SetTestCharacterOne()
    {
        CharacterData newCharacter = ScriptableObject.CreateInstance<CharacterData>();

        string runnerName = "Runner";
        string hackerName = "Hacker";

        newCharacter.SetupCharacter(runnerName, hackerName, 30, 30, 10, 10, 3);

        return newCharacter;
    }

    public static CharacterData SetTestCharacterTwo()
    {
        CharacterData newCharacter = ScriptableObject.CreateInstance<CharacterData>();

        string runnerName = "Runner";
        string hackerName = "Hacker";

        // hacker and runner names are swapped for sake of portrait testing for now..
        newCharacter.SetupCharacter(hackerName, runnerName, 200, 50, 100, 75, 4);

        return newCharacter;
    }
}
