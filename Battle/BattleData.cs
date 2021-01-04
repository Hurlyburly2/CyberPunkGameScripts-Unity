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
    int cardsPlayedThisTurn = 0;
    int weaponsPlayedThisTurn = 0;
    int cardsDrawnThisTurn = 0;

    // effects from traps
    MapObject.TrapTypes trapType;
    int trapAmount;

    // buffs from powerups
    List<PowerUp> powerUps = new List<PowerUp>();
    int personalShieldStacks = 0;

    // buffs for "when a card is played"
    public enum PlayerOnPlayedEffects { PlayWeaponDrawCard };
    private List<PlayerOnPlayedEffects> playerOnPlayedEffects = new List<PlayerOnPlayedEffects>();

    // misc buffs/debuffs
    bool canDrawExtraCards = true;
    bool hasStanceBeenPlayed = false;

    // list that resets every turn. Keywords in this list are disallowed from play.
    List<string> prohibitedKeywordsFromPlay = new List<string>();

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
        character.SetExtraDamageMultiplier(0);

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

        List<int> deckCardIds = new List<int>();
        if (trapType == MapObject.TrapTypes.ParalysisAgent)
        {
            // Paralysis Agent Trap: Blocks arm and leg mods
            List<Item.ItemTypes> blockedItemTypes = new List<Item.ItemTypes>{ Item.ItemTypes.Arm, Item.ItemTypes.Leg };
            deckCardIds.AddRange(character.GetLoadout().GetAllCardIds(blockedItemTypes));
        } else
        {
            deckCardIds.AddRange(character.GetLoadout().GetAllCardIds());
        }

        // Do not load hacker cards if triggered a Faraday Cage trap
        if (trapType != MapObject.TrapTypes.FaradayCage)
            deckCardIds.AddRange(hacker.GetHackerLoadout().GetCardIds());

        deck.SetupDeck(deckCardIds);

        playerHand.DrawStartingHand(character.GetStartingHandSize(), setupTimeInSeconds);

        SetupStartingBuffs();
        PlayerStartTurnChecks();

        StartCoroutine(EnablePlayAfterSetup());
    }

    private void DealMapDamageToEnemy()
    {
        enemy.TakePercentDamageFromMap(percentDamageToEnemy);
    }

    public void SetUpEnemy(int enemyId)
    {
        ConfigData configData = FindObjectOfType<ConfigData>();

        float enemyXPos = Camera.main.transform.position.x;
        float enemyYPos = configData.GetHalfHeight() + configData.GetHalfHeight() * .66f;
        enemy = Instantiate(Resources.Load<Enemy>("Enemies/Enemy" + enemyId.ToString()), new Vector2(enemyXPos, enemyYPos), Quaternion.identity);
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
                    // reset some stuff:
                    cardsPlayedThisTurn = 0;
                    weaponsPlayedThisTurn = 0;
                    playerOnPlayedEffects = new List<PlayerOnPlayedEffects>();
                    canDrawExtraCards = true;
                    hasStanceBeenPlayed = false;
                    cardsDrawnThisTurn = 0;
                    prohibitedKeywordsFromPlay = new List<string>();
                    playerHand.ResetFinishedDrawingStartOfTurnCards();

                    TickDownStatusEffectDurations("Enemy");
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
            PlayerStartTurnChecks();
            TickDownStatusEffectDurations("player");
            whoseTurn = "player";
            actionDisabled = false;
        }
    }

    private void PlayerStartTurnChecks()
    {
        // Energy Siphon
        int energySiphon = GetCombinedAmountFromPowerUps(GetPowerUpsOfType(PowerUp.PowerUpType.EnergySiphon));
        if (energySiphon > 0)
        {
            character.GainEnergy(energySiphon);
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
        trapType = mapSquare.GetTriggeredTrapType();
        trapAmount = mapSquare.GetTriggeredTrapAmount();
    }

    public void GetPowerUpDataFromMap(MapData mapData)
    {
        powerUps.AddRange(mapData.GetPowerUps());
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

    public void CountPlayedCard(Card card)
    {
        cardsPlayedThisTurn++;
        List<string> keywords = new List<string>(card.GetKeywords());
        if (keywords.Contains("Weapon"))
            weaponsPlayedThisTurn++;
    }

    public int GetCardsPlayedThisTurn()
    {
        return cardsPlayedThisTurn;
    }

    public int GetWeaponCardsPlayedThisTurn()
    {
        return weaponsPlayedThisTurn;
    }

    public void GainPlayerOnPlayedEffect(PlayerOnPlayedEffects newEffect)
    {
        playerOnPlayedEffects.Add(newEffect);
    }

    public void CheckOnPlayedEffects(Card card)
    {
        List<string> cardKeywords = new List<string>();
        cardKeywords.AddRange(card.GetKeywords());

        // Draw cards on play weapon
        if (playerOnPlayedEffects.Contains(PlayerOnPlayedEffects.PlayWeaponDrawCard) && cardKeywords.Contains("Weapon"))
        {
            int amountToDraw = CountInstanceOfOnPlayedEffect(PlayerOnPlayedEffects.PlayWeaponDrawCard);
            playerHand.TriggerAcceleration(amountToDraw);
            playerHand.DrawXCards(amountToDraw);
        }
    }

    private int CountInstanceOfOnPlayedEffect(PlayerOnPlayedEffects effectToCount)
    {
        int count = 0;
        foreach (PlayerOnPlayedEffects effect in playerOnPlayedEffects)
        {
            if (effect == effectToCount)
                count++;
        }
        return count;
    }

    public void InflictCannotDrawExtraCardsDebuff()
    {
        canDrawExtraCards = false;
    }

    public bool CanPlayerDrawExtraCards()
    {
        return canDrawExtraCards;
    }

    public void SetPlayedStance()
    {
        hasStanceBeenPlayed = true;
    }

    public bool GetHasStanceBeenPlayed()
    {
        return hasStanceBeenPlayed;
    }

    public void AddToDrawnCardCount(int amount)
    {
        cardsDrawnThisTurn += amount;
    }

    public int GetDrawnCardCount()
    {
        return cardsDrawnThisTurn;
    }

    public void AddToListOfProhibitedCards(string keyword)
    {
        prohibitedKeywordsFromPlay.Add(keyword);
    }

    public bool DoesCardContainProhibitedKeywords(Card card)
    {
        List<string> cardKeywords = new List<string>(card.GetKeywords());

        // Need to never prohibit the playing of a Weakness card
        if (cardKeywords.Contains("Weakness"))
            return false;

        foreach (string cardKeyword in cardKeywords)
        {
            if (prohibitedKeywordsFromPlay.Contains(cardKeyword))
                return true;
        }
        return false;
    }

    private void SetupStartingBuffs()
    {
        string playerOrEnemy = "player";

        // ELEMENT OF SURPRISE
        StatusEffectHolder playerCurrentStatusEffects = configData.GetPlayerStatusEffects();
        int startingMomentum = GetCombinedAmountFromPowerUps(GetPowerUpsOfType(PowerUp.PowerUpType.ElementOfSurprise));
        if (startingMomentum > 0)
            playerCurrentStatusEffects.InflictStatus(StatusEffect.StatusType.Momentum, startingMomentum, playerOrEnemy);

        // PERSONAL SHIELD
        personalShieldStacks = GetCombinedAmountFromPowerUps(GetPowerUpsOfType(PowerUp.PowerUpType.PersonalShield));

        // THE BEST OFFENSE
        int startingRetaliation = GetCombinedAmountFromPowerUps(GetPowerUpsOfType(PowerUp.PowerUpType.TheBestOffense));
        if (startingRetaliation > 0)
            playerCurrentStatusEffects.InflictStatus(StatusEffect.StatusType.Retaliate, startingRetaliation, playerOrEnemy, 99999);
    }

    private int GetCombinedAmountFromPowerUps(List<PowerUp> powerUps)
    {
        int count = 0;
        foreach (PowerUp powerUp in powerUps)
        {
            count += powerUp.GetAmount();
        }
        return count;
    }

    public List<PowerUp> GetPowerUpsOfType(PowerUp.PowerUpType powerUpType)
    {
        List<PowerUp> foundPowerUps = new List<PowerUp>();
        foreach (PowerUp powerUp in powerUps)
        {
            if (powerUp.GetPowerUpType() == powerUpType)
                foundPowerUps.Add(powerUp);
        }
        return foundPowerUps;
    }

    public int GetPersonalShieldStacks()
    {
        return personalShieldStacks;
    }

    public void ConsumePersonalShieldStack()
    {
        personalShieldStacks--;
    }

    public MapObject.TrapTypes GetTrapType()
    {
        return trapType;
    }

    public int GetTrapAmount()
    {
        return trapAmount;
    }
}
