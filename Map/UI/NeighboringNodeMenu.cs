using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeighboringNodeMenu : MonoBehaviour
{
    [SerializeField] PointOfInterestLine[] pointOfInterestLines;
    [SerializeField] NeighboringNodeEnemyInfo enemyInfo;

    MapSquare square;
    Sprite locationImage;
    List<HackTarget> hackTargets;
    List<MapObject> mapObjects;
    int poiScoutLevel;

    Enemy enemy;

    public void InitializeMenu(MapSquare mapSquare)
    {
        poiScoutLevel = 3;
        gameObject.SetActive(true);
        square = mapSquare;
        locationImage = square.GetLocationImage();
        hackTargets = square.GetHackTargets();
        mapObjects = square.GetMapObjects();

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

        if (enemy != null)
        {
            enemyInfo.SetupEnemyInfo(enemy, square);
        } else
        {
            enemyInfo.SetupEmptyEnemy();
        }

        SetupHackTargets();
    }

    private void SetupHackTargets()
    {
        switch(poiScoutLevel)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                SetupLevelThreeScoutPOIUI();
                break;
        }
    }

    private void SetupLevelThreeScoutPOIUI()
    {
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

    public void CloseMenu()
    {
        square = null;
        locationImage = null;
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        gameObject.SetActive(false);
    }
}
