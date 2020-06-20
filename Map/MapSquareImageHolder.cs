using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSquareImageHolder : MonoBehaviour
{
    [SerializeField] Sprite[] citySquares;

    List<Sprite> unusedSquares;
    List<Sprite> usedSquares;

    [SerializeField] Sprite[] slumImages;

    List<Sprite> unusedImages;
    List<Sprite> usedImages;

    [SerializeField] Sprite[] menuIcons;

    [SerializeField] Sprite[] activeMenuButtons;
    [SerializeField] Sprite[] inactiveMenuButtons;

    public Sprite GetButtonImageByName(string buttonName, bool isActive)
    {
        if (isActive)
        {
            switch (buttonName)
            {
                case "Trap":
                    return activeMenuButtons[9];
            }
        } else
        {
            switch (buttonName)
            {
                case "Trap":
                    return activeMenuButtons[9];
            }
        }
        return activeMenuButtons[0];
    }

    public Sprite GetMenuIconByName(string iconName)
    {
        switch (iconName)
        {
            case "Hacks":
                return menuIcons[1];
            case "Security Camera":
                return menuIcons[2];
            case "Combat Server":
                return menuIcons[3];
            case "Database":
                return menuIcons[4];
            case "Defense System":
                return menuIcons[5];
            case "Transportation":
                return menuIcons[6];
            case "Medical Server":
                return menuIcons[7];
            case "Objects":
                return menuIcons[8];
            case "Trap":
                return menuIcons[9];
            case "PowerUp":
                return menuIcons[10];
            case "Reward":
                return menuIcons[11];
            case "Shop":
                return menuIcons[12];
            case "Upgrade":
                return menuIcons[13];
            case "First Aid Station":
                return menuIcons[14];
        }
        return menuIcons[0];
    }

    public Sprite GetSquareImage()
    {
        if (unusedSquares.Count == 0)
        {
            unusedSquares = usedSquares;
            usedSquares = new List<Sprite>();
        }

        Sprite imageToReturn = unusedSquares[Random.Range(0, unusedSquares.Count)];
        usedSquares.Add(imageToReturn);
        unusedSquares.Remove(imageToReturn);
        return imageToReturn;
    }

    public Sprite GetLocationImage()
    {
        if (unusedImages.Count == 0)
        {
            unusedImages = usedImages;
            usedImages = new List<Sprite>();
        }

        Sprite imageToReturn = unusedImages[Random.Range(0, unusedImages.Count)];
        usedImages.Add(imageToReturn);
        unusedImages.Remove(imageToReturn);
        return imageToReturn;
    }

    public void Initialize(string mapType)
    {
        unusedSquares = new List<Sprite>();
        usedSquares = new List<Sprite>();

        unusedImages = new List<Sprite>();
        usedImages = new List<Sprite>();

        switch (mapType)
        {
            case "slums":
                unusedSquares.AddRange(citySquares);
                unusedImages.AddRange(slumImages);
                break;
        }
    }
}
