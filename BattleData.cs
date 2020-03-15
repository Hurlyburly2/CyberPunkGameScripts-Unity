using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    // Next to do: Load scene and don't destroy this!!!
    CharacterData character;

    private void Awake()
    {
        int count = FindObjectsOfType<BattleData>().Length;

        if (count > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetCharacterData(CharacterData characterToSet)
    {
        character = characterToSet;
    }

    public CharacterData GetCharacter()
    {
        return character;
    }
}
