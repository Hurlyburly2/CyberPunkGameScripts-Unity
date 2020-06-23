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

    public void SetupHackTarget(string newHackType)
    {
        redPoints = 0;
        bluePoints = 0;
        purplePoints = 0;
        isActive = true;
        hackIsDone = false;
        //hackType = newHackType;
        hackType = "Security Camera";
        // temporary for testing security camera
        mapType = FindObjectOfType<MapData>().GetMapType();
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

}
