using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] int cardId;
    [SerializeField] string[] keywords;
    [SerializeField] int energyCost = 0;
    [SerializeField] CardHelperText[] cardHelperTexts;
    [SerializeField] GameObject energyCostzone;
    ConfigData configData;
    BattleData battleData;
    PlayerHand playerHand;
    StatusEffectHolder playerCurrentStatusEffects;
    StatusEffectHolder enemyCurrentStatusEffects;
    Deck deck;
    Discard discard;

    [SerializeField] TextMeshProUGUI cardText;
    [SerializeField] SpriteRenderer leftCircuit;
    [SerializeField] SpriteRenderer topCircuit;
    [SerializeField] SpriteRenderer rightCircuit;
    [SerializeField] SpriteRenderer bottomCircuit;

    float xPos;
    float yPos;
    float zPos;
    float rotation;
    float handAdjustSpeed = 20f;

    int rememberSortingOrder = 0;
    float rememberRotation = 0;
    float critModifier = 2;

    // where is the card, and how is it behaving?
        // draw = when card is initially drawn
        // hand = drawn card reaches hand
        // dragging = mouse down, can move it from hand
        // played = card dragged to play zone (above a certain y axis)
    string state;
    string playerOrEnemy = "player";
        // tracks if the player or enemy is using the card, used for status effects

    // Start is called before the first frame update
    void Awake()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
        configData = FindObjectOfType<ConfigData>();
        battleData = FindObjectOfType<BattleData>();
        playerHand = FindObjectOfType<PlayerHand>();
        discard = FindObjectOfType<Discard>();
        deck = FindObjectOfType<Deck>();
        playerCurrentStatusEffects = configData.GetPlayerStatusEffects();
        enemyCurrentStatusEffects = configData.GetEnemyStatusEffects();
        if (energyCost > 0)
        {
            energyCostzone.SetActive(true);
            energyCostzone.GetComponentInChildren<TextMeshProUGUI>().text = energyCost.ToString();
        }
        WhenDrawnActions();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "draw" || state == "hand")
        {
            MoveTowardTarget(xPos, yPos);
            AdjustRotation();
            DoesDrawnCardReachHand();
        } else if (state == "dragging")
        {
            float mouseX = Input.mousePosition.x / Screen.width * configData.GetHalfWidth() * 2 / configData.GetCardSizeMultiplier();
            float mouseY = Input.mousePosition.y / Screen.height * configData.GetHalfHeight() * 2;

            MoveTowardTarget(mouseX, mouseY);
            AdjustRotation();
            DoesDrawnCardReachHand();
        }
    }

    private void OnMouseDown()
    {
        if (!battleData.AreActionsDisabled())
        {
            SetState("dragging");
        }
    }

    private void OnMouseUp()
    {
        if (state == "dragging")
        {
            CharacterData runner = FindObjectOfType<BattleData>().GetCharacter();
            RemoveHelperText();
            rotation = rememberRotation;
            SetSortingOrder(rememberSortingOrder);
            float mouseY = Input.mousePosition.y / Screen.height * configData.GetHalfHeight() * 2;

            if (mouseY > configData.GetCardPlayedLine() && battleData.WhoseTurnIsIt() == "player" && CanPlayCard())
            {
                runner.SpendEnergy(energyCost);
                SetState("played");
                playerHand.RemoveCard(GetComponent<Card>());
                PlayCard();
            } else
            {
                transform.localScale = new Vector3(1, 1, 1);
                SetState("draw");
                List<string> keywordList = new List<string>(keywords);

                if (mouseY > configData.GetCardPlayedLine() && battleData.WhoseTurnIsIt() == "player" && energyCost > runner.GetCurrentEnergy())
                {
                    // Here, we did attempt to play the card but did not have enough energy...
                    FindObjectOfType<PopupHolder>().SpawnNotEnoughEnergyPopup();
                } else if (keywordList.Contains("Stance") && battleData.GetHasStanceBeenPlayed()) {
                    FindObjectOfType<PopupHolder>().SpawnStancePopup();
                } else if (battleData.DoesCardContainProhibitedKeywords(this))
                {
                    FindObjectOfType<PopupHolder>().SpawnCouldNotPlayPopup();
                }
            }
        }
    }

    private bool CanPlayCard()
    {
        CharacterData runner = battleData.GetCharacter();
        List<string> keywordList = new List<string>(keywords);

        // Check for not enough energy
        if (energyCost > runner.GetCurrentEnergy())
            return false;
        // Check if card is a stance AND a stance has already been played
        if (keywordList.Contains("Stance") && battleData.GetHasStanceBeenPlayed())
            return false;
        if (battleData.DoesCardContainProhibitedKeywords(this))
            return false;
        return true;
    }

    public void PlayCard()
    {
        // A card fizzling removes it from your hand... Weaknesses cannot Fizzle
        if (playerCurrentStatusEffects.GetFizzleChance() > 0)
        {
            List<string> checkKeywords = new List<string>();
            checkKeywords.AddRange(keywords);
            if (PercentChance(playerCurrentStatusEffects.GetFizzleChance()) && !checkKeywords.Contains("Weakness"))
            {
                FindObjectOfType<PopupHolder>().SpawnFizzledPopup();
                DiscardCard();
                playerHand.RemoveFromHand(this);
                Destroy(gameObject);
                return;
            }
        }

        // Card played successfully
        CheckOnPlayedEffects();
        battleData.CountPlayedCard(this);

        List<string> keywordList = new List<string>(keywords);
        if (keywordList.Contains("Stance"))
            battleData.SetPlayedStance();

        bool furtherAction = PlayCardActions();
        DiscardCard();
        playerHand.RemoveFromHand(this);
        if (!furtherAction)
        {
            Destroy(gameObject);
        }
    }

    public void DiscardFromHand()
    {
        DiscardCard();
        playerHand.RemoveFromHand(this);
        Destroy(gameObject);
    }

    private void DiscardCard()
    {
        Card cardPrefab = configData.GetCardPrefabById(cardId);
        discard.AddCardToDiscard(cardPrefab);
    }

    private void DoesDrawnCardReachHand()
    {
        if (transform.position == new Vector3(xPos, yPos, transform.position.z))
        {
            SetState("hand");
        }
    }

    private void MoveTowardTarget(float targetX, float targetY)
    {
        float step = handAdjustSpeed * Time.deltaTime;

        // we make the card movement faster as the hand size increases to fight the slowdown
        step = ScaleStepByPlayerHandSize(step);
        Vector3 newPosition = new Vector3(targetX, targetY, zPos);

        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
    }

    private float ScaleStepByPlayerHandSize(float step)
    {
        float cardsInHandCount = playerHand.GetCardsInHandCount();

        if (cardsInHandCount < 6)
        {
            return step;
        }

        float modifier = 1;
        cardsInHandCount -= 5;
        cardsInHandCount *= Mathf.Clamp(cardsInHandCount * 0.05f, 1, 1.15f);
        modifier *= cardsInHandCount;

        return step * modifier;
    }

    private void AdjustRotation()
    {
        Quaternion target = Quaternion.Euler(0f, 0f, rotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * handAdjustSpeed);
    }

    public void SetSortingOrder(int newSortingOrder)
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Canvas canvas = GetComponentInChildren<Canvas>();

        rememberSortingOrder = spriteRenderers[0].sortingOrder;
        int currentSortingOrder = newSortingOrder;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingOrder = currentSortingOrder;
            currentSortingOrder++;
            if (spriteRenderer.name == "Image" && canvas != null)
            {
                canvas.sortingOrder = currentSortingOrder;
                currentSortingOrder++;
            }
        }
    }

    public void SetState(string state)
    {
        this.state = state;
        if (this.state == "draw")
        {
            handAdjustSpeed = configData.GetHandDrawSpeed();
        } else if (state == "hand")
        {
            handAdjustSpeed = configData.GetHandAdjustSpeed();
        } else if (state == "dragging")
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            rememberRotation = rotation;
            rotation = 0;
            SetSortingOrder(1000);
            handAdjustSpeed = 1000f;
            StartCoroutine(DisplayHelperText());
        }
    }

    private IEnumerator DisplayHelperText()
    {
        yield return new WaitForSeconds(1);
        if (state == "dragging")
        {
            List<string> keywordsToDefine = GetKeywordsToDefine();
            for(int i = 0; i < cardHelperTexts.Length; i++)
            {
                if (i < keywordsToDefine.Count)
                {
                    cardHelperTexts[i].Activate(keywordsToDefine[i]);
                }
            }
        } else
        {
            Debug.Log("Do not display helper text");
        }
    }

    private void RemoveHelperText()
    {
        foreach(CardHelperText cardHelperText in cardHelperTexts)
        {
            cardHelperText.Deactivate();
        }
    }

    private List<string> GetKeywordsToDefine()
    {
        List<string> keywordsToDefine = new List<string>();
        foreach(string keyword in keywords)
        {
            bool needsDefinition = false;
            switch(keyword)
            {
                case "Damage Resist":
                    needsDefinition = true;
                    break;
                case "Dodge":
                    needsDefinition = true;
                    break;
                case "Momentum":
                    needsDefinition = true;
                    break;
                case "Vulnerable":
                    needsDefinition = true;
                    break;
                case "Weakness":
                    needsDefinition = true;
                    break;
                case "Acceleration":
                    needsDefinition = true;
                    break;
                case "Stance":
                    needsDefinition = true;
                    break;
            }
            if (needsDefinition)
            {
                keywordsToDefine.Add(keyword);
            }
        }
        return keywordsToDefine;
    }

    public void SetCardPosition(float xPos)
    {
        this.xPos = xPos;
    }

    public void SetCardPosition(float xPos, float yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
    }

    public void SetCardPosition(float xPos, float yPos, float rotation)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.rotation = rotation;
    }

    public float GetXPos()
    {
        return xPos;
    }

    public void SetZPos(float zPos)
    {
        this.zPos = zPos;
    }

    public int GetCardId()
    {
        return cardId;
    }

    public string[] GetKeywords()
    {
        return keywords;
    }

    private void WhenDrawnActions()
    {
        switch (cardId)
        {
            case 83: // Radar Ghost 1
                GainStatus(StatusEffect.StatusType.FizzleChance, 75);
                break;
            case 84: // Radar Ghost 2
                GainStatus(StatusEffect.StatusType.FizzleChance, 50);
                break;
            case 85: // Radar Ghost 3
                GainStatus(StatusEffect.StatusType.FizzleChance, 35);
                break;
            case 86: // Radar Ghost 4
                GainStatus(StatusEffect.StatusType.FizzleChance, 20);
                break;
            case 131: // JAMMED SERVOS 1
            case 132: // JAMMED SERVOS 2
            case 133: // JAMMED SERVOS 3
            case 134: // JAMMED SERVOS 4
                battleData.InflictCannotDrawExtraCardsDebuff();
                break;
            case 179: // MISFIRE 1
                if (playerHand.GetIsInitialHandDrawFinished())
                    SelfDamage(8);
                break;
            case 180: // MISFIRE 2
                if (playerHand.GetIsInitialHandDrawFinished())
                    SelfDamage(6);
                break;
            case 181: // MISFIRE 3
                if (playerHand.GetIsInitialHandDrawFinished())
                    SelfDamage(4);
                break;
            case 182: // MISFIRE 4
                if (playerHand.GetIsInitialHandDrawFinished())
                    SelfDamage(2);
                break;
            case 204: // RELOAD 1
            case 205: // RELOAD 2
            case 206: // RELOAD 3
            case 207: // RELOAD 4
            case 208: // RELOAD 5
                battleData.AddToListOfProhibitedCards("Weapon");
                break;
        }
    }

    private bool PlayCardActions()
    {
        // Return true if further actions need to happen
        switch (cardId)
        {
            case 0: // DUMMY CARD
                Debug.Log("This is a dummy card, it doesn't actually do anything");
                break;
            case 1: // AWARENESS 1
                GainStatus(StatusEffect.StatusType.Dodge, 1);
                break;
            case 2: // OBSERVE
                LoadCardPicker(deck.GetTopXCardsWithoutDraw(3), 1);
                break;
            case 3: // DEEP BREATH
                DrawXCards(5);
                GainStatus(StatusEffect.StatusType.Vulnerable, 1);
                SkipEndTurnDiscard(true);
                EndTurn();
                break;
            case 4: // WEAK SPOT 1
                GainExtraDamageModifier(2f);
                break;
            case 5: // SHAKE OFF
                GainHealth(2);
                HealDebuff(1);
                break;
            case 6: // BRACE
                GainStatus(StatusEffect.StatusType.DamageResist, 1, 2);
                break;
            case 7: // PUNCH
                DealDamage(2);
                if (PercentChance(10)) {
                    DrawXCards(1);
                }
                break;
            case 8: // QUICKDRAW
                DrawRandomCardFromDeck("Weapon");
                break;
            case 9: // KICK
                DealDamage(1);
                GainStatus(StatusEffect.StatusType.Momentum, 1);
                break;
            case 10: // SPRINT
                DrawXCards(2);
                break;
            case 11: // WHACK
                DealDamage(3, 10);
                break;
            case 12: // KNEECAP
                DealDamage(4);
                break;
            case 13: // BRUISE
                DealDamage(1);
                InflictStatus(StatusEffect.StatusType.Vulnerable, 2);
                break;
            case 14: // HIDDEN TRIGGER
                List<int> cardsToAddIds = new List<int>();
                cardsToAddIds.Add(3);
                ShuffleCardsIntoEnemyDeck(cardsToAddIds);
                break;
            case 15: // TOO OBVIOUS
                int removedTraps = FindObjectOfType<EnemyDeck>().RemoveAllVirusCards();
                SelfDamage(removedTraps);
                break;
            case 16: // QWIKTHINK
                DrawXCards(1);
                break;
            case 17: // AD-HOC UPGRADE
                DrawXCards(1);
                SelfDamage(2);
                GainEnergy(2);
                break;
            case 18: // FAILED CONNECTION
                GainHandDebuff(1);
                GainEnergy(2);
                break;
            case 19: // CRACKED
                PowerupStatus("buffs", 1, 1);
                break;
            case 20: // AWARENESS 2
                GainStatus(StatusEffect.StatusType.Dodge, 2);
                break;
            case 21: // AWARENESS 3
                GainStatus(StatusEffect.StatusType.Dodge, 3);
                break;
            case 22: // OBSERVE 2
            case 23: // OBSERVE 3
                LoadCardPicker(deck.GetTopXCardsWithoutDraw(4), 1);
                break;
            case 24: // OBSERVE 4
                LoadCardPicker(deck.GetTopXCardsWithoutDraw(5), 1);
                break;
            case 25: // OBSERVE 5
                LoadCardPicker(deck.GetTopXCardsWithoutDraw(5), 2);
                break;
            case 26: // DEEP BREATH 2
                DrawXCards(6);
                GainStatus(StatusEffect.StatusType.Vulnerable, 1);
                SkipEndTurnDiscard(true);
                EndTurn();
                break;
            case 27: // DEEP BREATH 3
                DrawXCards(6);
                SkipEndTurnDiscard(true);
                EndTurn();
                break;
            case 28: // DEEP BREATH 4
                DrawXCards(6);
                SkipEndTurnDiscard(true);
                StartCoroutine(WaitForSomethingToFinish("drawingCards"));
                // After all cards draw, discard the weaknesses
                return true;
            case 29: // WEAK SPOT 2
                GainExtraDamageModifier(1.5f);
                break;
            case 30: // WEAK SPOT 3
                GainExtraDamageModifier(1.25f);
                break;
            case 31: // SHAKE OFF 2
                GainHealth(Random.Range(2, 4));
                HealDebuff(1);
                break;
            case 32: // SHAKE OFF 3
                GainHealth(Random.Range(2, 5));
                HealDebuff(1);
                break;
            case 33: // SHAKE OFF 4
                GainHealth(Random.Range(3, 6));
                HealDebuff(1);
                break;
            case 34: // SHAKE OFF 5
                GainHealth(Random.Range(3, 7));
                HealDebuff(1);
                break;
            case 35: // BRACE 2
                GainStatus(StatusEffect.StatusType.DamageResist, 1, 2);
                DrawXCards(1);
                break;
            case 36: // BRACE 3
                GainStatus(StatusEffect.StatusType.DamageResist, 1, 2);
                DrawXCards(1);
                break;
            case 37: // BRACE 4
                GainStatus(StatusEffect.StatusType.DamageResist, 2, 2);
                DrawXCards(1);
                break;
            case 38: // PUNCH 2
                DealDamage(2);
                if (PercentChance(15))
                {
                    DrawXCards(1);
                }
                break;
            case 39: // PUNCH 3
                DealDamage(3);
                if (PercentChance(15))
                {
                    DrawXCards(1);
                }
                break;
            case 40: // PUNCH 4
                DealDamage(3);
                if (PercentChance(20))
                {
                    DrawXCards(1);
                }
                break;
            case 41: // PUNCH 5
                DealDamage(3);
                if (PercentChance(25))
                {
                    DrawXCards(1);
                }
                break;
            case 42: // QUICKDRAW 2
            case 43: // QUICKDRAW 3
                DealDamage(1);
                DrawRandomCardFromDeck("Weapon");
                break;
            case 44:
                DealDamage(1);
                DrawRandomCardFromDeck("Weapon");
                DrawRandomCardFromDeck("Weapon");
                break;
            case 45:
            case 46:
                DealDamage(2);
                GainStatus(StatusEffect.StatusType.Momentum, 1);
                break;
            case 47:
                DealDamage(2);
                GainStatus(StatusEffect.StatusType.Momentum, 2);
                break;
            case 48:
                DealDamage(3);
                GainStatus(StatusEffect.StatusType.Momentum, 2);
                break;
            case 49:
                DrawXCards(2);
                break;
            case 50:
                DrawXCards(2);
                GainHandBuff(1);
                break;
            case 51:
                DrawXCards(3);
                GainHandBuff(1);
                break;
            case 52: // WHACK 2
                DealDamage(3, 15);
                break;
            case 53: // WHACK 3
                DealDamage(3, 20);
                break;
            case 54: // WHACK 4
                DealDamage(3, 25);
                break;
            case 55: // WHACK 5
                DealDamage(4, 25);
                break;
            case 56: // KNEECAP 2
                DealDamage(Random.Range(4, 6));
                break;
            case 57: // KNEECAP 3
                DealDamage(Random.Range(4, 6));
                InflictStatus(StatusEffect.StatusType.Vulnerable, 1);
                break;
            case 58: // KNEECAP 4
                DealDamage(Random.Range(5, 7));
                InflictStatus(StatusEffect.StatusType.Vulnerable, 1);
                break;
            case 59: // KNEECAP 5
                DealDamage(Random.Range(5, 9));
                InflictStatus(StatusEffect.StatusType.Vulnerable, 1);
                break;
            case 60: // BRUISE 2
            case 61: // BRUISE 3
                DealDamage(2);
                InflictStatus(StatusEffect.StatusType.Vulnerable, 2);
                break;
            case 62: // BRUISE 4
                DealDamage(2);
                InflictStatus(StatusEffect.StatusType.Vulnerable, 3);
                break;
            case 63: // BRUISE 5
                DealDamage(3);
                InflictStatus(StatusEffect.StatusType.Vulnerable, 3);
                break;
            case 64: // HIDDEN TRIGGER 2
                cardsToAddIds = new List<int>();
                cardsToAddIds.Add(3);
                ShuffleCardsIntoEnemyDeck(cardsToAddIds);
                GainStatus(StatusEffect.StatusType.Dodge, 1);
                break;
            case 65: // HIDDEN TRIGGER 3
                cardsToAddIds = new List<int>();
                cardsToAddIds.Add(3);
                cardsToAddIds.Add(3);
                ShuffleCardsIntoEnemyDeck(cardsToAddIds);
                GainStatus(StatusEffect.StatusType.Dodge, 1);
                break;
            case 66: // TOO OBVIOUS 2
                FindObjectOfType<EnemyDeck>().RemoveAllVirusCards();
                break;
            case 67: // QUIKTHINK 2
                DrawXCards(1);
                if (PercentChance(50))
                {
                    DrawXCards(1);
                }
                break;
            case 68: // QUICKTHINK 3
                DrawXCards(2);
                break;
            case 69: // AD-HOC UPGRADE 2
                DrawXCards(1);
                SelfDamage(1);
                GainEnergy(2);
                break;
            case 70: // AD-HOC UPGRADE 3
                DrawXCards(1);
                GainEnergy(3);
                break;
            case 71: // CRACKED 2
                PowerupStatus("buffs", 2, 1);
                break;
            case 72: // CRACKED 3
                PowerupStatus("buffs", 3, 1);
                break;
            case 73: // QUICK TARGETTING 1
                GainStatus(StatusEffect.StatusType.Momentum, 1);
                DrawXCards(1);
                break;
            case 74: // Quick Targetting 1
            case 75: // Quick Targetting 2
                GainStatus(StatusEffect.StatusType.Momentum, 2);
                DrawXCards(1);
                break;
            case 76: // Quick Targeting 3
                GainStatus(StatusEffect.StatusType.Momentum, 2);
                DrawXCards(2);
                break;
            case 77: // Quick TARGETING 4
                GainStatus(StatusEffect.StatusType.Momentum, 3);
                DrawXCards(2);
                break;
            case 78: // PINPOINT ACCURACY 1
                InflictStatus(StatusEffect.StatusType.Vulnerable, 1);
                break;
            case 79: // PINPOINT ACCURACY 2
                InflictStatus(StatusEffect.StatusType.Vulnerable, 1);
                GainStatus(StatusEffect.StatusType.AutoCrit, 1);
                break;
            case 80: // PINPOINT ACCURACY 3
                InflictStatus(StatusEffect.StatusType.Vulnerable, 2);
                GainStatus(StatusEffect.StatusType.AutoCrit, 1);
                break;
            case 81: // PINPOINT ACCURACY 4
                InflictStatus(StatusEffect.StatusType.Vulnerable, 3);
                GainStatus(StatusEffect.StatusType.AutoCrit, 1);
                break;
            case 82: // PINPOINT ACCURACY 5
                InflictStatus(StatusEffect.StatusType.Vulnerable, 3);
                GainStatus(StatusEffect.StatusType.AutoCrit, 2);
                break;
            case 83: // RADAR GHOST 1
            case 84: // RADAR GHOST 2
            case 85: // RADAR GHOST 3
            case 86: // RADAR GHOST 4
                Debug.Log("Doesn't do anything when played.");
                break;
            case 87: // STIM INJECTION 1
                GainEnergy(1);
                DrawXCards(1);
                GainStatus(StatusEffect.StatusType.CritChance, 5);
                break;
            case 88: // STIM INJECTION 2
                GainEnergy(2);
                DrawXCards(1);
                GainStatus(StatusEffect.StatusType.CritChance, 10);
                break;
            case 89: // STIM INJECTION 3
                GainEnergy(2);
                DrawXCards(1);
                GainStatus(StatusEffect.StatusType.CritChance, 15);
                break;
            case 90: // STIM INJECTION 4
                GainEnergy(3);
                DrawXCards(2);
                GainStatus(StatusEffect.StatusType.CritChance, 20);
                break;
            case 91: // STIM INJECTION 5
                GainEnergy(3);
                DrawXCards(2);
                GainStatus(StatusEffect.StatusType.CritChance, 25);
                break;
            case 92: // RAISE HEARTRATE 1
                GainEnergy(1);
                DrawXCards(1);
                break;
            case 93: // RAISE HEARTRATE 2
                GainEnergy(2);
                DrawXCards(1);
                break;
            case 94: // RAISE HEARTRATE 3
            case 95: // RAISE HEARTRATE 4
                GainEnergy(3);
                DrawXCards(1);
                break;
            case 96: // RAISE HEARTRATE 5
                GainEnergy(4);
                DrawXCards(1);
                break;
            case 97: // CARDIAC ARREST 1
                SelfDamage(5);
                GainHandDebuff(10000);
                break;
            case 98: // CARDIAC ARREST 2
                SelfDamage(4);
                GainHandDebuff(10000);
                break;
            case 99: // CARDIAC ARREST 3
                SelfDamage(3);
                GainHandDebuff(10000);
                break;
            case 100: // CARDIAC ARREST 4
                SelfDamage(2);
                GainHandDebuff(10000);
                break;
            case 101: // DEADEN SENSES 1
                HealDebuff(1);
                GainHealth(battleData.GetCardsPlayedThisTurn() / 4);
                break;
            case 102: // DEADEN SENSES 2
            case 103: // DEADEN SENSES 3
                HealDebuff(2);
                GainHealth(battleData.GetCardsPlayedThisTurn() / 3);
                break;
            case 104: // DEADEN SENSES 4
                HealDebuff(3);
                GainHealth(battleData.GetCardsPlayedThisTurn() / 2);
                break;
            case 105: // DEADEN SENSES 5
                HealDebuff(4);
                GainHealth(battleData.GetCardsPlayedThisTurn() / 2);
                break;
            case 106: // HEIGHTENED RECEPTORS 1
                GainStatus(StatusEffect.StatusType.Vulnerable, 1);
                GainStatus(StatusEffect.StatusType.Dodge, 1);
                GainStatus(StatusEffect.StatusType.CritChance, 5);
                if (PercentChance(50))
                    DrawXCards(1);
                break;
            case 107: // HEIGHTENED RECEPTORS 2
                GainStatus(StatusEffect.StatusType.Vulnerable, 1);
                GainStatus(StatusEffect.StatusType.Dodge, 1);
                GainStatus(StatusEffect.StatusType.CritChance, 10);
                DrawXCards(1);
                break;
            case 108: // HEIGHTENED RECEPTORS 3
                GainStatus(StatusEffect.StatusType.Vulnerable, 2);
                GainStatus(StatusEffect.StatusType.Dodge, 1);
                GainStatus(StatusEffect.StatusType.CritChance, 15);
                DrawXCards(1);
                break;
            case 109: // HEIGHTENED RECEPTORS 4
                GainStatus(StatusEffect.StatusType.Vulnerable, 2);
                GainStatus(StatusEffect.StatusType.Dodge, 2);
                GainStatus(StatusEffect.StatusType.CritChance, 20);
                DrawXCards(1);
                break;
            case 110: // HEIGHTENED RECEPTORS 5
                GainStatus(StatusEffect.StatusType.Vulnerable, 3);
                GainStatus(StatusEffect.StatusType.Dodge, 3);
                GainStatus(StatusEffect.StatusType.CritChance, 25);
                DrawXCards(1);
                break;
            case 111: // SENSORY OVERLOAD 1
                GainStatus(StatusEffect.StatusType.Vulnerable, 4);
                break;
            case 112: // SENSORY OVERLOAD 2
                GainStatus(StatusEffect.StatusType.Vulnerable, 5);
                break;
            case 113: // SENSORY OVERLOAD 3
                GainStatus(StatusEffect.StatusType.Vulnerable, 6);
                break;
            case 114: // SENSORY OVERLOAD 4
                GainStatus(StatusEffect.StatusType.Vulnerable, 7);
                break;
            case 115: // ZEN CONTROL 1
                DrawXCards(1);
                int currentVulnerableStacks = playerCurrentStatusEffects.GetVulnerableStacks();
                GainStatus(StatusEffect.StatusType.Vulnerable, -currentVulnerableStacks);
                break;
            case 116: // LIGHTNING RELOAD 1
            case 117: // LIGHTNING RELOAD 2
            case 118: // LIGHTNING RELOAD 3
            case 119: // LIGHTNING RELOAD 4
            case 120: // LIGHTNING RELOAD 5
                battleData.GainPlayerOnPlayedEffect(BattleData.PlayerOnPlayedEffects.PlayWeaponDrawCard);
                break;
            case 121: // AUTO-UNHOLSTER 1
                DrawRandomCardFromDeck("Weapon");
                break;
            case 122: // AUTO-UNHOLSTER 2
                DrawRandomCardFromDeck("Weapon");
                GainEnergy(1);
                break;
            case 123: // AUTO-UNHOLSTER 3
                DrawRandomCardFromDeck("Weapon");
                DrawRandomCardFromDeck("Weapon");
                GainEnergy(1);
                break;
            case 124: // AUTO-UNHOLSTER 4
                DrawRandomCardFromDeck("Weapon");
                DrawRandomCardFromDeck("Weapon");
                GainEnergy(2);
                break;
            case 125: // AUTO-UNHOLSTER 5
                DrawRandomCardFromDeckOrDiscard("Weapon");
                DrawRandomCardFromDeckOrDiscard("Weapon");
                GainEnergy(2);
                break;
            case 126: // IMPLANTED QUIKBLADE 1
                DealDamage(1);
                if (PercentChance(10))
                    DrawXCards(1);
                break;
            case 127: // IMPLANTED QUIKBLADE 2
                DealDamage(2);
                if (PercentChance(20))
                    DrawXCards(1);
                break;
            case 128: // IMPLANTED QUIKBLADE 3
                DealDamage(2);
                if (PercentChance(30))
                    DrawXCards(1);
                break;
            case 129: // IMPLANTED QUIKBLADE 4
                DealDamage(3);
                if (PercentChance(40))
                    DrawXCards(1);
                break;
            case 130: // IMPLANTED QUIKBLADE 5
                DealDamage(3);
                if (PercentChance(60))
                    DrawXCards(1);
                break;
            case 131: // JAMMED SERVOS 1
                SelfDamage(3);
                break;
            case 132: // JAMMED SERVOS 2
                SelfDamage(4);
                break;
            case 133: // JAMMED SERVOS 3
                SelfDamage(4);
                break;
            case 134: // JAMMED SERVOS 4
                SelfDamage(3);
                break;
            case 135: // STABILIZED STANCE 1
                GainStatus(StatusEffect.StatusType.Momentum, 1);
                GainStatus(StatusEffect.StatusType.CritChance, 10);
                GainHandDebuff(1);
                break;
            case 136: // STABILIZED STANCE 2
                GainStatus(StatusEffect.StatusType.Momentum, 2);
                GainStatus(StatusEffect.StatusType.CritChance, 10);
                GainHandDebuff(1);
                break;
            case 137: // STABILIZED STANCE 3
                GainStatus(StatusEffect.StatusType.Momentum, 2);
                GainStatus(StatusEffect.StatusType.CritChance, 15);
                GainHandDebuff(1);
                break;
            case 138: // STABILIZED STANCE 4
                GainStatus(StatusEffect.StatusType.Momentum, 2);
                GainStatus(StatusEffect.StatusType.CritChance, 15);
                DrawRandomCardFromDiscard("Weapon");
                GainHandDebuff(1);
                break;
            case 139: // STABILIZED STANCE 5
                GainStatus(StatusEffect.StatusType.Momentum, 2);
                GainStatus(StatusEffect.StatusType.CritChance, 15);
                DrawRandomCardFromDiscard("Weapon");
                break;
            case 140: // PREPARED STANCE 1
                GainStatus(StatusEffect.StatusType.DamageResist, 1);
                GainEnergy(2);
                GainHandBuff(1);
                break;
            case 141: // PREPARED STANCE 2
                GainStatus(StatusEffect.StatusType.DamageResist, 1);
                GainEnergy(3);
                GainHandBuff(1);
                break;
            case 142: // PREPARED STANCE 3
                GainStatus(StatusEffect.StatusType.DamageResist, 1);
                GainEnergy(4);
                GainHandBuff(2);
                break;
            case 143: // PREPARED STANCE 4
                GainStatus(StatusEffect.StatusType.DamageResist, 2);
                GainEnergy(4);
                GainHandBuff(2);
                break;
            case 144: // PREPARED STANCE 5
                GainStatus(StatusEffect.StatusType.DamageResist, 2);
                GainEnergy(5);
                GainHandBuff(3);
                break;
            case 145: // NIMBLE STANCE 1
                GainStatus(StatusEffect.StatusType.Dodge, 1);
                DrawXCards(1);
                break;
            case 146: // NIMBLE STANCE 2
                GainStatus(StatusEffect.StatusType.Dodge, 1);
                DrawXCards(2);
                break;
            case 147: // NIMBLE STANCE 3
                GainStatus(StatusEffect.StatusType.Dodge, 2);
                DrawXCards(2);
                break;
            case 148: // NIMBLE STANCE 4
                GainStatus(StatusEffect.StatusType.Dodge, 2);
                DrawXCards(2);
                break;
            case 149: // NIMBLE STANCE 5
                GainStatus(StatusEffect.StatusType.Dodge, 2);
                DrawXCards(3);
                break;
            case 150: // READY FOR ANYTHING 1
            case 151: // READY FOR ANYTHING 2
            case 152: // READY FOR ANYTHING 3
            case 153: // READY FOR ANYTHING 4
            case 154: // READY FOR ANYTHING 5
                DrawAllOfTypeFromDeck("Stance");
                break;
            case 155: // MISSTEP 1
                GainStatus(StatusEffect.StatusType.Vulnerable, 2);
                GainHandDebuff(2);
                break;
            case 156: // MISSTEP 2
                GainStatus(StatusEffect.StatusType.Vulnerable, 3);
                GainHandDebuff(2);
                break;
            case 157: // MISSTEP 3
                GainStatus(StatusEffect.StatusType.Vulnerable, 4);
                GainHandDebuff(2);
                break;
            case 158: // MISSTEP 4
                GainStatus(StatusEffect.StatusType.Vulnerable, 5);
                GainHandDebuff(2);
                break;
            case 159: // BARRAGE 1
            case 160: // BARRAGE 2
                DealDamage(battleData.GetDrawnCardCount() / 2);
                break;
            case 161: // BARRAGE 3
            case 162: // BARRAGE 4
                DealDamage(battleData.GetDrawnCardCount());
                break;
            case 163: // BARRAGE 5
                int damage = battleData.GetDrawnCardCount();
                damage += battleData.GetCardsPlayedThisTurn() / 2;
                DealDamage(damage);
                break;
            case 164: // SHOOT 1
                DealDamage(1, 2 * battleData.GetCardsPlayedThisTurn());
                break;
            case 165: // SHOOT 2
                DealDamage(2, 3 * battleData.GetCardsPlayedThisTurn());
                break;
            case 166: // SHOOT 3
                DealDamage(2, 4 * battleData.GetCardsPlayedThisTurn());
                break;
            case 167: // SHOOT 4
                DealDamage(3, 4 * battleData.GetCardsPlayedThisTurn());
                break;
            case 168: // SHOOT 5
                DealDamage(3, 5 * battleData.GetCardsPlayedThisTurn());
                break;
            case 169: // HAIR TRIGGER 1
                DealDamage(1);
                DrawXCards(1);
                break;
            case 170: // HAIR TRIGGER 2
            case 171: // HAIR TRIGGER 3
                DealDamage(1);
                int cardsToDraw = 1;
                if (PercentChance(50))
                    cardsToDraw++;
                DrawXCards(cardsToDraw);
                break;
            case 172: // HAIR TRIGGER 4
                DealDamage(2);
                cardsToDraw = 1;
                if (PercentChance(50))
                    cardsToDraw++;
                DrawXCards(cardsToDraw);
                break;
            case 173: // HAIR TRIGGER 5
                DealDamage(2);
                cardsToDraw = 1;
                if (PercentChance(50))
                    cardsToDraw += 2;
                DrawXCards(cardsToDraw);
                break;
            case 174: // RELOAD 1
                DrawRandomCardFromDeck("Weapon");
                break;
            case 175: // RELOAD 2
                DrawRandomCardFromDeck("Weapon");
                break;
            case 176: // RELOAD 3
                DrawRandomCardFromDeck("Weapon");
                DealDamage(1);
                break;
            case 177: // RELOAD 4
                DrawRandomCardFromDeck("Weapon");
                DealDamage(1);
                break;
            case 178: // RELOAD 5
                DrawRandomCardFromDeck("Weapon");
                DealDamage(2);
                GainStatus(StatusEffect.StatusType.Momentum, 1);
                break;
            case 179: // MISFIRE 1
                DealDamage(1);
                break;
            case 180: // MISFIRE 2
                DealDamage(1);
                break;
            case 181: // MISFIRE 3
                DealDamage(2);
                break;
            case 182: // MISFIRE 4
                DealDamage(2);
                break;
            case 183: // MISFIRE 5
                DealDamage(2);
                break;
            case 184: // CHARGED SHOT 1
                DealDamage(battleData.GetWeaponCardsPlayedThisTurn(), 4 * battleData.GetWeaponCardsPlayedThisTurn());
                break;
            case 185: // CHARGED SHOT 2
                DealDamage(battleData.GetWeaponCardsPlayedThisTurn(), 6 * battleData.GetWeaponCardsPlayedThisTurn());
                break;
            case 186: // CHARGED SHOT 3
                DealDamage(2 * battleData.GetWeaponCardsPlayedThisTurn(), 8 * battleData.GetWeaponCardsPlayedThisTurn());
                break;
            case 187: // CHARGED SHOT 4
                DealDamage(2 * battleData.GetWeaponCardsPlayedThisTurn(), 10 * battleData.GetWeaponCardsPlayedThisTurn());
                break;
            case 188: // CHARGED SHOT 5
                DealDamage(3 * battleData.GetWeaponCardsPlayedThisTurn(), 12 * battleData.GetWeaponCardsPlayedThisTurn());
                break;
            case 189: // VENT HEAT 1
                SelfDamage(6);
                DrawXCards(1);
                break;
            case 190: // VENT HEAT 2
                SelfDamage(4);
                DrawXCards(1);
                break;
            case 191: // VENT HEAT 3
                SelfDamage(2);
                DrawXCards(1);
                break;
            case 192: // VENT HEAT 4
                DrawXCards(1);
                break;
            case 193: // VENT HEAT 5
                DrawXCards(2);
                break;
            case 194: // DOUBLE TAP 1
            case 195: // DOUBLE TAP 2
                DealDamage(1);
                StartCoroutine(WaitForSomethingToFinish("dealingDamage"));
                return true;
            case 196: // DOUBLE TAP 3
            case 197: // DOUBLE TAP 4
                GainStatus(StatusEffect.StatusType.Momentum, 1);
                DealDamage(1);
                StartCoroutine(WaitForSomethingToFinish("dealingDamage"));
                return true;
            case 198: // DOUBLE TAP 5
                GainStatus(StatusEffect.StatusType.Momentum, 2);
                DealDamage(1);
                StartCoroutine(WaitForSomethingToFinish("dealingDamage"));
                return true;
            case 199: // SHOOT 1
            case 200: // SHOOT 2
                DealDamage(1);
                DrawXCards(1);
                break;
            case 201: // SHOOT 3
                DealDamage(2);
                DrawXCards(1);
                break;
            case 202: // SHOOT 4
                DealDamage(2);
                DrawXCards(1);
                break;
            case 203: // SHOOT 5
                DealDamage(3);
                DrawXCards(1);
                break;
            case 204: // RELOAD 1
            case 205: // RELOAD 2
            case 206: // RELOAD 3
            case 207: // RELOAD 4
            case 208: // RELOAD 5
                Debug.Log("No on-played effect");
                break;
            default:
                Debug.Log("That card doesn't exist or doesn't have any actions on it built yet");
                break;
        }
        return false;
    }

    public int GetCardImageId()
    {
        // This method is for loading card images of cards that use the same image
        // For example, upgraded cards.
        // If a card uses the same image id as another, it must be converted here when getting images
        switch (cardId)
        {
            case 20:
            case 21:
                return 1;
            case 22:
            case 23:
            case 24:
            case 25:
                return 2;
            case 26:
            case 27:
            case 28:
                return 3;
            case 29:
            case 30:
                return 4;
            case 31:
            case 32:
            case 33:
            case 34:
                return 5;
            case 35:
            case 36:
            case 37:
                return 6;
            case 38:
            case 39:
            case 40:
            case 41:
                return 7;
            case 42:
            case 43:
            case 44:
                return 8;
            case 45:
            case 46:
            case 47:
            case 48:
                return 9;
            case 49:
            case 50:
            case 51:
                return 10;
            case 52:
            case 53:
            case 54:
            case 55:
                return 11;
            case 56:
            case 57:
            case 58:
            case 59:
                return 12;
            case 60:
            case 61:
            case 62:
            case 63:
                return 13;
            case 64:
            case 65:
                return 14;
            case 66:
                return 15;
            case 67:
            case 68:
                return 16;
            case 69:
            case 70:
                return 17;
            case 71:
            case 72:
                return 19;
            case 73:
            case 74:
            case 75:
            case 76:
            case 77:
                return 20;
            case 78:
            case 79:
            case 80:
            case 81:
            case 82:
                return 21;
            case 83:
            case 84:
            case 85:
            case 86:
                return 22;
            case 87:
            case 88:
            case 89:
            case 90:
            case 91:
                return 23;
            case 92:
            case 93:
            case 94:
            case 95:
            case 96:
                return 24;
            case 97:
            case 98:
            case 99:
            case 100:
                return 25;
            case 101:
            case 102:
            case 103:
            case 104:
            case 105:
                return 26;
            case 106:
            case 107:
            case 108:
            case 109:
            case 110:
                return 27;
            case 111:
            case 112:
            case 113:
            case 114:
                return 28;
            case 115:
                return 29;
            case 116:
            case 117:
            case 118:
            case 119:
            case 120:
                return 30;
            case 121:
            case 122:
            case 123:
            case 124:
            case 125:
                return 31;
            case 126:
            case 127:
            case 128:
            case 129:
            case 130:
                return 32;
            case 131:
            case 132:
            case 133:
            case 134:
                return 33;
            case 135:
            case 136:
            case 137:
            case 138:
            case 139:
                return 34;
            case 140:
            case 141:
            case 142:
            case 143:
            case 144:
                return 35;
            case 145:
            case 146:
            case 147:
            case 148:
            case 149:
                return 36;
            case 150:
            case 151:
            case 152:
            case 153:
            case 154:
                return 37;
            case 155:
            case 156:
            case 157:
            case 158:
                return 38;
            case 159:
            case 160:
            case 161:
            case 162:
            case 163:
                return 39;
            case 164:
            case 165:
            case 166:
            case 167:
            case 168:
                return 40;
            case 169:
            case 170:
            case 171:
            case 172:
            case 173:
                return 41;
            case 174:
            case 175:
            case 176:
            case 177:
            case 178:
                return 42;
            case 179:
            case 180:
            case 181:
            case 182:
                return 43;
            case 183:
                return 44;
            case 184:
            case 185:
            case 186:
            case 187:
            case 188:
                return 45;
            case 189:
            case 190:
            case 191:
                return 46;
            case 192:
            case 193:
                return 50;
            case 194:
            case 195:
            case 196:
            case 197:
            case 198:
                return 47;
            case 199:
            case 200:
            case 201:
            case 202:
            case 203:
                return 48;
            case 204:
            case 205:
            case 206:
            case 207:
            case 208:
                return 49;
            default:
                return cardId;
        }
    }

    private void AfterWaitAction()
    {
        switch (cardId)
        {
            case 28:
                DiscardCardsWithTag("Weakness");
                EndTurn();
                break;
            case 194: // DOUBLE TAP 1
            case 195: // DOUBLE TAP 2
            case 196: // DOUBLE TAP 3
            case 197: // DOUBLE TAP 4
            case 198: // DOUBLE TAP 5
                DealDamage(1);
                break;
        }
        // NOW we can destroy the gameobject
        Destroy(gameObject);
    }

    private IEnumerator WaitForSomethingToFinish(string finishWhat)
    {
        if (finishWhat == "drawingCards")
        {
            PlayerHand playerHand = FindObjectOfType<PlayerHand>();
            while (playerHand.GetIsDrawing() == true)
            {
                yield return null;
            }
        } else if (finishWhat == "dealingDamage")
        {
            PipManagerEnemy enemyPipManager = configData.GetEnemyHealthPipManager();
            while (enemyPipManager.GetIsAnimatingDamage())
            {
                yield return null;
            }
        }
        AfterWaitAction();
    }

    private void GainExtraDamageModifier(float amount)
    {
        CharacterData runner = FindObjectOfType<BattleData>().GetCharacter();
        runner.SetExtraDamageMultiplier(amount);
    }

    private void DiscardCardsWithTag(string tagName)
    {
        FindObjectOfType<PlayerHand>().DiscardCardsWithTag(tagName);
    }

    private void ShuffleCardsIntoEnemyDeck(List<int> cardsToAddIds)
    {
        FindObjectOfType<EnemyDeck>().ShuffleGeneratedCardsIntoDeck(cardsToAddIds);
    }

    private void GainHandBuff(int amount)
    {
        PlayerHand playerHand = FindObjectOfType<PlayerHand>();
        playerHand.GainHandBuff(amount);
    }

    private void GainHandDebuff(int amount)
    {
        PlayerHand playerHand = FindObjectOfType<PlayerHand>();
        playerHand.InflictHandDebuff(amount);
    }

    private void LoadCardPicker(List<Card> cards, int amountToPick)
    {
        if (cards.Count > 0)
        {
            configData.GetCardPicker().Initialize(cards, amountToPick, "SelectToHandFromDeckAndDiscardOthers");
        }
    }

    private void GainHealth(int amountToGain)
    {
        CharacterData runner = FindObjectOfType<BattleData>().GetCharacter();
        runner.GainHealth(amountToGain);
    }

    private void GainEnergy(int amountToGain)
    {
        CharacterData runner = FindObjectOfType<BattleData>().GetCharacter();
        runner.GainEnergy(amountToGain);
    }

    private void HealDebuff(int amountOfDebuffsToHeal)
    {
        playerCurrentStatusEffects.HealDebuffs(amountOfDebuffsToHeal);
    }

    private void GainStatus(StatusEffect.StatusType statusType, int stacks)
    {
        playerCurrentStatusEffects.InflictStatus(statusType, stacks, playerOrEnemy);
    }

    private void GainStatus(StatusEffect.StatusType statusType, int stacks, int duration)
    {
        playerCurrentStatusEffects.InflictStatus(statusType, stacks, playerOrEnemy, duration);
    }

    private void PowerupStatus(string type, int buffAmount, int durationBuffAmount)
    {
        playerCurrentStatusEffects.PowerupStatus(type, buffAmount, durationBuffAmount);
    }

    private void InflictStatus(StatusEffect.StatusType statusType, int stacks)
    {
        enemyCurrentStatusEffects.InflictStatus(statusType, stacks, playerOrEnemy);
    }

    private void SelfDamage(int damageAmount)
    {
        CharacterData runner = FindObjectOfType<BattleData>().GetCharacter();
        runner.TakeDamage(damageAmount);
    }

    private void DealDamage(int damageAmount, int critChance = 0)
    {
        // Modify Damage
        damageAmount += playerCurrentStatusEffects.GetMomentumStacks();
        damageAmount += enemyCurrentStatusEffects.GetVulnerableStacks();
        damageAmount += FindObjectOfType<BattleData>().GetEnemyVulnerabilityMapDebuff();
        damageAmount -= enemyCurrentStatusEffects.GetDamageResistStacks();

        int dodgeChance = enemyCurrentStatusEffects.GetDodgeChance();
        if (PercentChance(dodgeChance))
        {
            Debug.Log("Dodged!");
            damageAmount = 0;
        } else
        {
            int modifiedCritChance = Mathf.Clamp(critChance + FindObjectOfType<BattleData>().GetPlayerCritMapBuff() + playerCurrentStatusEffects.GetCritChanceStacks(), 0, 100);
            damageAmount = CheckAndApplyCritical(damageAmount, modifiedCritChance);
        }

        damageAmount = Mathf.Clamp(damageAmount, 0, 999999);
        battleData.GetEnemy().TakeDamage(damageAmount);
    }

    private int CheckAndApplyCritical(int damageAmount, int critChance)
    {
        // Check for crit
        bool criticalHit = false;
        if (PercentChance(critChance))
        {
            criticalHit = true;
        }
        else if (playerCurrentStatusEffects.GetCritUpStacks() > 0)
        {
            criticalHit = true;
            GainStatus(StatusEffect.StatusType.AutoCrit, -1);
        }

        // Apply crit
        int calculatedDamage = damageAmount;
        if (criticalHit)
        {
            calculatedDamage = Mathf.FloorToInt(critModifier * calculatedDamage);
        }
        return calculatedDamage;
    }

    private void DrawXCards(int amountToDraw)
    {
        if (!battleData.CanPlayerDrawExtraCards())
            return;

        playerHand.TriggerAcceleration(amountToDraw);
        playerHand.DrawXCards(amountToDraw);
    }

    private bool PercentChance(int percentChance)
    {
        int randomNumber = Mathf.CeilToInt(Random.Range(0, 100));
        if (randomNumber <= percentChance)
        {
            return true;
        }
        return false;
    }

    private bool DrawRandomCardFromDeck(string keyword)
    {
        if (!battleData.CanPlayerDrawExtraCards())
            return false;

        Card cardToDraw = deck.DrawRandomCardFromDeck(keyword);
        if (cardToDraw.GetCardId() == 0)
        {
            return false;
        }
        playerHand.TriggerAcceleration(1);
        battleData.AddToDrawnCardCount(1);
        playerHand.DrawCard(cardToDraw);
        return true;
    }

    private void DrawAllOfTypeFromDeck(string keyword)
    {
        while (deck.DoesKeywordExistInDeck(keyword))
        {
            DrawRandomCardFromDeck(keyword);
        }
    }

    private void DrawRandomCardFromDiscard(string keyword)
    {
        Card cardToDraw = discard.DrawRandomCardFromDiscard(keyword);
        if (cardToDraw.GetCardId() == 0)
        {
            return;
        }
        playerHand.TriggerAcceleration(1);
        battleData.AddToDrawnCardCount(1);
        playerHand.DrawCard(cardToDraw);
    }

    private void DrawRandomCardFromDeckOrDiscard(string keyword)
    {
        if (!battleData.CanPlayerDrawExtraCards())
            return;

        bool result = DrawRandomCardFromDeck(keyword);
        if (!result)
        {
            DrawRandomCardFromDiscard(keyword);
        }
    }

    private void SkipEndTurnDiscard(bool newSkipDiscard)
    {
        battleData.SkipEndTurnDiscard(newSkipDiscard);
    }

    private void EndTurn()
    {
        battleData.EndTurn();
    }

    public string GetPlayerOrEnemy()
    {
        return playerOrEnemy;
    }

    public void SetPlayerOrEnemy(string newPlayerOrEnemy)
    {
        playerOrEnemy = newPlayerOrEnemy;
    }

    public Sprite GetImageByGameobjectName(string gameobjectName)
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.name == gameobjectName)
            {
                return spriteRenderer.sprite;
            }
        }

        return FindObjectOfType<AllCards>().GetCardById(0).GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public string GetCardText()
    {
        return cardText.text;
    }

    public Sprite GetLeftCircuitImage()
    {
        return leftCircuit.sprite;
    }

    public Sprite GetTopCircuitImage()
    {
        return topCircuit.sprite;
    }

    public Sprite GetRightCircuitImage()
    {
        return rightCircuit.sprite;
    }

    public Sprite GetBottomCircuitImage()
    {
        return bottomCircuit.sprite;
    }

    public int GetEnergyCost()
    {
        return energyCost;
    }

    public void ReduceEnergyCost(int amount)
    {
        if (energyCost - amount < 0)
            energyCost = 0;
        else
            energyCost -= amount;
        energyCostzone.GetComponentInChildren<TextMeshProUGUI>().text = energyCost.ToString();
    }

    private void CheckOnPlayedEffects()
    {
        battleData.CheckOnPlayedEffects(this);
    }
}
