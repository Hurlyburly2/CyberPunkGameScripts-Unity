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
    string playerOrEnemy;
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
            RemoveHelperText();
            rotation = rememberRotation;
            SetSortingOrder(rememberSortingOrder);
            float mouseY = Input.mousePosition.y / Screen.height * configData.GetHalfHeight() * 2;
            if (mouseY > configData.GetCardPlayedLine() && battleData.WhoseTurnIsIt() == "player")
            {
                SetState("played");
                playerHand.RemoveCard(GetComponent<Card>());
                PlayCard();
            } else
            {
                transform.localScale = new Vector3(1, 1, 1);
                SetState("draw");
            }
        }
    }

    public void PlayCard()
    {
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

    // PLAY CARD ACTIONS

    private bool PlayCardActions()
    {
        // Return true if further actions need to happen
        switch (cardId)
        {
            case 0: // DUMMY CARD
                Debug.Log("This is a dummy card, it doesn't actually do anything");
                break;
            case 1: // AWARENESS 1
                GainStatus("Dodge", 1);
                break;
            case 2: // OBSERVE
                LoadCardPicker(deck.GetTopXCardsWithoutDraw(3), 1);
                break;
            case 3: // DEEP BREATH
                DrawXCards(5);
                GainStatus("Vulnerable", 1);
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
                GainStatus("Damage Resist", 1, 2);
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
                GainStatus("Momentum", 1);
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
                InflictStatus("Vulnerable", 2);
                break;
            case 14: // HIDDEN TRIGGER
                List<int> cardsToAddIds = new List<int>();
                cardsToAddIds.Add(3);
                ShuffleCardsIntoEnemyDeck(cardsToAddIds);
                break;
            case 15: // TOO OBVIOUS
                int removedTraps = FindObjectOfType<EnemyDeck>().RemoveAllTrapCards();
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
                GainStatus("Dodge", 2);
                break;
            case 21: // AWARENESS 3
                GainStatus("Dodge", 3);
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
                GainStatus("Vulnerable", 1);
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
                GainStatus("Damage Resist", 1, 2);
                DrawXCards(1);
                break;
            case 36: // BRACE 3
                GainStatus("Damage Resist", 1, 2);
                DrawXCards(1);
                break;
            case 37: // BRACE 4
                GainStatus("Damage Resist", 2, 2);
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
                GainStatus("Momentum", 1);
                break;
            case 47:
                DealDamage(2);
                GainStatus("Momentum", 2);
                break;
            case 48:
                DealDamage(3);
                GainStatus("Momentum", 2);
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
                InflictStatus("Vulnerable", 1);
                break;
            case 58: // KNEECAP 4
                DealDamage(Random.Range(5, 7));
                InflictStatus("Vulnerable", 1);
                break;
            case 59: // KNEECAP 5
                DealDamage(Random.Range(5, 9));
                InflictStatus("Vulnerable", 1);
                break;
            case 60: // BRUISE 2
            case 61: // BRUISE 3
                DealDamage(2);
                InflictStatus("Vulnerable", 2);
                break;
            case 62: // BRUISE 4
                DealDamage(2);
                InflictStatus("Vulnerable", 3);
                break;
            case 63: // BRUISE 5
                DealDamage(3);
                InflictStatus("Vulnerable", 3);
                break;
            case 64: // HIDDEN TRIGGER 2
                cardsToAddIds = new List<int>();
                cardsToAddIds.Add(3);
                ShuffleCardsIntoEnemyDeck(cardsToAddIds);
                GainStatus("Dodge", 1);
                break;
            case 65: // HIDDEN TRIGGER 3
                cardsToAddIds = new List<int>();
                cardsToAddIds.Add(3);
                cardsToAddIds.Add(3);
                ShuffleCardsIntoEnemyDeck(cardsToAddIds);
                GainStatus("Dodge", 1);
                break;
            case 66: // TOO OBVIOUS 2
                FindObjectOfType<EnemyDeck>().RemoveAllTrapCards();
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
                return 18;
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

    private void GainStatus(string statusType, int stacks)
    {
        playerCurrentStatusEffects.InflictStatus(statusType, stacks, playerOrEnemy);
    }

    private void GainStatus(string statusType, int stacks, int duration)
    {
        playerCurrentStatusEffects.InflictStatus(statusType, stacks, playerOrEnemy, duration);
    }

    private void PowerupStatus(string type, int buffAmount, int durationBuffAmount)
    {
        playerCurrentStatusEffects.PowerupStatus(type, buffAmount, durationBuffAmount);
    }

    private void InflictStatus(string statusType, int stacks)
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
            int modifiedCritChance = Mathf.Clamp(critChance + FindObjectOfType<BattleData>().GetPlayerCritMapBuff(), 0, 100);
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
            GainStatus("CritUp", -1);
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

    private void DrawRandomCardFromDeck(string keyword)
    {
        Card cardToDraw = deck.DrawRandomCardFromDeck(keyword);
        if (cardToDraw.GetCardId() == 0)
        {
            return;
        }
        playerHand.DrawCard(cardToDraw);
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
}
