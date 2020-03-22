using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    // config
    [SerializeField] float setupTimeInSeconds = 1f;
    PlayerHand playerHand;

    // data
    CharacterData character;

    // state
    bool actionDisabled = true;

    private void Awake()
    {
        int count = FindObjectsOfType<BattleData>().Length;

        if (count > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetUpBattle()
    {
        playerHand = FindObjectOfType<PlayerHand>();

        playerHand.DrawStartingHand(character.GetStartingHandSize(), setupTimeInSeconds);
        character.BattleSetup(setupTimeInSeconds);

        StartCoroutine(EnablePlayAfterSetup());
    }

    private IEnumerator EnablePlayAfterSetup()
    {
        yield return new WaitForSeconds(setupTimeInSeconds);
        actionDisabled = false;
    }

    public bool AreActionsDisabled()
    {
        return actionDisabled;
    }

    public void SetCharacterData(CharacterData characterToSet)
    {
        character = characterToSet;
    }

    public CharacterData GetCharacter()
    {
        return character;
    }

    public float GetSetupTimeInSeconds()
    {
        return setupTimeInSeconds;
    }
}
