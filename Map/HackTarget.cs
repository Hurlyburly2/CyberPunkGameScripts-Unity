using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackTarget : ScriptableObject
{
    // config
    Job.JobArea mapType;
    string hackType;
    // options are fed to it by mapSquare:
    // "Security Camera", "Combat Server", "Database", "Defense System", "Transportation", "Medical Server"

    // consts
    const string red = "red";
    const string blue = "blue";
    const string purple = "purple";

    // if hackType is Transportation
    string transportationType = null;
    bool isStationOpen = false;

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
        //SetupHackTest();
    }

    // METHOD FOR TESTING
    public void SetupHackTest()
    {
        hackType = "Medical Server";
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
            case "Transportation":
                return transportationColors[count];
            case "Medical Server":
                return medicalServerColors[count];
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
            case "Transportation":
                return transportationOptions[count];
            case "Medical Server":
                return medicalServerOptions[count];
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
            case "Transportation":
                return transportationCosts[count];
            case "Medical Server":
                return medicalServerCosts[count];
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

    string[] transportationOptions =
    {
        "Map Ventilation Systems",
        "Co-Opt Stealth Tech",
        "Unlock This Metro Station",
        "Unlock Remote Metro Station",
        "Unlock All Metro Stations",
        "Hinder Enemy Movement",
        "Stop Enemy Movement"
    };
    string[] transportationColors = { blue, blue, red, red, red, purple, purple };
    int[] transportationCosts = { 10, 20, 5, 15, 30, 10, 20 };

    string[] medicalServerOptions = {
        "Gain Health and Energy Regeneration",
        "Sabotage Medicines",
        "Upload and Sell Medical Research",
        "Healing Boost",
        "Energy Infusion",
        "Heal and Gain Energy",
        "Injest Stimulants"
    };
    string[] medicalServerColors = { blue, blue, blue, red, red, purple, purple };
    int[] medicalServerCosts = { 10, 10, 10, 15, 15, 15, 15 };

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
                DotEnemies(2, square);
                break;
            case "Infect Weapon Systems":
                DebuffEnemyDamage(1, square);
                break;
            case "Despawn a Strong Enemy":
                DespawnAnEnemy(4, 3);
                break;

            // TRANSPORTATION
            case "Unlock This Metro Station":
                UnlockThisMetro(square);
                break;
            case "Unlock Remote Metro Station":
                UnlockRemoteMetro(square);
                break;
            case "Unlock All Metro Stations":
                UnlockAllMetroStations();
                break;
            case "Map Ventilation Systems":
                MapVentilationSystem(square);
                break;
            case "Co-Opt Stealth Tech":
                EngageStealthTech();
                break;
            case "Hinder Enemy Movement":
                HinderEnemyMovement(3);
                break;
            case "Stop Enemy Movement":
                StopEnemyMovement(1);
                break;

            // MEDICAL SERVER
            case "Gain Health and Energy Regeneration":
                GainHealthRegeneration(5);
                GainEnergyRegeneration(5);
                break;
            case "Sabotage Medicines":
                DotEnemies(4, square);
                break;
            case "Upload and Sell Medical Research":
                GainMoney(600);
                break;
            case "Healing Boost":
                GainHealth(15);
                break;
            case "Energy Infusion":
                GainEnergy(15);
                break;
            case "Heal and Gain Energy":
                GainHealth(10);
                GainEnergy(10);
                break;
            case "Injest Stimulants":
                BuffPlayerDodge(10, square);
                BuffPlayerHandSize(1, square);
                BuffPlayerCritChance(10, square);
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
        LogEnemyCount();
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
        LogEnemyCount();
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

    private void DotEnemies(int amount, MapSquare currentSquare)
    {
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustDotDamageToEnemy(amount);
        }
    }

    private void DebuffEnemyDamage(int amount, MapSquare currentSquare)
    {
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustEnemyDamageDebuff(amount);
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

    private void UnlockThisMetro(MapSquare currentSquare)
    {
        currentSquare.OpenMetroStation();
    }

    private void UnlockRemoteMetro(MapSquare currentSquare)
    {
        MapSquare[] allSquaresArray = FindObjectsOfType<MapSquare>();
        List<MapSquare> allSquares = new List<MapSquare>();
        allSquares.AddRange(allSquaresArray);

        // We don't want to deal with transportation nodes on the current square
        allSquares.Remove(currentSquare);

        // find the active squares with a transportation hack target that has not been unlocked
        List<MapSquare> squaresWithTransportation = new List<MapSquare>();
        foreach (MapSquare square in allSquares)
        {
            if (square.IsActive() && square.DoesSquareHaveTransportationNode() && !square.GetIsTransportationNodeUnlocked())
            {
                squaresWithTransportation.Add(square);
            }
        }

        if (squaresWithTransportation.Count > 0)
            squaresWithTransportation[Random.Range(0, squaresWithTransportation.Count)].OpenMetroStation();
    }

    private void UnlockAllMetroStations()
    {
        MapSquare[] allSquaresArray = FindObjectsOfType<MapSquare>();

        foreach (MapSquare square in allSquaresArray)
        {
            if (square.IsActive() && square.DoesSquareHaveTransportationNode() && !square.GetIsTransportationNodeUnlocked())
            {
                square.OpenMetroStation();
            }
        }
    }

    private void MapVentilationSystem(MapSquare currentSquare)
    {
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        squares.Add(currentSquare);

        foreach (MapSquare square in squares)
        {
            square.MapVentilation();
        }
    }

    private void EngageStealthTech()
    {
        FindObjectOfType<MapGrid>().RaiseStealthMovement(1);
    }

    private void HinderEnemyMovement(int amount)
    {
        FindObjectOfType<MapData>().RaiseEnemyHindrance(amount);
    }

    private void StopEnemyMovement(int amount)
    {
        FindObjectOfType<MapData>().RaiseBlockEnemySpawn(amount);
    }

    private void GainHealthRegeneration(int duration)
    {
        FindObjectOfType<MapData>().AddDurationToHealthRegen(duration);
    }

    private void GainEnergyRegeneration(int duration)
    {
        FindObjectOfType<MapData>().AddDurationToEnergyRegen(duration);
    }

    private void GainHealth(int percentToGain)
    {
        FindObjectOfType<MapData>().GainHealth(percentToGain);
    }

    private void GainEnergy(int percentToGain)
    {
        FindObjectOfType<MapData>().GainEnergy(percentToGain);
    }
}
