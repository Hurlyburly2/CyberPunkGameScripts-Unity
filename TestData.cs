using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    public static CharacterData SetTestCharacterOne()
    {
        CharacterData newCharacter = ScriptableObject.CreateInstance<CharacterData>();
        newCharacter.CreateNewRunnerByClassId(0);

        return newCharacter;
    }

    public static HackerData SetTestHackerOne()
    {
        HackerData newHacker = ScriptableObject.CreateInstance<HackerData>();
        newHacker.CreateNewHackerByClassId(0);

        return newHacker;
    }
}
