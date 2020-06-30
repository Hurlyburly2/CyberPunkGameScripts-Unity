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

    // consts
    const string red = "red";
    const string blue = "blue";
    const string purple = "purple";

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
        hackType = "Defense System";
        redPoints = 500;
        bluePoints = 500;
        purplePoints = 500;
        canPlayerAffordAnything = true;
        hackIsDone = true;
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
            case red:
                if (redPoints >= cost)
                    return true;
                else
                    return false;
            case blue:
                if (bluePoints >= cost)
                    return true;
                else
                    return false;
            case purple:
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
            case "Combat Server":
                return combatServerColors[count];
            case "Database":
                return databaseColors[count];
            case "Defense System":
                return defenseSystemColors[count];
        }
        return "";
    }

    public string GetDescription(int count)
    {
        switch (hackType)
        {
            case "Security Camera":
                return securityCameraOptions[count];
            case "Combat Server":
                return combatServerOptions[count];
            case "Database":
                return databaseOptions[count];
            case "Defense System":
                return defenseSystemOptions[count];
        }
        return "";
    }

    public int GetCost(int count)
    {
        switch (hackType)
        {
            case "Security Camera":
                return securityCameraCosts[count];
            case "Combat Server":
                return combatServerCosts[count];
            case "Database":
                return databaseCosts[count];
            case "Defense System":
                return defenseSystemCosts[count];
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
    string[] securityCameraColors = { blue, blue, blue, red, red, purple, purple };
    int[] securityCameraCosts = { 5, 10, 15, 10, 20, 15, 25 };

    string[] combatServerOptions =
    {
        "Accelerate Reaction Time",
        "Disengage Enemy Shielding",
        "Enhance Targetting",
        "Overclock Systems",
        "Harden Armor",
        "Install Mod Intereference",
        "Sabotage Enemy Tech"
    };
    string[] combatServerColors = { blue, blue, red, red, red, purple, purple };
    int[] combatServerCosts = { 10, 20, 8, 12, 18, 10, 20 };

    string[] databaseOptions = {
        "Erase Database",
        "Upload and Sell Financial Data",
        "Install Backdoor",
        "Brute Force Passwords",
        "Upload and Sell Personal Data",
        "Download Buyer List",
        "Download VIP Buyer List"
    };
    string[] databaseColors = { blue, blue, blue, red, red, purple, purple };
    int[] databaseCosts = { 10, 10, 20, 5, 15, 10, 15 };

    string[] defenseSystemOptions =
    {
        "Disable Trap",
        "Reduce Security Level",
        "Detonate EMP",
        "Turn Turrets on Enemies",
        "Control Attack Drones",
        "Infect Weapon Systems",
        "Despawn a Strong Enemy"
    };
    string[] defenseSystemColors = { blue, blue, blue, red, red, purple, purple };
    int[] defenseSystemCosts = { 20, 10, 15, 10, 20, 10, 20 };

    // ability usages
    public void UseAbility(MapSquare square, string description, string color, int cost)
    {
        switch (description)
        {
            // SECURITY CAMERA OPTIONS
            case "Scout Points of Interest":
                ScoutPointOfInterest(2, square);
                break;
            case "Reduce Security Level":
                ReduceSecurityLevel(5);
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
            case "Despa0wn a Weak Enemy":
                DespawnAnEnemy(2, 1);
                break;
            case "Despawn a Medium Enemy":
                DespawnAnEnemy(3, 2);
                break;

            // COMBAT SERVER OPTIONS
            case "Accelerate Reaction Time":
                BuffPlayerDodge(10, square);
                break;
            case "Disengage Enemy Shielding":
                IncreaseEnemyVulnerability(1, square);
                break;
            case "Enhance Targetting":
                BuffPlayerCritChance(15, square);
                break;
            case "Overclock Systems":
                BuffPlayerHandSize(1, square);
                break;
            case "Harden Armor":
                BuffPlayerDefense(1, square);
                break;
            case "Install Mod Intereference":
                DebuffEnemyHandSize(1, square);
                break;
            case "Sabotage Enemy Tech":
                AddToEnemyFizzleChance(10, square);
                break;

            // DATABASE OPTIONS
            case "Erase Database":
                ReduceSecurityLevel(5);
                break;
            case "Upload and Sell Financial Data":
                GainMoney(600);
                break;
            case "Install Backdoor":
                RaiseMoneyMultiplier(5);
                RaiseGoalMultiplier(5);
                break;
            case "Brute Force Passwords":
                GainMoney(300);
                RaiseSecurityLevel(5);
                break;
            case "Upload and Sell Personal Data":
                GainMoney(400);
                break;
            case "Download Buyer List":
                RaiseGoalMultiplier(15);
                break;
            case "Download VIP Buyer List":
                RaiseGoalMultiplier(25);
                break;

            // DEFENSE SYSTEM OPTIONS
            case "Disable Trap":
                DisableATrap();
                break;
            //case "Reduce Security Level":
                // THIS IS ALREADY PART OF THE ABOVE, AND IT WORKS FOR BOTH
                // break;
            case "Detonate EMP":
                AddToEnemyFizzleChance(10, square);
                break;
            case "Turn Turrets on Enemies":
                DamageEnemies(10, square);
                break;
            case "Control Attack Drones":
                Debug.Log("Not yet Implemented");
                break;
            case "Infect Weapon Systems":
                Debug.Log("Not yet Implemented");
                break;
            case "Despawn a Strong Enemy":
                Debug.Log("Not yet Implemented");
                break;
        }
        switch (color)
        {
            case red:
                redPoints -= cost;
                break;
            case blue:
                bluePoints -= cost;
                break;
            case purple:
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

    private void ReduceSecurityLevel(int amount)
    {
        amount = amount * -1;
        FindObjectOfType<MapData>().AdjustSecurityLevel(amount);
    }

    private void RaiseSecurityLevel(int amount)
    {
        FindObjectOfType<MapData>().AdjustSecurityLevel(amount);
    }

    private void DespawnAnEnemy(int maxDespawn, int preferredMinDespawn)
    {
        MapSquare[] mapSquares = FindObjectsOfType<MapSquare>();
        List<MapSquare> preferredToDespawnSquares = new List<MapSquare>();
        List<MapSquare> weakerEnemiesToDespawn = new List<MapSquare>();

        foreach(MapSquare square in mapSquares)
        {
            if (square.GetEnemy() != null)
            {
                int starRating = square.GetEnemy().GetStarRating();
                if (starRating <= maxDespawn && starRating >= preferredMinDespawn)
                {
                    preferredToDespawnSquares.Add(square);
                }
                else if (starRating < preferredMinDespawn)
                {
                    weakerEnemiesToDespawn.Add(square);
                }
            }
        }

        if (preferredToDespawnSquares.Count > 0)
        {
            DespawnEnemyFromSquareList(preferredToDespawnSquares);
        } else
        {
            DespawnEnemyFromSquareList(weakerEnemiesToDespawn);
        }
    }

    private void DespawnEnemyFromSquareList(List<MapSquare> squares)
    {
        squares[Random.Range(0, squares.Count)].DespawnEnemy();
    }

    private void GainMoney(int amount)
    {
        FindObjectOfType<MapData>().ChangeMoney(amount);
    }

    private void RaiseMoneyMultiplier(int amount)
    {
        FindObjectOfType<MapData>().ChangeMoneyMultiplier(amount);
    }

    private void RaiseGoalMultiplier(int amount)
    {
        FindObjectOfType<MapData>().ChangeGoalMultiplier(amount);
    }

    private void BuffPlayerDodge(int amount, MapSquare currentSquare)
    {
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustPlayerDodge(10);
        }
    }

    private void IncreaseEnemyVulnerability(int amount, MapSquare currentSquare)
    {
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustEnemyVulnerability(amount);
        }
    }

    private void BuffPlayerCritChance(int amount, MapSquare currentSquare)
    {
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustPlayerCritChance(amount);
        }
    }

    private void BuffPlayerHandSize(int amount, MapSquare currentSquare)
    {
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustPlayerHandSize(amount);
        }
    }

    private void BuffPlayerDefense(int amount, MapSquare currentSquare)
    {
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustPlayerDefenseBuff(amount);
        }
    }

    private void DebuffEnemyHandSize(int amount, MapSquare currentSquare)
    {
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustEnemyHandSizeDebuff(amount);
        }
    }

    private void AddToEnemyFizzleChance(int amount, MapSquare currentSquare)
    {
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustEnemyFizzleChance(amount);
        }
    }

    private void DisableATrap()
    {
        MapSquare[] squares = FindObjectsOfType<MapSquare>();
        List<MapSquare> squaresWithTraps = new List<MapSquare>();
        foreach (MapSquare square in squares)
        {
            if (square.IsActive())
            {
                List<MapObject> mapObjects = square.GetMapObjects();
                foreach (MapObject mapObject in mapObjects)
                {
                    if (mapObject.GetObjectType() == "Trap" && mapObject.GetIsActive() == true)
                    {
                        squaresWithTraps.Add(square);
                    }
                }
            }
        }

        if (squaresWithTraps.Count > 0)
        {
            MapSquare squareToDisable = squaresWithTraps[Random.Range(0, squaresWithTraps.Count)];
            List<MapObject> mapObjects = squareToDisable.GetMapObjects();
            foreach (MapObject mapObject in mapObjects)
            {
                if (mapObject.GetObjectType() == "Trap")
                {
                    mapObject.SetIsActive(false);
                    Debug.Log("Disabled a Trap");
                }
            }
        }
    }

    private void DamageEnemies(int percentage, MapSquare currentSquare)
    {
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustPercentDamageToEnemy(percentage);
        }
    }

    private void LogEnemyCount()
    {
        int count = 0;
        MapSquare[] squares = FindObjectsOfType<MapSquare>();
        foreach (MapSquare square in squares)
        {
            if (square.GetEnemy() != null)
                count++;
        }
        Debug.Log("Found " + count + " enemies");
    }
}
