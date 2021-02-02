using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeighboringNodeMenu : MonoBehaviour
{
    [SerializeField] PointOfInterestLine[] pointOfInterestLines;
    [SerializeField] NeighboringNodeEnemyInfo enemyInfo;
    [SerializeField] GameObject levelOneScoutPOI;
    [SerializeField] GameObject levelTwoAndThreeScoutPOI;
    [SerializeField] Button moveButton;

    [SerializeField] Button effectsInfoButton;
    [SerializeField] EffectsListControl effectsListControl;
    bool effectsListOpen;

    MapSquare square;
    MapSquare playerLocation;
    Sprite locationImage;
    List<HackTarget> hackTargets;
    List<MapObject> mapObjects;
    int poiScoutLevel;
    int enemyScoutLevel;

    Enemy enemy;
    MapSFX mapSFX;

    private void Awake()
    {
        mapSFX = FindObjectOfType<MapSFX>();
    }

    public void InitializeMenu(MapSquare mapSquare)
    {
        gameObject.SetActive(true);
        square = mapSquare;
        locationImage = square.GetLocationImage();
        hackTargets = square.GetHackTargets();
        mapObjects = square.GetMapObjects();
        poiScoutLevel = square.GetPOIScoutLevel();
        enemyScoutLevel = square.GetEnemyScoutLevel();

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

        enemy = mapSquare.GetEnemy();

        if (enemy != null || enemyScoutLevel == 1)
        {
            enemyInfo.SetupEnemyInfo(enemy, square, enemyScoutLevel);
        } else if (enemy == null && enemyScoutLevel == 3 || enemy == null && enemyScoutLevel == 2)
        {
            enemyInfo.SetupEmptyEnemy(square);
        }

        SetupHackTargets();
        CheckIsPlayerAdjacent();
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

    private void CheckIsPlayerAdjacent()
    {
        List<MapSquare> adjacentSquares = square.GetAdjacentSquares();
        moveButton.interactable = false;
        foreach(MapSquare square in adjacentSquares)
        {
            if (square.GetIsPlayerPresent())
            {
                moveButton.interactable = true;
                playerLocation = square;
            }
        }
    }

    private void SetupHackTargets()
    {
        switch(poiScoutLevel)
        {
            case 1:
                SetupLevelOneScoutPOIUI();
                break;
            case 2:
                SetupLevelTwoScoutPOIUI();
                break;
            case 3:
                SetupLevelThreeScoutPOIUI();
                break;
        }
    }

    private void SetupLevelOneScoutPOIUI()
    {
        levelOneScoutPOI.SetActive(true);
        levelTwoAndThreeScoutPOI.SetActive(false);
    }

    private void SetupLevelTwoScoutPOIUI()
    {
        levelOneScoutPOI.SetActive(false);
        levelTwoAndThreeScoutPOI.SetActive(true);

        int counter = 0;
        PointOfInterestLine line = pointOfInterestLines[counter];
        line.SetupLine(false, "Hacks", hackTargets.Count);

        counter++;
        line = pointOfInterestLines[counter];
        line.SetupLine(false, "Objects", mapObjects.Count);

        while (counter < 4)
        {
            counter += 1;
            line = pointOfInterestLines[counter];
            line.SetBlankLine();
        }
    }

    private void SetupLevelThreeScoutPOIUI()
    {
        levelOneScoutPOI.SetActive(false);
        levelTwoAndThreeScoutPOI.SetActive(true);

        int counter = 0;
        PointOfInterestLine line = pointOfInterestLines[counter];
        line.SetupLine(false, "Hacks", hackTargets.Count);

        foreach (HackTarget hackTarget in hackTargets)
        {
            counter += 1;
            line = pointOfInterestLines[counter];
            line.SetupLine(true, hackTarget.getHackType());
        }

        counter++;
        line = pointOfInterestLines[counter];
        line.SetupLine(false, "Objects", mapObjects.Count);

        foreach (MapObject mapObject in mapObjects)
        {
            counter += 1;
            line = pointOfInterestLines[counter];
            line.SetupLine(true, mapObject.GetObjectType());
        }

        while (counter < 4)
        {
            counter++;
            line = pointOfInterestLines[counter];
            line.SetBlankLine();
        }
    }

    public void MovePlayer()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.PlayerMove);
        FindObjectOfType<MapData>().MovePlayer(playerLocation, square);
        CloseMenu();
    }

    public void CloseMenu()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        square = null;
        locationImage = null;
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        gameObject.SetActive(false);
    }

    public void ClickEffectsButton()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        if (!effectsListOpen)
        {
            effectsListControl.gameObject.SetActive(true);
            effectsListControl.GenerateEffectsList(square);
            effectsListOpen = true;
        }
        else
        {
            effectsListOpen = false;
            effectsListControl.gameObject.SetActive(false);
        }
    }
}
