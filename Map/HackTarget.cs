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
    const string green = "green";

    // if hackType is Transportation
    string transportationType = null;
    bool isStationOpen = false;

    // state
    int redPoints;
    int bluePoints;
    int greenPoints;
    bool isActive;
    bool hackIsDone;
    bool canPlayerAffordAnything;

    public void SetupHackTarget(string newHackType)
    {
        redPoints = 0;
        bluePoints = 0;
        greenPoints = 0;
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
        greenPoints = 500;
        canPlayerAffordAnything = true;
        hackIsDone = true;
    }

    public void SetPoints(int newRedPoints, int newBluePoints, int newGreenPoints)
    {
        redPoints = newRedPoints;
        bluePoints = newBluePoints;
        greenPoints = newGreenPoints;

        //redPoints = 99;
        //bluePoints = 99;
        //greenPoints = 99;

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

    public int GetGreenPoints()
    {
        return greenPoints;
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
            case green:
                if (greenPoints >= cost)
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

    public string GetHackHelperText(int count)
    {
        switch (hackType)
        {
            case "Security Camera":
                return securityCameraHelperText[count];
            case "Combat Server":
                return combatServerHelperText[count];
            case "Database":
                return databaseHelperText[count];
            case "Defense System":
                return defenseSystemsHelperText[count];
            case "Transportation":
                return transportationHelperText[count];
            case "Medical Server":
                return medicalServerHelperText[count];
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
    string[] securityCameraHelperText =
    {
        "Scout the presence of hack targets and points of interest in all squares up to 2 spaces away",
        "Lower the security level",
        "Fully reveal the hack targets and points of interest in all squares up to 2 spaces away",
        "Scout the presence of enemies in all squares up to 2 spaces away",
        "Fully reveal enemies in all squares up to 2 spaces away",
        "Despawn a weak enemy from a random square",
        "Despawn an enemy of average strength from a random square"
    };
    string[] securityCameraColors = { blue, blue, blue, red, red, green, green };
    int[] securityCameraCosts = { 5, 10, 15, 10, 20, 10, 15 };

    string[] combatServerOptions =
    {
        "Accelerate Reaction Time",
        "Disengage Enemy Shielding",
        "Enhance Targetting",
        "Overclock Systems",
        "Harden Armor",
        "Install Mod Interference",
        "Sabotage Enemy Tech"
    };
    string[] combatServerHelperText =
    {
        "Gain 10% Dodge chance on this and adjacent squares",
        "Enemies on this and adjacent squares take +1 damage",
        "Gain 15% Crit Chance when on this and adjacent squares",
        "Hand size increased by 1 on this and adjacent squares",
        "Take -1 damage on this and adjacent squares",
        "Enemies on this and adjacent squares have -1 hand size (minimum 1)",
        "Cards enemies play on this and adjacent squares have a 15% chance to have no effect"
    };
    string[] combatServerColors = { blue, blue, red, red, red, green, green };
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
    string[] databaseHelperText =
    {
        "Reduce Security Level",
        "Gain extra credits upon Job completion",
        "Base reward and bonus credits earned are increased by 10%",
        "Gain credits, but also raise the security level",
        "Gain extra credits upon Job completion",
        "Increase your base contract reward by 20% upon Job completion",
        "Increase your base contract reward by 40% upon Job completion"
    };
    string[] databaseColors = { blue, blue, blue, red, red, green, green };
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
    string[] defenseSystemsHelperText =
    {
        "Disable the nearest trap",
        "Lower the security level",
        "Cards enemies play on this and adjacent squares have a 15% chance to have no effect",
        "Deal damage to enemies in adjacent squares equal to 15% of their health",
        "During combat on this and adjacent squares, enemies will take 5% of their health in damage every turn",
        "Enemies on adjacent squares deal -1 damage in combat",
        "Despawn a strong enemy somewhere on the map"
    };
    string[] defenseSystemColors = { blue, blue, blue, red, red, green, green };
    int[] defenseSystemCosts = { 10, 10, 15, 10, 20, 10, 15 };

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
    string[] transportationHelperText =
    {
        "Moving to this and adjacent squares will not raise the security level",
        "The next time you move onto a square with an enemy, avoid combat",
        "Open the metro station on this square. Metro stations allow you to move to other open metro stations.",
        "Open a random metro station on another square. Metro stations allow you to move to other open metro stations.",
        "Open all metro stations on the map. Metro stations allow you to move to other open metro stations.",
        "Enemies have a reduced chance to spawn",
        "The next time an enemy would spawn, block that spawn"
    };
    string[] transportationColors = { blue, blue, red, red, red, green, green };
    int[] transportationCosts = { 10, 20, 5, 10, 25, 10, 10 };

    string[] medicalServerOptions = {
        "Gain Health and Energy Regeneration",
        "Sabotage Medicines",
        "Upload and Sell Medical Research",
        "Healing Boost",
        "Energy Infusion",
        "Heal and Gain Energy",
        "Injest Stimulants"
    };
    string[] medicalServerHelperText =
    {
        "Every time you move, regain 10% health and energy. Lasts for the next 5 moves.",
        "During combat on this and adjacent squares, enemies will take 5% of their health in damage every turn",
        "Gain extra credits upon Job completion",
        "Heal for 25% of your maximum health",
        "Refill your energy",
        "Heal for 25% of your maximum health and gain 50% energy",
        "On this and adjacent squares, gain 10% dodge, +1 hand size, and 15% Crit Chance"
    };
    string[] medicalServerColors = { blue, blue, blue, red, red, green, green };
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
                AddToEnemyFizzleChance(15, square);
                break;

            // DATABASE OPTIONS
            case "Erase Database":
                ReduceSecurityLevel(5);
                break;
            case "Upload and Sell Financial Data":
                int jobDifficulty = FindObjectOfType<MapData>().GetJob().GetJobDifficulty();
                GainMoney(Random.Range(50, 101) * (jobDifficulty + 1));
                break;
            case "Install Backdoor":
                RaiseMoneyMultiplier(10);
                RaiseGoalMultiplier(10);
                break;
            case "Brute Force Passwords":
                jobDifficulty = FindObjectOfType<MapData>().GetJob().GetJobDifficulty();
                GainMoney(Random.Range(1, 51) * (jobDifficulty + 1));
                RaiseSecurityLevel(7);
                break;
            case "Upload and Sell Personal Data":
                jobDifficulty = FindObjectOfType<MapData>().GetJob().GetJobDifficulty();
                GainMoney(Random.Range(75, 151) * (jobDifficulty + 1));
                break;
            case "Download Buyer List":
                RaiseGoalMultiplier(20);
                break;
            case "Download VIP Buyer List":
                RaiseGoalMultiplier(40);
                break;

            // DEFENSE SYSTEM OPTIONS
            case "Disable Trap":
                DisableATrap(square);
                break;
            //case "Reduce Security Level":
                // THIS IS ALREADY PART OF THE ABOVE, AND IT WORKS FOR BOTH
                // break;
            case "Detonate EMP":
                AddToEnemyFizzleChance(15, square);
                break;
            case "Turn Turrets on Enemies":
                DamageEnemies(15, square);
                break;
            case "Control Attack Drones":
                DotEnemies(5, square);
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
                HinderEnemyMovement(5);
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
                DotEnemies(5, square);
                break;
            case "Upload and Sell Medical Research":
                jobDifficulty = FindObjectOfType<MapData>().GetJob().GetJobDifficulty();
                GainMoney(Random.Range(50, 101) * (jobDifficulty + 1));
                break;
            case "Healing Boost":
                GainHealth(25);
                break;
            case "Energy Infusion":
                GainEnergy(100);
                break;
            case "Heal and Gain Energy":
                GainHealth(25);
                GainEnergy(50);
                break;
            case "Injest Stimulants":
                BuffPlayerDodge(10, square);
                BuffPlayerHandSize(1, square);
                BuffPlayerCritChance(15, square);
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
            case green:
                greenPoints -= cost;
                break;
        }
    }

    private void ScoutPointOfInterest(int newScoutLevel, MapSquare square)
    {
        List<MapSquare> adjacentSquares = square.GetAdjacentSquares();

        List<MapSquare> squaresToSearch = new List<MapSquare>();
        squaresToSearch.AddRange(adjacentSquares);

        foreach(MapSquare adjSquare in adjacentSquares)
        {
            squaresToSearch.AddRange(adjSquare.GetAdjacentSquares());
        }

        foreach (MapSquare squareToSearch in squaresToSearch)
        {
            squareToSearch.SetPOIScoutLevel(newScoutLevel);
        }
    }

    private void ScoutEnemies(int newScoutLevel, MapSquare square)
    {
        List<MapSquare> adjacentSquares = square.GetAdjacentSquares();

        List<MapSquare> squaresToSearch = new List<MapSquare>();
        squaresToSearch.AddRange(adjacentSquares);

        foreach (MapSquare adjSquare in adjacentSquares)
        {
            squaresToSearch.AddRange(adjSquare.GetAdjacentSquares());
        }

        foreach(MapSquare squareToSearch in squaresToSearch)
        {
            squareToSearch.SetEnemyScoutLevel(newScoutLevel);
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
        currentSquare.AdjustPlayerDodge(amount);
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustPlayerDodge(amount);
        }
    }

    private void IncreaseEnemyVulnerability(int amount, MapSquare currentSquare)
    {
        currentSquare.AdjustEnemyVulnerability(amount);
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustEnemyVulnerability(amount);
        }
    }

    private void BuffPlayerCritChance(int amount, MapSquare currentSquare)
    {
        currentSquare.AdjustPlayerCritChance(amount);
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustPlayerCritChance(amount);
        }
    }

    private void BuffPlayerHandSize(int amount, MapSquare currentSquare)
    {
        currentSquare.AdjustPlayerHandSize(amount);
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustPlayerHandSize(amount);
        }
    }

    private void BuffPlayerDefense(int amount, MapSquare currentSquare)
    {
        currentSquare.AdjustPlayerDefenseBuff(amount);
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustPlayerDefenseBuff(amount);
        }
    }

    private void DebuffEnemyHandSize(int amount, MapSquare currentSquare)
    {
        currentSquare.AdjustEnemyHandSizeDebuff(amount);
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustEnemyHandSizeDebuff(amount);
        }
    }

    private void AddToEnemyFizzleChance(int amount, MapSquare currentSquare)
    {
        currentSquare.AdjustEnemyFizzleChance(amount);
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustEnemyFizzleChance(amount);
        }
    }

    private void DisableATrap(MapSquare currentSquare)
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

        // Sort to get the nearest trap
        squaresWithTraps.Sort(SortByDistance);

        if (squaresWithTraps.Count > 0)
        {
            MapSquare squareToDisable = squaresWithTraps[0];
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

    private int SortByDistance(MapSquare square1, MapSquare square2)
    {
        return square1.GetDistanceMeasurement().CompareTo(square2.GetDistanceMeasurement());
    }

    private void DamageEnemies(int percentage, MapSquare currentSquare)
    {
        currentSquare.AdjustPercentDamageToEnemy(percentage);
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustPercentDamageToEnemy(percentage);
        }
    }

    private void DotEnemies(int amount, MapSquare currentSquare)
    {
        currentSquare.AdjustDotDamageToEnemy(amount);
        List<MapSquare> squares = currentSquare.GetAdjacentSquares();
        foreach (MapSquare square in squares)
        {
            square.AdjustDotDamageToEnemy(amount);
        }
    }

    private void DebuffEnemyDamage(int amount, MapSquare currentSquare)
    {
        currentSquare.AdjustEnemyDamageDebuff(amount);
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
