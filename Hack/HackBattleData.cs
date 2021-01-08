using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackBattleData : MonoBehaviour
{
    // config
    CharacterData runner;
    HackerData hacker;
    HackDeck hackDeck;
    HackDiscard hackDiscard;
    AllHackCards allHackCards;
    HackSecurityUI hackSecurityUI;
    MapGrid mapGrid;
    MapSquare currentMapSquare;
    HackTarget hackTarget;

    string state = "normal";
    // currently: normal, cardui
    string securityType;
        // currently: default
    int redPoints = 0;
    int bluePoints = 0;
    int greenPoints = 0;
    int safeZoneSize;
    int securityLevel = 0;

    PointIconHolder redPointIconHolder;
    PointIconHolder bluePointIconHolder;
    PointIconHolder greenPointIconHolder;

    // Passive abilities variables
    List<PassiveAbility> passiveAbilities;

    // Traptype
    MapObject.TrapTypes trapType;

    List<int> temporaryCardIdsToRemove = new List<int>();

    public void Iunno()
    {
        Debug.Log("test");
    }

    private void Awake()
    {
        int count = FindObjectsOfType<BattleData>().Length;

        if (count > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void FinishHack()
    {
        FindObjectOfType<SceneLoader>().LoadMapFromHack(redPoints, bluePoints, greenPoints, currentMapSquare, hackTarget);
    }

    public void SetCharacterData(CharacterData newRunner, HackerData newHacker)
    {
        runner = newRunner;
        hacker = newHacker;
    }

    public void SetupHack(int safeSize, string newSecurityType)
    {
        hackDeck = FindObjectOfType<HackDeck>();
        hackDiscard = FindObjectOfType<HackDiscard>();
        allHackCards = FindObjectOfType<AllHackCards>();

        List<int> cardIds = new List<int>();
        if (trapType == MapObject.TrapTypes.ParalysisAgent)
        {
            // Paralysis Agent Trap: Blocks arm and leg mods
            List<Item.ItemTypes> blockedItemTypes = new List<Item.ItemTypes> { Item.ItemTypes.Arm, Item.ItemTypes.Leg };
            cardIds.AddRange(runner.GetLoadout().GetAllCardIds(blockedItemTypes));
        } else if (trapType == MapObject.TrapTypes.ConcussiveBlast)
        {
            List<Item.ItemTypes> blockedItemTypes = new List<Item.ItemTypes> { Item.ItemTypes.Head, Item.ItemTypes.Exoskeleton };
            cardIds.AddRange(runner.GetLoadout().GetAllCardIds(blockedItemTypes));
        }
        else
        {
            cardIds.AddRange(runner.GetLoadout().GetAllCardIds());
        }

        // Block Hacker cards if faraday cage
        if (trapType != MapObject.TrapTypes.FaradayCage)
            cardIds.AddRange(hacker.GetHackerLoadout().GetCardIds());

        cardIds.AddRange(FindObjectOfType<MapData>().GetTemporaryCardIds());

        // Create a deck from here, but for now we use nonsense cards
        LogAllCardIds(cardIds);

        List<HackCard> cards = GetCardsByIds(cardIds);
        hackDeck.SetDeckPrefabs(cards);
        hackDeck.ShuffleDeck();
        hackDeck.SetTopCard();

        securityType = newSecurityType;

        hackSecurityUI = FindObjectOfType<HackSecurityUI>();
        SetupPointHolders();
        this.safeZoneSize = safeSize;
        SetupSafeSquares();

        SetupAbilityButtons();

        SetupPassiveAbilities();
    }

    private void SetupPassiveAbilities()
    {
        List<HackerModChip> modChips = hacker.GetHackerLoadout().GetAllModChips();
        passiveAbilities = new List<PassiveAbility>();

        foreach (HackerModChip chip in modChips)
        {
            passiveAbilities.Add(chip.SetupPassiveAbility());
        }
    }

    private void SetupAbilityButtons()
    {
        AbilityButton[] abilityButtons = FindObjectsOfType<AbilityButton>();
        HackerLoadout currentLoadout = hacker.GetHackerLoadout();

        foreach (AbilityButton abilityButton in abilityButtons)
        {
            switch(abilityButton.GetWhichAbility())
            {
                case "rig":
                    HackerMod rigMod = currentLoadout.GetRigMod();
                    abilityButton.SetupAbility(rigMod);
                    break;
                case "neuralImplant":
                    HackerMod neuralImplantMod = currentLoadout.GetNeuralImplantMod();
                    abilityButton.SetupAbility(neuralImplantMod);
                    break;
                case "uplink":
                    HackerMod uplinkMod = currentLoadout.GetUplinkMod();
                    abilityButton.SetupAbility(uplinkMod);
                    break;
            }
        }
    }

    public void RaiseSecurityLevel()
    {
        if (!IsThereADangerZoneBuffer())
        {
            securityLevel++;
            safeZoneSize += 2;
            SetupSafeSquares();
            hackSecurityUI.UpdateHackSecurityUI(securityLevel);
        }
    }

    public void LowerSecurityLevel()
    {
        securityLevel--;
        safeZoneSize -= 2;
        SetupSafeSquares();
        hackSecurityUI.UpdateHackSecurityUI(securityLevel);
    }

    public bool IsThereADangerZoneBuffer()
    {
        foreach(PassiveAbility passiveAbility in passiveAbilities)
        {
            if (passiveAbility.GetAbilityType() == PassiveAbility.PassiveAbilityType.dangerZoneBuffer && passiveAbility.GetRemainingUses() > 0)
            {
                passiveAbility.UseOne();
                return true;
            }
        }
        return false;
    }

    private void SetupSafeSquares()
    {
        HackGridSquare[] hackGridSquares = FindObjectsOfType<HackGridSquare>();
        int squareHomeY = 7 - (safeZoneSize / 2 - 1);
        int squareHomeX = 5 - (safeZoneSize / 2 - 1);  
        foreach(HackGridSquare square in hackGridSquares)
        {
            bool isSafe = false;
            bool isPlacementLegal = true;
            // Check safe area
            if (square.GetParentRowNumber() >= squareHomeY && square.GetParentRowNumber() < squareHomeY + safeZoneSize)
            {
                if (square.GetSquareNumber() >= squareHomeX && square.GetSquareNumber() < squareHomeX + safeZoneSize)
                {
                    if (IsEdgeSquare(square))
                        isSafe = false;
                    else
                        isSafe = true;
                }
            }
            square.SetSafe(isSafe);

            // Check for illegal area
            if (square.GetParentRowNumber() < squareHomeY - 1 || square.GetParentRowNumber() > squareHomeY + safeZoneSize)
            {
                isPlacementLegal = false;
            }
            if (square.GetSquareNumber() < squareHomeX - 1 || square.GetSquareNumber() > squareHomeX + safeZoneSize)
            {
                isPlacementLegal = false;
            }
            if (IsEdgeSquare(square))
                isPlacementLegal = false;
            square.SetLegality(isPlacementLegal);

        }
        hackSecurityUI.UpdateHackSecurityUI(securityLevel);
    }

    private bool IsEdgeSquare(HackGridSquare square)
    {
        if (square.GetParentRowNumber() == 0 || square.GetParentRowNumber() == 16 || square.GetSquareNumber() == 0 || square.GetSquareNumber() == 12)
            return true;
        else
            return false;
    }

    private List<HackCard> GetCardsByIds(List<int> cardIds)
    {
        List<HackCard> foundCards = new List<HackCard>();
        foreach(int cardId in cardIds)
        {
            foundCards.Add(allHackCards.GetCardById(cardId));
        }
        return foundCards;
    }

    public HackerData GetHacker()
    {
        return hacker;
    }

    private void LogAllCardIds(List<int> cardIds)
    {
        string idString = "";
        foreach (int cardId in cardIds)
        {
            idString += cardId + " ";
        }
    }

    public void SetStateToCardUI()
    {
        state = "cardui";
    }

    public void SetStateToNormal()
    {
        state = "normal";
    }

    public string GetState()
    {
        return state;
    }

    public bool IsPlayerAllowedToDragCard()
    {
        if (state == "cardui")
        {
            return false;
        } else
        {
            return true;
        }
    }

    private void SetupPointHolders()
    {
        PointIconHolder[] pointIconHolders = FindObjectsOfType<PointIconHolder>();
        foreach (PointIconHolder pointIconHolder in pointIconHolders)
        {
            switch(pointIconHolder.GetWhichColor())
            {
                case "red":
                    redPointIconHolder = pointIconHolder;
                    break;
                case "blue":
                    bluePointIconHolder = pointIconHolder;
                    break;
                case "green":
                    greenPointIconHolder = pointIconHolder;
                    break;
            }
        }
        UpdatePointDisplay();
    }

    public void UpdatePointValue(string color, int amount)
    {
        int modifier = CheckForPointModifierPassive(color, amount);
        switch(color)
        {
            case "red":
                redPoints += amount * modifier;
                break;
            case "blue":
                bluePoints += amount * modifier;
                break;
            case "green":
                greenPoints += amount * modifier;
                break;
        }
        UpdatePointDisplay();
    }

    private int CheckForPointModifierPassive(string color, int amount)
    {
        foreach(PassiveAbility passiveAbility in passiveAbilities)
        {
            if (passiveAbility.GetAbilityType() == PassiveAbility.PassiveAbilityType.spikePointMultiplier &&
                (passiveAbility.GetColor() == color || passiveAbility.GetColor() == "any") &&
                passiveAbility.GetConnectionType() == amount &&
                passiveAbility.GetRemainingUses() > 0)  
            {
                passiveAbility.UseOne();
                return passiveAbility.GetMultiplier();
            }
        }
        return 1;
    }

    public void UpdatePointDisplay()
    {
        redPointIconHolder.UpdatePointDisplay(redPoints);
        bluePointIconHolder.UpdatePointDisplay(bluePoints);
        greenPointIconHolder.UpdatePointDisplay(greenPoints);
    }

    public void OnCardPlacement(int cardId)
    {
        switch(securityType)
        {
            case "default":
                DefaultSecurityOnCardPlacement();
                break;
        }

        if (cardId == 210)
        {
            temporaryCardIdsToRemove.Add(cardId);
        }
    }

    private void DefaultSecurityOnCardPlacement()
    {
        hackDeck.TrashXCards(securityLevel);
    }

    public void SetMapData(MapGrid newMapGrid, MapSquare mapSquare, HackTarget newHackTarget)
    {
        mapGrid = newMapGrid;
        mapGrid.gameObject.SetActive(false);
        currentMapSquare = mapSquare;
        hackTarget = newHackTarget;
        trapType = mapSquare.GetTriggeredTrapType();
    }

    public MapGrid GetMapGrid()
    {
        return mapGrid;
    }

    public List<int> GetTemporaryCardIdsToRemove()
    {
        return temporaryCardIdsToRemove;
    }
}
