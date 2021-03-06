﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentNodeMenu : MonoBehaviour
{
    [SerializeField] CurrentNodeMenuPointsOfInterest poiMenu;
    [SerializeField] CurrentNodeMenuHacks hackMenu;
    MapSquare square;
    Sprite locationImage;

    [SerializeField] Button effectsInfoButton;
    [SerializeField] EffectsListControl effectsListControl;
    bool effectsListOpen;

    MapSFX mapSFX;

    private void Awake()
    {
        mapSFX = FindObjectOfType<MapSFX>();
    }

    public void InitializeMenu(MapSquare newSquare)
    {
        gameObject.SetActive(true);
        square = newSquare;

        locationImage = square.GetLocationImage();

        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            switch (image.name)
            {
                case "LocationImageContent":
                    image.sprite = locationImage;
                    break;
            }
        }

        poiMenu.SetupButtons(square.GetMapObjects());
        hackMenu.SetupButtons(square.GetHackTargets());

        UpdateEffectsButton();
    }

    public void UpdateEffectsButton()
    {
        effectsListControl.gameObject.SetActive(false);
        effectsListOpen = false;
        if (square.ShouldEffectsButtonAppear())
        {
            effectsInfoButton.gameObject.SetActive(true);
        }
        else
        {
            effectsInfoButton.gameObject.SetActive(false);
        }
    }

    public void ReopenHackMenu(HackTarget hackTarget)
    {
        hackMenu.ReopenHackMenu(hackTarget);
    }

    public void CloseMenu()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        square = null;
        locationImage = null;
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        gameObject.SetActive(false);
    }

    public MapSquare GetMapSquare()
    {
        return square;
    }

    public void ClickEffectsButton()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        if (!effectsListOpen)
        {
            effectsListControl.gameObject.SetActive(true);
            effectsListControl.GenerateEffectsList(square);
            effectsListOpen = true;
        } else
        {
            effectsListOpen = false;
            effectsListControl.gameObject.SetActive(false);
        }
    }
}
