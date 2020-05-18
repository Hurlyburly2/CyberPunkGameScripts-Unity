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
        } else
        {
            cards.Add(previousTopHackCard);
        }

        FindObjectOfType<HackBattleData>().SetStateToNormal();
        SetTopCard();
        transform.position = Input.mousePosition;
        clickChecker.SetDraggingState();
        SetTextFieldCount();
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
        previousTopHackCard = cards[0];
        cards.RemoveAt(0);
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
                        string color = topCard.GetLeftCircuit();
                        Sprite currentImage = allSpikeImages.GetCircuitImageByColorAndDirection(color, "left");
                        image.sprite = currentImage;
                        break;
                    case "TopCircuit":
                        color = topCard.GetTopCircuit();
                        currentImage = allSpikeImages.GetCircuitImageByColorAndDirection(color, "top");
                        image.sprite = currentImage;
                        break;
                    case "RightCircuit":
                        color = topCard.GetRightCircuit();
                        currentImage = allSpikeImages.GetCircuitImageByColorAndDirection(color, "right");
                        image.sprite = currentImage;
                        break;
                    case "DownCircuit":
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
}
