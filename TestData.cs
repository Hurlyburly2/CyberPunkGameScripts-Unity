using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    public static CharacterData SetTestCharacterOne()
    {
        // Full Health, Full Energy

        CharacterData newCharacter = ScriptableObject.CreateInstance<CharacterData>();
        newCharacter.SetupCharacter("FullHealth/Energy", 100, 100, 50, 50, 3);

        return newCharacter;
    }
}
