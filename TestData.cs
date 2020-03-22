using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    public static CharacterData SetTestCharacterOne()
    {
        CharacterData newCharacter = ScriptableObject.CreateInstance<CharacterData>();
        newCharacter.SetupCharacter("FullHealth/Energy", 1, 100, 100, 50, 50, 3);

        return newCharacter;
    }

    public static CharacterData SetTestCharacterTwo()
    {
        CharacterData newCharacter = ScriptableObject.CreateInstance<CharacterData>();
        newCharacter.SetupCharacter("FullHealth/Energy", 2, 200, 50, 100, 75, 4);

        return newCharacter;
    }
}
