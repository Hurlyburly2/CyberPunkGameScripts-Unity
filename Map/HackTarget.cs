using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackTarget : ScriptableObject
{
    // config
    string mapType;
    string hackType;
    // options are fed to it by mapSquare:
    // "Security Camera", "Combat Server", "Database", "Defense System", "Transportation", "Medical Server"
    
    // state
    int redPoints;
    int bluePoints;
    int purplePoints;
    bool isActive;
    bool hackIsDone;
    bool canPlayerAffordAnything;

    public void SetupHackTarget(string newHackType)
    {
        redPoints = 0;
        bluePoints = 0;
        purplePoints = 0;
        isActive = true;
        hackIsDone = false;
        hackType = newHackType;
        mapType = FindObjectOfType<MapData>().GetMapType();
        SetupHackTest();
    }

    // METHOD FOR TESTING
    public void SetupHackTest()
    {
        hackType = "Security Camera";
        //redPoints = 500;
        //bluePoints = 500;
        //purplePoints = 500;
        //canPlayerAffordAnything = true;
        //hackIsDone = true;
    }

    public void SetPoints(int newRedPoints, int newBluePoints, int newPurplePoints)
    {
        redPoints = newRedPoints;
        bluePoints = newBluePoints;
        purplePoints = newPurplePoints;
        hackIsDone = true;
    }

    public bool GetIsHackDone()
    {
        return hackIsDone;
    }

    public int GetRedPoints()
    {
        return redPoints;
    }

    public int GetBluePoints()
    {
        return bluePoints;
    }

    public int GetPurplePoints()
    {
        return purplePoints;
    }

    public void SetCanPlayerAffordAnything(bool canTheyAffordAnything)
    {
        canPlayerAffordAnything = canTheyAffordAnything;
        if (!canPlayerAffordAnything && hackIsDone)
        {
            isActive = false;
        }
    }

    public bool CanPlayerAffordAbility(string color, int cost)
    {
        switch(color)
        {
            case "red":
                if (redPoints >= cost)
                    return true;
                else
                    return false;
            case "blue":
                if (bluePoints >= cost)
                    return true;
                else
                    return false;
            case "purple":
                if (purplePoints >= cost)
                    return true;
                else
                    return false;
        }
        return false;
    }

    public string getHackType()
    {
        return hackType;
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public string GetColor(int count)
    {
        switch (hackType)
        {
            case "Security Camera":
                return securityCameraColors[count];
        }
        return "";
    }

    public string GetDescription(int count)
    {
        switch (hackType)
        {
            case "Security Camera":
                return securityCameraOptions[count];
        }
        return "";
    }

    public int GetCost(int count)
    {
        switch (hackType)
        {
            case "Security Camera":
                return securityCameraCosts[count];
        }
        return 0;
    }

    // data
    string[] securityCameraOptions = {
        "Scout Points of Interest",
        "Reduce Security Level",
        "Reveal Points of Interest",
        "Scout Enemies",
        "Reveal Enemies",
        "Despawn a Weak Enemy",
        "Despawn a Medium Enemy"
    };
    string[] securityCameraColors = { "blue", "blue", "blue", "red", "red", "purple", "purple" };
    int[] securityCameraCosts = { 5, 10, 15, 10, 20, 15, 25 };

    public void UseAbility(MapSquare square, string description, string color, int cost)
    {
        switch (description)
        {
            case "Scout Points of Interest":
                ScoutPointOfInterest(2, square);
                break;
            case "Reduce Security Level":
                Debug.Log("Not yet implemented");
                break;
            case "Reveal Points of Interest":
                ScoutPointOfInterest(3, square);
                break;
            case "Scout Enemies":
                ScoutEnemies(2, square);
                break;
            case "Reveal Enemies":
                ScoutEnemies(3, square);
                break;
            case "Despawn a Weak Enemy":
                Debug.Log("Not yet implemented");
                break;
            case "Despawn a Medium Enemy":
                Debug.Log("Not yet implemented");
                break;
        }
        switch (color)
        {
            case "red":
                redPoints -= cost;
                break;
            case "blue":
                bluePoints -= cost;
                break;
            case "purple":
                purplePoints -= cost;
                break;
        }
    }

    private void ScoutPointOfInterest(int newScoutLevel, MapSquare square)
    {
        List<MapSquare> adjacentSquares = square.GetAdjacentSquares();
        foreach(MapSquare adjSquare in adjacentSquares)
        {
            adjSquare.SetPOIScoutLevel(newScoutLevel);
        }
    }

    private void ScoutEnemies(int newScoutLevel, MapSquare square)
    {
        List<MapSquare> adjacentSquares = square.GetAdjacentSquares();
        foreach(MapSquare adjSquare in adjacentSquares)
        {
            adjSquare.SetEnemyScoutLevel(newScoutLevel);
        }
    }

    private void ReduceSecurityLevel()
    {
        
    }

    private void DespawnAnEnemy()
    {

    }
}
