using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HackDeck : MonoBehaviour
{
    [SerializeField] CheckClicks clickChecker;

    HackDiscard hackDiscard;
    List<HackCard> cards = new List<HackCard>();
    float movementSpeed = 10000f;
    int zPos = 1;
    Vector3 startPostion;
    bool isEmpty = false;

    private void Start()
    {
        hackDiscard = FindObjectOfType<HackDiscard>();
        isEmpty = false;
        movementSpeed = 10000f;
        startPostion = transform.position;
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
    }

    private void MoveTowardTarget(float targetX, float targetY)
    {
        float step = movementSpeed * Time.deltaTime;
        Vector3 newPosition = new Vector3(targetX, targetY, zPos);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
    }

    public void RemoveTopCardFromDeck()
    {
        cards.RemoveAt(0);
        if (cards.Count > 0)
        {
            SetTopCard();
        } else
        {
            isEmpty = true;
            Image[] imageHolders = GetComponentsInChildren<Image>();
            AllSpikeImages allSpikeImages = FindObjectOfType<AllSpikeImages>();
            foreach(Image image in imageHolders)
            {
                image.sprite = allSpikeImages.GetEmptyImage();
            }
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
        return isEmpty;
    }

    public HackCard GetTopCard()
    {
        return cards[0];
    }

    public void SetTopCard()
    {
        Image[] imageHolders = GetComponentsInChildren<Image>();
        AllSpikeImages allSpikeImages = FindObjectOfType<AllSpikeImages>();

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
            Debug.Log(topCard.GetLeftCircuit());
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
    }
}
