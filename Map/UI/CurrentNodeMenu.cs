using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentNodeMenu : MonoBehaviour
{
    [SerializeField] CurrentNodeMenuPointsOfInterest poiMenu;
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
    }

    public void CloseMenu()
    {
        square = null;
        locationImage = null;
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        gameObject.SetActive(false);
    }
}
