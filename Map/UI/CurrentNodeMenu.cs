using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentNodeMenu : MonoBehaviour
{
    [SerializeField] CurrentNodeMenuPointsOfInterest poiMenu;
    [SerializeField] CurrentNodeMenuHacks hackMenu;
    MapSquare square;
    Sprite locationImage;

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
    }

    public void ReopenHackMenu(HackTarget hackTarget)
    {
        hackMenu.ReopenHackMenu(hackTarget);
    }

    public void CloseMenu()
    {
        Debug.Log("closed menu");
        square = null;
        locationImage = null;
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        gameObject.SetActive(false);
    }

    public MapSquare GetMapSquare()
    {
        return square;
    }
}
