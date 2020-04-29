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

    int orientation = 0;
        // 0 = 0, 1 = 90, 2 = 180, 3 = 270
        // needed for figuring out connections

    private void Start()
    {
        SetupCard();
    }

    public void SetupCard()
    {
        
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
}