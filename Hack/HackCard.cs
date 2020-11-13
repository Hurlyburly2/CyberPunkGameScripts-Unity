﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackCard : MonoBehaviour
{
    [SerializeField] int cardId;

    [SerializeField] string leftConnection = "none";
    [SerializeField] string topConnection = "none";
    [SerializeField] string rightConnection = "none";
    [SerializeField] string bottomConnection = "none";

    [SerializeField] Spike topLeftSpike;
    [SerializeField] Spike topRightSpike;
    [SerializeField] Spike bottomLeftSpike;
    [SerializeField] Spike bottomRightSpike;

    [SerializeField] HackCardUIHolder uiImageHolder;

    HackGridSquare gridSquareHolder;

    int orientation = 0;
        // 0 = 0, 1 = 90, 2 = 180, 3 = 270
        // needed for figuring out connections

    public void SetupUI(int toPreviousRotation, int toNextRotation)
    {
        uiImageHolder.gameObject.SetActive(true);
        uiImageHolder.SetRotationsOfArrows(toPreviousRotation, toNextRotation);
    }

    public void SetGridSquareHolder(HackGridSquare parentSquare)
    {
        gridSquareHolder = parentSquare;
    }

    public HackGridSquare GetCurrentSquareHolder()
    {
        return gridSquareHolder;
    }

    public Sprite GetCardImage()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.name == "Image")
            {
                return spriteRenderer.sprite;
            }
        }

        // THIS COULD RETURN THE WRONG IMAGE IF NAMES ARE CHANGED ON THE GAMEOBJECT IN UNITY
        return spriteRenderers[0].sprite;
    }

    public void RotateCircuitsAndSpikesNinetyDegrees()
    {
        string previousLeftConnection = leftConnection;
        leftConnection = bottomConnection;
        bottomConnection = rightConnection;
        rightConnection = topConnection;
        topConnection = previousLeftConnection;

        string previousTopLeftSpikeColor = topLeftSpike.GetSpikeColor();
        string previousTopLeftSpikeState = topLeftSpike.GetSpikeState();

        topLeftSpike.SetupNewValues(bottomLeftSpike.GetSpikeColor(), bottomLeftSpike.GetSpikeState());
        topLeftSpike.SetSpikeImage("topleft");

        bottomLeftSpike.SetupNewValues(bottomRightSpike.GetSpikeColor(), bottomRightSpike.GetSpikeState());
        bottomLeftSpike.SetSpikeImage("bottomleft");

        bottomRightSpike.SetupNewValues(topRightSpike.GetSpikeColor(), topRightSpike.GetSpikeState());
        bottomRightSpike.SetSpikeImage("bottomright");

        topRightSpike.SetupNewValues(previousTopLeftSpikeColor, previousTopLeftSpikeState);
        topRightSpike.SetSpikeImage("topright");
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
    }

    public Spike GetTopLeftSpike()
    {
        return topLeftSpike;
    }

    public Spike GetTopRightSpike()
    {
        return topRightSpike;
    }

    public Spike GetBottomLeftSpike()
    {
        return bottomLeftSpike;
    }

    public Spike GetbottomRightSpike()
    {
        return bottomRightSpike;
    }

    public string GetLeftCircuit()
    {
        return leftConnection;
    }

    public string GetTopCircuit()
    {
        return topConnection;
    }

    public string GetRightCircuit()
    {
        return rightConnection;
    }

    public string GetBottomCircuit()
    {
        return bottomConnection;
    }

    public string[] GetConnectionsArray()
    {
        string[] allConnections = new string[] { leftConnection, topConnection, rightConnection, bottomConnection };
        return allConnections;
    }

    public int GetCardId()
    {
        return cardId;
    }

    public HackGridSquare FindParentSquare()
    {
        HackGridSquare[] allGridSquares = FindObjectsOfType<HackGridSquare>();
        foreach(HackGridSquare square in allGridSquares)
        {
            if (square.GetAttachedCard() == this)
            {
                return square;
            }
        }

        return null;
    }

    public void SetModifiedCircuit(string[] tempCircuits)
    {
        if (tempCircuits[0] != null)
            leftConnection = tempCircuits[0];
        if (tempCircuits[1] != null)
            topConnection = tempCircuits[1];
        if (tempCircuits[2] != null)
            rightConnection = tempCircuits[2];
        if (tempCircuits[3] != null)
            bottomConnection = tempCircuits[3];

        UpdateCircuitImages();
    }

    private void UpdateCircuitImages()
    {
        AllSpikeImages allSpikesAndCircuits = FindObjectOfType<AllSpikeImages>();
        SpriteRenderer[] allImages = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer image in allImages)
        {
            if (image.name == "Left")
                image.sprite = allSpikesAndCircuits.GetCircuitImageByColorAndDirection(leftConnection, "left");
            if (image.name == "Up")
                image.sprite = allSpikesAndCircuits.GetCircuitImageByColorAndDirection(topConnection, "top");
            if (image.name == "Right")
                image.sprite = allSpikesAndCircuits.GetCircuitImageByColorAndDirection(rightConnection, "right");
            if (image.name == "Down")
                image.sprite = allSpikesAndCircuits.GetCircuitImageByColorAndDirection(bottomConnection, "bottom");
        }
    }

    public Sprite GetImageByGameObjectName(string gameobjectName)
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.name == gameobjectName)
            {
                return spriteRenderer.sprite;
            }
        }

        return FindObjectOfType<AllHackCards>().GetCardById(0).GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public int GetImageId()
    {
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
            default:
                return cardId;
        }
    }
}
