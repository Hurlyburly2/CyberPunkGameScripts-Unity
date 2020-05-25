using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HackDeck : MonoBehaviour
{
    [SerializeField] CheckClicks clickChecker;

    HackDiscard hackDiscard;
    List<HackCard> cards = new List<HackCard>();
    float movementSpeed = 10000f;
    int zPos = 1;
    Vector3 startPostion;
    TextMeshProUGUI cardsInHackDeckCountTextField;
    HackCard previousTopHackCard;

    string tempLeftCircuit;
    string tempTopCircuit;
    string tempRightCircuit;
    string tempBottomCircuit;

    string prevTempLeftCircuit;
    string prevTempTopCircuit;
    string prevTempRightCircuit;
    string prevtempBottomCircuit;

    private void Start()
    {
        hackDiscard = FindObjectOfType<HackDiscard>();
        movementSpeed = 10000f;
        startPostion = transform.position;
        FindAndSetTextField();
    }

    private void FindAndSetTextField()
    {
        TextMeshProUGUI[] textFields = FindObjectsOfType<TextMeshProUGUI>();
        foreach(TextMeshProUGUI textField in textFields)
        {
            if (textField.name == "DeckCountText")
            {
                cardsInHackDeckCountTextField = textField;
                return;
            }
        }
    }

    private void Update()
    {
        if (clickChecker.GetClickState() == "dragging")
        {
            MoveTowardTarget(Input.mousePosition.x, Input.mousePosition.y);
        } else if (clickChecker.GetClickState() == "goingback")
        {
            MoveTowardTarget(startPostion.x, startPostion.y);
            if (transform.position == new Vector3(startPostion.x, startPostion.y, zPos))
            {
                clickChecker.SetNormalState();
            }
        } else if (clickChecker.GetClickState() == "discarding")
        {
            MoveTowardTarget(hackDiscard.transform.position.x, hackDiscard.transform.position.y);
            if (transform.position == hackDiscard.transform.position)
            {
                ResetToStartPosition();
            }
        }
    }

    public void SendTopCardToDiscard()
    {
        hackDiscard.AddCardToDiscard(cards[0]);
        RemoveTopCardFromDeck();
        SetTopCard();
        ResetToStartPosition();
        SetTextFieldCount();
    }

    private void ResetToStartPosition()
    {
        transform.position = startPostion;
    }

    private void MoveTowardTarget(float targetX, float targetY)
    {
        float step = movementSpeed * Time.deltaTime;
        Vector3 newPosition = new Vector3(targetX, targetY, zPos);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
    }

    public void ReAttachTopCard()
    {
        if (cards.Count > 0)
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                if (i == cards.Count - 1)
                {
                    cards.Add(cards[i]);
                } else
                {
                    cards[i + 1] = cards[i];
                }
            }
            cards[0] = previousTopHackCard;
            ReSetPrevModifiers();
        } else
        {
            cards.Add(previousTopHackCard);
            ReSetPrevModifiers();
        }

        FindObjectOfType<HackBattleData>().SetStateToNormal();
        SetTopCard();
        transform.position = Input.mousePosition;
        clickChecker.SetDraggingState();
        SetTextFieldCount();
    }

    private void ReSetPrevModifiers()
    {
        tempLeftCircuit = prevTempLeftCircuit;
        tempTopCircuit = prevTempTopCircuit;
        tempRightCircuit = prevTempRightCircuit;
        tempBottomCircuit = prevtempBottomCircuit;
    }

    private void LogCardIdsInList()
    {
        string cardIds = "";
        foreach (HackCard card in cards)
        {
            cardIds += " " + card.GetCardId().ToString();
        }
        Debug.Log(cardIds);
    }

    public void RemoveTopCardFromDeck()
    {
        SetPreviousCardAndModifiers();
        cards.RemoveAt(0);
        ClearTemporaryCardModifications();
        if (cards.Count > 0)
        {
            //SetTopCard();
            // Do not place top card until the previous card is confirmed
            SetAllImagesToEmpty();
            SetTextFieldCount();
        } else
        {
            SetTextFieldCount();
            SetAllImagesToEmpty();
        }
    }

    private void SetPreviousCardAndModifiers()
    {
        previousTopHackCard = cards[0];
        prevTempLeftCircuit = tempLeftCircuit;
        prevTempTopCircuit = tempTopCircuit;
        prevTempRightCircuit = tempRightCircuit;
        prevtempBottomCircuit = tempBottomCircuit;
    }

    private void SetAllImagesToEmpty()
    {
        Image[] imageHolders = GetComponentsInChildren<Image>();
        AllSpikeImages allSpikeImages = FindObjectOfType<AllSpikeImages>();
        foreach (Image image in imageHolders)
        {
            image.sprite = allSpikeImages.GetEmptyImage();
        }
    }

    public void ShuffleDeck()
    {
        System.Random _random = new System.Random();

        HackCard myGO;

        int n = cards.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = cards[r];
            cards[r] = cards[i];
            cards[i] = myGO;
        }
    }

    public bool IsDeckEmpty()
    {
        if (cards.Count > 0)
        {
            return false;
        }
        return true;
    }

    public HackCard GetTopCard()
    {
        return cards[0];
    }

    public void SetTopCard()
    {
        Image[] imageHolders = GetComponentsInChildren<Image>();
        AllSpikeImages allSpikeImages = FindObjectOfType<AllSpikeImages>();
        Debug.Log(FindObjectOfType<HackBattleData>().GetState());

        if (cards.Count == 0)
        {
            foreach (Image image in imageHolders)
            {
                image.sprite = allSpikeImages.GetEmptyImage();
            }
        }
        else
        {
            HackCard topCard = cards[0];
            foreach (Image image in imageHolders)
            {
                switch (image.name)
                {
                    case "HackDeckCardBack":
                        image.sprite = allSpikeImages.GetCardBack();
                        break;
                    case "LeftCircuit":
                        string color;
                        if (tempLeftCircuit != null)
                            color = tempLeftCircuit;
                        else
                            color = topCard.GetLeftCircuit();
                        Sprite currentImage = allSpikeImages.GetCircuitImageByColorAndDirection(color, "left");
                        image.sprite = currentImage;
                        break;
                    case "TopCircuit":
                        if (tempTopCircuit != null)
                            color = tempTopCircuit;
                        else
                            color = topCard.GetTopCircuit();
                        currentImage = allSpikeImages.GetCircuitImageByColorAndDirection(color, "top");
                        image.sprite = currentImage;
                        break;
                    case "RightCircuit":
                        if (tempRightCircuit != null)
                            color = tempRightCircuit;
                        else
                            color = topCard.GetRightCircuit();
                        currentImage = allSpikeImages.GetCircuitImageByColorAndDirection(color, "right");
                        image.sprite = currentImage;
                        break;
                    case "DownCircuit":
                        if (tempBottomCircuit != null)
                            color = tempBottomCircuit;
                        else
                            color = topCard.GetBottomCircuit();
                        currentImage = allSpikeImages.GetCircuitImageByColorAndDirection(color, "bottom");
                        image.sprite = currentImage;
                        break;
                    case "TopLeftSpike":
                        Spike currentspike = topCard.GetTopLeftSpike();
                        string state = currentspike.GetSpikeState();
                        color = currentspike.GetSpikeColor();
                        currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, "topleft", state);
                        image.sprite = currentImage;
                        break;
                    case "TopRightSpike":
                        currentspike = topCard.GetTopRightSpike();
                        state = currentspike.GetSpikeState();
                        color = currentspike.GetSpikeColor();
                        currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, "topright", state);
                        image.sprite = currentImage;
                        break;
                    case "BottomLeftSpike":
                        currentspike = topCard.GetBottomLeftSpike();
                        state = currentspike.GetSpikeState();
                        color = currentspike.GetSpikeColor();
                        currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, "bottomleft", state);
                        image.sprite = currentImage;
                        break;
                    case "BottomRightSpike":
                        currentspike = topCard.GetbottomRightSpike();
                        state = currentspike.GetSpikeState();
                        color = currentspike.GetSpikeColor();
                        currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, "bottomright", state);
                        image.sprite = currentImage;
                        break;
                    case "CardImage":
                        image.sprite = topCard.GetCardImage();
                        break;
                }
            }
        }
    }

    public void SetDeckPrefabs(List<HackCard> newHackCards)
    {
        cards = newHackCards;
        SetTextFieldCount();
    }

    public void SetTextFieldCount()
    {
        cardsInHackDeckCountTextField.text = cards.Count.ToString();
    }

    public int GetCardCount()
    {
        return cards.Count;
    }

    public void AddConnectionsToActiveCard(int number, string color)
    {
        HackCard topCard = cards[0];

        // in circuit array: 0 = left, 1 = top, 2 = right, 3 = bottom
        string[] circuits = { topCard.GetLeftCircuit(), topCard.GetTopCircuit(), topCard.GetRightCircuit(), topCard.GetBottomCircuit() };
        List<int> emptyColorIndices = new List<int>();
        List<int> differentColorIndices = new List<int>();
        List<int> sameColorIndices = new List<int>();

        for (int i = 0; i < circuits.Length; i++)
        {
            if (circuits[i] == color)
            {
                sameColorIndices.Add(i);
            } else if (circuits[i] == "none")
            {
                emptyColorIndices.Add(i);
            } else
            {
                differentColorIndices.Add(i);
            }
        }

        for (int i = 0; i < number; i++)
        {
            if (emptyColorIndices.Count > 0)
            {
                int indexToUse = Random.Range(0, emptyColorIndices.Count);
                SetTemporaryCircuit(emptyColorIndices[indexToUse], color);
                emptyColorIndices.RemoveAt(indexToUse);

            } else if (differentColorIndices.Count > 0)
            {
                int indexToUse = Random.Range(0, differentColorIndices.Count);
                SetTemporaryCircuit(differentColorIndices[indexToUse], color);
                differentColorIndices.RemoveAt(indexToUse);
            }
        }

        SetTopCard();
    }

    private void SetTemporaryCircuit(int positionNumber, string color)
    {
        switch(positionNumber)
        {
            case 0:
                tempLeftCircuit = color;
                break;
            case 1:
                tempTopCircuit = color;
                break;
            case 2:
                tempRightCircuit = color;
                break;
            case 3:
                tempBottomCircuit = color;
                break;
        }
    }

    public List<HackCard> GetTopXHackCards(int howManyCards)
    {
        int counter = 0;
        List<HackCard> foundCards = new List<HackCard>();
        while (counter < howManyCards && counter < cards.Count)
        {
            foundCards.Add(cards[counter]);
            counter++;
        }

        return foundCards;
    }

    // SECURITY EFFECTS

    public void TrashXCards(int amountOfCardsToTrash)
    {
        StartCoroutine(TrashCardFromDeck(amountOfCardsToTrash));
    }

    private IEnumerator TrashCardFromDeck(int amountOfCardsToTrash)
    {
        if (amountOfCardsToTrash > cards.Count)
        {
            amountOfCardsToTrash = cards.Count;
        }
        for (int i = 0; i < amountOfCardsToTrash; i++)
        {
            clickChecker.SetDiscardingState();
            yield return new WaitForSeconds(0.5f);
            SendTopCardToDiscard();
        }
        ResetToStartPosition();
        clickChecker.SetNormalState();
    }

    private void ClearTemporaryCardModifications()
    {
        tempLeftCircuit = null;
        tempTopCircuit = null;
        tempRightCircuit = null;
        tempBottomCircuit = null;
    }

    public string[] GetTemporaryCircuits()
    {
        string[] tempCircuits = { tempLeftCircuit, tempTopCircuit, tempRightCircuit, tempBottomCircuit };
        return tempCircuits;
    }
}
