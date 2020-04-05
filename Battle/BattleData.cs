using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleData : MonoBehaviour
{
    // config
    [SerializeField] float setupTimeInSeconds = 1f;
    PlayerHand playerHand;
    Deck deck;
    Discard discard;
    ConfigData configData;

    // data
    CharacterData character;
    Enemy enemy;

    // state
    bool actionDisabled = true;
    string whoseTurn = "player";
        // possible values: player, enemy
        //  playerDiscard = player discarding at end of their turn

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
        deck = FindObjectOfType<Deck>();
        discard = FindObjectOfType<Discard>();

        // TODO: pass a value here, sets the current enemy to the test enemy and instantiates them
        SetUpEnemy(0);
        enemy.BattleSetup(setupTimeInSeconds);

        configData = FindObjectOfType<ConfigData>();
        configData.SetupStatusEffectHolders();

        character.BattleSetup(setupTimeInSeconds);
        deck.SetupDeck(character.GetLoadout().GetAllCardIds());
        playerHand.DrawStartingHand(character.GetStartingHandSize(), setupTimeInSeconds);

        StartCoroutine(EnablePlayAfterSetup());
    }

    public void SetUpEnemy(int enemyId)
    {
        ConfigData configData = FindObjectOfType<ConfigData>();

        // TODO: This will need to be built out as enemies and areas are added to the game
        Enemy[] allEnemies = FindObjectOfType<EnemyCollection>().GetEnemyArray();
        float enemyXPos = Camera.main.transform.position.x;
        float enemyYPos = configData.GetHalfHeight() + configData.GetHalfHeight() * .66f;
        enemy = Instantiate(allEnemies[enemyId], new Vector2(enemyXPos, enemyYPos), Quaternion.identity);
    }

    public void EndTurn()
    {
        if (whoseTurn == "player" || whoseTurn == "playerDiscard")
        {
            bool finishedDiscarding = DiscardDownToMaxHandSize();
            if (finishedDiscarding)
            {
                TickDownStatusEffectDurations("enemy");
                whoseTurn = "enemy";
                actionDisabled = true;
            }
        } else if (whoseTurn == "enemy") {
            // TODO Player draws to max hand size
            // TODO Switch back to player's turn and enable player actions
            TickDownStatusEffectDurations("player");
            whoseTurn = "player";
            actionDisabled = false;
        }
    }

    private void TickDownStatusEffectDurations(string whoseEffectsToTick)
    {
        StatusEffectHolder[] statusEffectHolders = FindObjectsOfType<StatusEffectHolder>();
        foreach(StatusEffectHolder statusEffectHolder in statusEffectHolders)
        {
            statusEffectHolder.TickDownStatusEffects(whoseEffectsToTick);
        }
    }

    private bool DiscardDownToMaxHandSize()
    {
        int extraCardsInHand = playerHand.GetCardsInHandCount() - character.GetStartingHandSize();
        PopupHolder popupHolder = FindObjectOfType<PopupHolder>();
        if (extraCardsInHand > 0)
        {
            popupHolder.SpawnDiscardPopup(extraCardsInHand);
            whoseTurn = "playerDiscard";
            return false;
        } else
        {
            popupHolder.DestroyAllPopups();
            return true;
        }
    }

    public string WhoseTurnIsIt()
    {
        return whoseTurn;
    }

    private IEnumerator EnablePlayAfterSetup()
    {
        yield return new WaitForSeconds(setupTimeInSeconds);
        FindObjectOfType<EndTurnButton>().GetComponent<Button>().interactable = true;
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

    public Enemy GetEnemy()
    {
        return enemy;
    }
}
