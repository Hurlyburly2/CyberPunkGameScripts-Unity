using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HackDiscard : MonoBehaviour
{
    List<HackCard> cards = new List<HackCard>();
    HackDeck hackDeck;
    TextMeshProUGUI discardCountTextField;

    [SerializeField] GameObject usualParent;
    [SerializeField] GameObject temporaryParent;

    int zPos = 1;
    float movementSpeed = 10000f;
    string state;
        // normal, returningToDeck

    Vector3 startPosition;

    private void Awake()
    {
        hackDeck = FindObjectOfType<HackDeck>();
        state = "normal";
        SetTopCard();
        SetTextField();
        startPosition = transform.position;
    }

    private void Update()
    {
        if (state == "returningToDeck")
        {
            MoveTowardTarget(hackDeck.transform.position.x, hackDeck.transform.position.y);
        }
    }

    private void MoveTowardTarget(float targetX, float targetY)
    {
        float step = movementSpeed * Time.deltaTime;
        Vector3 newPosition = new Vector3(targetX, targetY, zPos);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
    }

    private void SetTextField()
    {
        TextMeshProUGUI[] allTextFields = FindObjectsOfType<TextMeshProUGUI>();
        foreach(TextMeshProUGUI textField in allTextFields)
        {
            if (textField.name == "DiscardCountText")
            {
                discardCountTextField = textField;
                UpdateTextFieldCounter();
                return;
            }
        }
    }

    public void SendCardsFromDiscardToTopOfDeck(int numberOfCards)
    {
        transform.SetParent(temporaryParent.transform);
        StartCoroutine(SendCardsFromDiscardToTopOfDeckCoroutine(numberOfCards));
    }

    private IEnumerator SendCardsFromDiscardToTopOfDeckCoroutine(int numberOfCards)
    {
        if (numberOfCards > cards.Count)
        {
            numberOfCards = cards.Count;
        }
        for (int i = 0; i < numberOfCards; i++)
        {
            state = "returningToDeck";
            yield return new WaitForSeconds(0.5f);
            SendTopCardToDeck();
            ResetToStartPosition();
        }
        transform.SetParent(usualParent.transform);
        state = "normal";
        FindObjectOfType<HackBattleData>().SetStateToNormal();
        ResetToStartPosition();
    }

    private void SendTopCardToDeck()
    {
        hackDeck.ReAttachTopCardFromDiscard(cards[cards.Count - 1]);
        cards.RemoveAt(cards.Count - 1);
        UpdateTextFieldCounter();
        SetTopCard();
    }

    private void ResetToStartPosition()
    {
        transform.localPosition = new Vector3(0, 0, zPos);
    }

    public void UpdateTextFieldCounter()
    {
        discardCountTextField.text = cards.Count.ToString();
    }

    public void AddCardToDiscard(HackCard card)
    {
        cards.Add(card);
        SetTopCard();
        UpdateTextFieldCounter();
    }

    public void SetTopCard()
    {
        Image[] imageHolders = GetComponentsInChildren<Image>();
        AllSpikeImages allSpikeImages = FindObjectOfType<AllSpikeImages>();

        if (cards.Count == 0)
        {
            foreach(Image image in imageHolders)
            {
                image.sprite = allSpikeImages.GetEmptyImage();
            }
        } else
        {
            HackCard topCard = cards[cards.Count - 1];
            foreach (Image image in imageHolders)
            {
                switch (image.name)
                {
                    case "HackDiscardCardBack":
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
}
