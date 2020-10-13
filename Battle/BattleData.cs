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
    MapSquare mapSquare;

    // data
    CharacterData character;
    HackerData hacker;
    Enemy enemy;

    // data from map
    int enemyId;
    bool enemyLoaded = false;
    MapGrid mapGrid;
    Sprite backgroundImage = null;

    // state
    bool actionDisabled = true;
    string whoseTurn = "player";
        // possible values: player, enemy
    bool skipEndTurnDiscard;
        // if true, skip discard and set to false
    int playerDodgeMapBuff = 0;
    int enemyVulnerability = 0;
    int playerCritMapBuff = 0;
    int playerHandSizeBuff = 0;
    int playerDefenseBuff = 0;
    int enemyHandSizeDebuff = 0;
    int enemyFizzleChance = 0;
    int percentDamageToEnemy = 0;
    int dotDamageToEnemy = 0;
    int enemyDamageDebuff = 0;

    // buffs from powerups
    int extraCardChanceFromMap = 0;

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
        // Set this to 0 so it doesn't carry over from battle to battle if unconsumed
        FindObjectOfType<CharacterData>().SetExtraDamageMultiplier(0);

        SetBackgroundImage();
        playerHand = FindObjectOfType<PlayerHand>();
        deck = FindObjectOfType<Deck>();
        discard = FindObjectOfType<Discard>();

        // square will only be null when testing battles directly, so we just set it to a default enemy
        if (!enemyLoaded)
        {
            SetUpEnemy(0);
        } else
        {
            SetUpEnemy(enemyId);
        }
        DealMapDamageToEnemy();
        enemy.BattleSetup(setupTimeInSeconds);

        configData = FindObjectOfType<ConfigData>();
        configData.SetupStatusEffectHolders();

        character.BattleSetup(setupTimeInSeconds);

        List<int> deckCardIds = character.GetLoadout().GetAllCardIds();
        deckCardIds.AddRange(hacker.GetHackerLoadout().GetCardIds());
        deck.SetupDeck(deckCardIds);

        playerHand.DrawStartingHand(character.GetStartingHandSize(), setupTimeInSeconds);

        StartCoroutine(EnablePlayAfterSetup());
    }

    private void DealMapDamageToEnemy()
    {
        enemy.TakePercentDamageFromMap(percentDamageToEnemy);
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
                    enemy.StartTurn();
                }
            }
        } else if (whoseTurn == "enemy") {
            if (deck.GetCardCount() > 0 || discard.GetCardCount() > 0)
            {
                if (deck.GetCardCount() <= character.GetStartingHandSize() - playerHand.GetCardsInHandCount())
                {
                    discard.ShuffleDiscardIntoDeck();
                }
                playerHand.DrawToMaxHandSize();
            }
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

    public void SetCharacterData(CharacterData characterToSet, HackerData hackerToSet)
    {
        character = characterToSet;
        hacker = hackerToSet;
    }

    public void GetDataFromMapSquare(MapSquare newCurrentSquare)
    {
        mapSquare = newCurrentSquare;
        enemyId = GetEnemyIDFromMapSquare(newCurrentSquare);
        enemyLoaded = true;
    }

    public void GetDataFromMapData(MapData mapData)
    {
        extraCardChanceFromMap = mapData.GetHandSizeBoostChance();
    }

    private int GetEnemyIDFromMapSquare(MapSquare square)
    {
        Enemy prefabEnemy = square.GetEnemy();
        Vector3 dummyPosition = new Vector3(-100, -100, -100);
        Enemy dummyEnemy = Instantiate(prefabEnemy, dummyPosition, Quaternion.identity);
        int enemyId = dummyEnemy.GetEnemyId();
        Destroy(dummyEnemy);
        return enemyId;
    }

    public void SetMapGrid(MapGrid newMapGrid)
    {
        mapGrid = newMapGrid;
        mapGrid.gameObject.SetActive(false);
    }

    public void LoadModifiersFromMap(List<int> loadedMapSquareEffects)
    {
        // buff order: dodge, enemyVulnerability, playerCrit, playerHandSize, playerDefense, enemy hand size debuff
            // percentDamageToEnemy, dotDamageToEnemy, enemyDamageDebuff
        playerDodgeMapBuff = loadedMapSquareEffects[0];
        enemyVulnerability = loadedMapSquareEffects[1];
        playerCritMapBuff = loadedMapSquareEffects[2];
        playerHandSizeBuff = loadedMapSquareEffects[3];
        playerDefenseBuff = loadedMapSquareEffects[4];
        enemyHandSizeDebuff = loadedMapSquareEffects[5];
        enemyFizzleChance = loadedMapSquareEffects[6];
        percentDamageToEnemy = loadedMapSquareEffects[7];
        dotDamageToEnemy = loadedMapSquareEffects[8];
        enemyDamageDebuff = loadedMapSquareEffects[9];
    }

    public int GetPlayerDodgeMapBuff()
    {
        return playerDodgeMapBuff;
    }

    public int GetEnemyVulnerabilityMapDebuff()
    {
        return enemyVulnerability;
    }

    public int GetPlayerCritMapBuff()
    {
        return playerCritMapBuff;
    }

    public int GetPlayerHandMapBuff()
    {
        return playerHandSizeBuff;
    }

    public int GetPlayerDefenseBuff()
    {
        return playerDefenseBuff;
    }

    public int GetEnemyHandSizeDebuff()
    {
        return enemyHandSizeDebuff;
    }

    public int GetEnemyFizzleChance()
    {
        return enemyFizzleChance;
    }

    public int GetMapDotDamage()
    {
        return dotDamageToEnemy;
    }

    public int GetEnemyDamageDebuff()
    {
        return enemyDamageDebuff;
    }

    public MapGrid GetMapGrid()
    {
        return mapGrid;
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

    public MapSquare GetMapSquare()
    {
        return mapSquare;
    }

    public void SetBackgroundImage()
    {
        BattleBackground battleBackground = FindObjectOfType<BattleBackground>();
        if (mapSquare)
        {
            battleBackground.SetImage(mapSquare.GetLocationImage());
        } else
        {
            battleBackground.SetImage(Resources.Load<Sprite>("LocationImages/City/Location1"));
        }
    }

    public int GetExtraCardDrawFromMapChance()
    {
        return extraCardChanceFromMap;
    }
}
