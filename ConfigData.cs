using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigData : MonoBehaviour
{
    // Screen measurements
    float halfHeight;
    float halfWidth;

    // Hand config
    float handStartPos;
    float handEndPos;
    float handMiddlePos;
    float cardSizeMultiplier = 1;
    float cardWidth;

    // Cards are played when dragged above this y axis
    float cardPlayLine;

    // Seriealized fields
    [SerializeField] float handDrawSpeed = 20f;
    [SerializeField] float handAdjustSpeed = 5f;
    [SerializeField] float maxCardInHandAngle = 20f;
    [SerializeField] float curvedHandMaxVerticalOffset = .75f;

    AllCards allCards;

    // Start is called before the first frame update
    void Start()
    {
        SetupConfig();
    }

    private void SetupConfig()
    {
        allCards = FindObjectOfType<AllCards>();

        halfHeight = Camera.main.orthographicSize;
        halfWidth = Camera.main.aspect * halfHeight;
        cardPlayLine = halfHeight + halfHeight / 9;
        cardSizeMultiplier = halfWidth / 8.1775f;
        handMiddlePos = halfHeight / 1.48f;

        cardWidth = allCards.GetRandomCard().GetComponentInChildren<SpriteRenderer>().bounds.size.x * cardSizeMultiplier;

        float margin = halfWidth / 15.72f;
        handStartPos = Camera.main.transform.position.x - halfWidth + margin + (cardWidth / cardSizeMultiplier / 2);
        handEndPos = Camera.main.transform.position.x + halfWidth - margin - (cardWidth / cardSizeMultiplier / 2);
        handMiddlePos = halfHeight / 1.48f;

        // Tweak for narrow screens
        if (handStartPos - (Camera.main.transform.position.x - halfWidth) > halfWidth / 4)
        {
            handStartPos = Camera.main.transform.position.x - halfWidth * .55f;
            handEndPos = Camera.main.transform.position.x + halfWidth * .55f;
        }
    }

    public float GetCardWidth()
    {
        return cardWidth;
    }

    public float GetCardSizeMultiplier()
    {
        return cardSizeMultiplier;
    }

    public float GetHandStartPos()
    {
        return handStartPos;
    }

    public float GetHandEndPos()
    {
        return handEndPos;
    }

    public float GetHandMiddlePos()
    {
        return handMiddlePos;
    }

    public float GetHalfHeight()
    {
        return halfHeight;
    }

    public float GetHalfWidth()
    {
        return halfWidth;
    }

    public float GetHandAdjustSpeed()
    {
        return handAdjustSpeed;
    }

    public float GetHandDrawSpeed()
    {
        return handDrawSpeed;
    }

    public float MaxCardInHandAngle()
    {
        return maxCardInHandAngle;
    }

    public float GetHandMaxVerticalOffset()
    {
        return curvedHandMaxVerticalOffset;
    }

    public float GetCardPlayedLine()
    {
        return cardPlayLine;
    }
}
