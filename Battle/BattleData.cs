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
    bool skipEndTurnDiscard;
        // if true, skip discard and set to false

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
        if (whoseTurn == "player")
        {
            bool foundWeaknesses = false;
            if (!skipEndTurnDiscard)
            {
                // If we're skipping end turn discards, we also let the player skip weakness playing
                foundWeaknesses = AreThereWeaknesses();
            }
            
            if (!foundWeaknesses)
            {
                bool finishedDiscarding = DiscardDownToMaxHandSize();
                if (finishedDiscarding)
                {
                    TickDownStatusEffectDurations("enemy");
                    whoseTurn = "enemy";
                    actionDisabled = true;
                }
            }
        } else if (whoseTurn == "enemy") {
            playerHand.DrawToMaxHandSize();
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

    private bool AreThereWeaknesses()
    {
        bool areThereWeaknesses = playerHand.AreThereWeaknesses();
        PopupHolder popupHolder = FindObjectOfType<PopupHolder>();
        if (areThereWeaknesses)
        {
            popupHolder.SpawnWeaknessesInHandPopup();
            return true;
        } else
        {
            popupHolder.DestroyAllPopups();
            return false;
        }
    }

    private bool DiscardDownToMaxHandSize()
    {
        int extraCardsInHand = playerHand.GetCardsInHandCount() - character.GetStartingHandSize();
        PopupHolder popupHolder = FindObjectOfType<PopupHolder>();

        if (skipEndTurnDiscard)
        {
            skipEndTurnDiscard = false;
            return true;
        }

        if (extraCardsInHand > 0)
        {
            List<Card> cardsInHand = playerHand.GetCardsInHand();
            configData.GetCardPicker().Initialize(cardsInHand, extraCardsInHand, "DiscardCardsFromHand");
            return false;
        } else
        {
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

    public void SkipEndTurnDiscard(bool newSkipDiscard)
    {
        skipEndTurnDiscard = newSkipDiscard;
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
