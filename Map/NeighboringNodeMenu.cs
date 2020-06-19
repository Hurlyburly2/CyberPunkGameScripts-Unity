using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeighboringNodeMenu : MonoBehaviour
{
    MapSquare square;
    Sprite locationImage;

    public void InitializeMenu(MapSquare mapSquare)
    {
        gameObject.SetActive(true);
        square = mapSquare;
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
    }

    public void CloseMenu()
    {
        square = null;
        locationImage = null;
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        gameObject.SetActive(false);
    }
}
