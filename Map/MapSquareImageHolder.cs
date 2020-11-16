using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSquareImageHolder : MonoBehaviour
{
    List<int> unusedSquareIds;
    List<int> usedSquareIds;

    List<int> usedImageIds;
    List<int> unusedImageIds;

    [SerializeField] Sprite[] menuIcons;

    [SerializeField] Sprite[] activeMenuButtons;
    [SerializeField] Sprite[] inactiveMenuButtons;

    [SerializeField] Sprite[] hackAndPOITypeImages;

    [SerializeField] Sprite[] pointSquares;

    int maxSlumLocations = 43; // amount of locations images in slums
    int maxCitySquares = 20;

    public Sprite GetPointSquareByColor(string color)
    {
        switch (color)
        {
            case "red":
                return pointSquares[1];
            case "blue":
                return pointSquares[2];
            case "green":
                return pointSquares[3];
        }
        return pointSquares[0];
    }

    public Sprite GetImageForHackOrPOI(string name)
    {
        switch(name)
        {
            case "Security Camera":
                return hackAndPOITypeImages[2];
            case "Combat Server":
                return hackAndPOITypeImages[3];
            case "Database":
                return hackAndPOITypeImages[4];
            case "Defense System":
                return hackAndPOITypeImages[5];
            case "Transportation":
                return hackAndPOITypeImages[6];
            case "Medical Server":
                return hackAndPOITypeImages[7];
        }
        return hackAndPOITypeImages[0];
    }

    public Sprite GetButtonImageByName(string buttonName, bool isActive)
    {
        if (isActive)
        {
            switch (buttonName)
            {
                case "Security Camera":
                    return activeMenuButtons[2];
                case "Combat Server":
                    return activeMenuButtons[3];
                case "Database":
                    return activeMenuButtons[4];
                case "Defense System":
                    return activeMenuButtons[5];
                case "Transportation":
                    return activeMenuButtons[6];
                case "Medical Server":
                    return activeMenuButtons[7];
                case "Trap":
                    return activeMenuButtons[9];
                case "PowerUp":
                    return activeMenuButtons[10];
                case "Reward":
                    return activeMenuButtons[11];
                case "Shop":
                    return activeMenuButtons[12];
                case "Upgrade":
                    return activeMenuButtons[13];
                case "First Aid Station":
                    return activeMenuButtons[14];
            }
        } else
        {
            switch (buttonName)
            {
                case "Security Camera":
                    return inactiveMenuButtons[2];
                case "Combat Server":
                    return inactiveMenuButtons[3];
                case "Database":
                    return inactiveMenuButtons[4];
                case "Defense System":
                    return inactiveMenuButtons[5];
                case "Transportation":
                    return inactiveMenuButtons[6];
                case "Medical Server":
                    return inactiveMenuButtons[7];
                case "Trap":
                    return inactiveMenuButtons[9];
                case "PowerUp":
                    return inactiveMenuButtons[10];
                case "Reward":
                    return inactiveMenuButtons[11];
                case "Shop":
                    return inactiveMenuButtons[12];
                case "Upgrade":
                    return inactiveMenuButtons[13];
                case "First Aid Station":
                    return inactiveMenuButtons[14];
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

    public Sprite GetSquareImage(Job.JobArea mapType)
    {
        string locationString = "Squares";
        switch (mapType)
        {
            case Job.JobArea.Slums:
                locationString += "/City/Square";
                locationString += GetRandomSquareId().ToString();
                return Resources.Load<Sprite>(locationString);
        }
        return Resources.Load<Sprite>("Squares/City/Square1");
    }

    private int GetRandomSquareId()
    {
        if (unusedSquareIds.Count == 0)
        {
            unusedSquareIds = new List<int>();
            unusedSquareIds.AddRange(usedSquareIds);
            usedSquareIds = new List<int>();
        }

        int index = Random.Range(0, unusedSquareIds.Count - 1);
        int currentId = unusedSquareIds[index];
        usedSquareIds.Add(currentId);
        unusedSquareIds.Remove(currentId);
        return currentId;
    }

    public Sprite GetLocationImage(Job.JobArea mapType)
    {
        string locationString = "LocationImages";
        switch(mapType)
        {
            case Job.JobArea.Slums:
                locationString += "/Slums/Location";
                locationString += GetRandomLocationId().ToString();
                return Resources.Load<Sprite>(locationString);
        }

        return Resources.Load<Sprite>("LocationImages/City/Location1");
    }

    private int GetRandomLocationId()
    {
        int currentId = unusedImageIds[Random.Range(0, unusedImageIds.Count - 1)];
        usedImageIds.Add(currentId);
        unusedImageIds.Remove(currentId);
        return currentId;
    }

    public void InitializeMapSquareImageHolder(Job.JobArea mapType)
    {
        switch (mapType)
        {
            case Job.JobArea.Slums:
                InitializeLists(maxSlumLocations, maxCitySquares);
                break;
        }
    }

    private void InitializeLists(int maxLocations, int maxCitySquares)
    {
        usedSquareIds = new List<int>();
        unusedSquareIds = new List<int>();

        for (int i = 1; i <= maxCitySquares; i++)
        {
            unusedSquareIds.Add(i);
        }

        usedImageIds = new List<int>();
        unusedImageIds = new List<int>();
        for (int i = 1; i <= maxLocations; i++)
        {
            unusedImageIds.Add(i);
        }
    }
}
