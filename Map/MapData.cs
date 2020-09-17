using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    // config
    CharacterData runner;
    HackerData hacker;
    Job.JobArea mapType;
    MapConfig mapConfig;
    [SerializeField] float setupTimeInSeconds = 1f;
    MapGrid mapGrid;
    int mapSize;

    // state
    int securityLevel;
    int enemyHindrance = 0;
        // enemyHindrance helps to hinder enemy spawn, each tick lowers the level by one.
        // Capped at 40%. Spawn chance minimum 5% so matter what this value is
    int enemySpawnBlock = 0;
        // enemySpawnBlock blocks a successful spawn, then goes down by one. If zero- doesn't function
    int playerHealthRegenDuration = 0;
    int playerEnergyRegenDuration = 0;
    bool hasGoalBeenReached = false;
        // remember if the player has or has not reached the goal
    bool wasPlayerOnGoalBeforeCombat = false;
        // remember if the player was able to trigger the goal state before loading combat. Ditto for extraction
    bool wasPlayerOnExtractionBeforeCombat = false;

    // reward stuff
    int creditsEarned; // in match
    int creditsMultiplier; // percent multiplier added to money earned in round
    int goalMultiplier; // percent multiplier added to money earned at end of round

    // player powerups
    int handSizeBoostChance = 0;

    private void Awake()
    {
        int count = FindObjectsOfType<MapData>().Length;
        creditsEarned = 0;
        creditsMultiplier = 0;
        goalMultiplier = 0;

        if (count > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void MovePlayer(MapSquare currentSquare, MapSquare targetSquare)
    {
        PlayerMarker playerMarker = FindObjectOfType<PlayerMarker>();
        playerMarker.SetTargetPosition(targetSquare.GetPlayerMarkerPosition());
        playerMarker.moveTowardTargetPosition();
        playerMarker.SetCurrentSquare(targetSquare);

        currentSquare.UnsetPlayerPosition();
        targetSquare.SetPlayerPosition();
    }

    public void PlayerFinishesMoving(MapSquare currentSquare)
    {
        CheckMapGridExists();
        bool moveOnToBattle = PostMovementActions(currentSquare);
        bool trapSprung = false;
        bool goalReady = false;
        bool extractionReady = false;

        if (moveOnToBattle)
        {
            if (currentSquare.GetIsGoal() && !hasGoalBeenReached)
            {
                wasPlayerOnGoalBeforeCombat = true;
            } else if (currentSquare.GetIsExtraction())
            {
                wasPlayerOnExtractionBeforeCombat = true;
            }
            StartBattleIfEnemyExists(currentSquare);
        } else
        {
            if (currentSquare.GetIsGoal() && !hasGoalBeenReached && currentSquare.GetEnemy() == null)
            {
                goalReady = true;
            } else if (currentSquare.GetIsExtraction() && currentSquare.GetEnemy() == null)
            {
                extractionReady = true;
            }
            mapConfig.GetTrapSpringMenu().OpenMenu(currentSquare, goalReady, extractionReady);
            trapSprung = true;
        }

        if (currentSquare.GetIsGoal() && !hasGoalBeenReached && currentSquare.GetEnemy() == null && !trapSprung)
        {
            FindObjectOfType<MapConfig>().GetGoalWindow().OpenGoalWindow(currentSquare);
        } else if (currentSquare.GetIsExtraction() && currentSquare.GetEnemy() == null && !trapSprung)
        {
            FindObjectOfType<MapConfig>().GetExtractionWindow().OpenExtractionWindow();
        }
    }

    public void StartBattleIfEnemyExists(MapSquare currentSquare)
    {
        if (currentSquare.GetEnemy() != null && mapGrid.GetStealthMovement() < 1)
        {
            StartBattle(currentSquare);
        }
        else if (currentSquare.GetEnemy() != null && mapGrid.GetStealthMovement() > 0)
        {
            mapGrid.UseAStealthCharge();
        }
    }

    private void CheckMapGridExists()
    {
        if (mapGrid == null)
            mapGrid = FindObjectOfType<MapGrid>();
    }

    public void StartBattle(MapSquare currentSquare)
    {
        FindObjectOfType<SceneLoader>().LoadBattleFromMap(currentSquare);
    }

    private bool PostMovementActions(MapSquare currentSquare)
    {
        CheckRegen();
        AttemptToSpawnEnemy();
        RaiseSecurityLevel(currentSquare);
        bool didTrapsSpring = TrapsSpring(currentSquare);

        if (didTrapsSpring)
        {
            return false;
        } else
        {
            return true;
        }
    }

    private void CheckRegen()
    {
        if (playerHealthRegenDuration > 0)
        {
            float maxHealth = runner.GetMaximumHealth();
            int amountToHeal = Mathf.FloorToInt(maxHealth * 0.10f);
            if (amountToHeal < 1)
            {
                amountToHeal = 1;
            }

            runner.GainHealthOnMap(amountToHeal);
            playerHealthRegenDuration--;
        }

        if (playerEnergyRegenDuration > 0)
        {
            float maxEnergy = runner.GetMaximumEnergy();
            int amountToGain = Mathf.FloorToInt(maxEnergy * 0.05f);
            if (amountToGain < 1)
            {
                amountToGain = 1;
            }

            runner.GainEnergyOnMap(amountToGain);
            playerEnergyRegenDuration--;
        }
    }

    private bool TrapsSpring(MapSquare square)
    {
        List<MapObject> mapObjects = square.GetMapObjects();
        foreach (MapObject mapObject in mapObjects)
        {
            if (mapObject.GetObjectType() == "Trap" && mapObject.GetIsActive() == true)
            {
                mapObject.TriggerTrap();
                return true;
            }
        }
        return false;
    }

    private void AttemptToSpawnEnemy()
    {
        int enemySpawnChance = securityLevel;
        if (enemyHindrance > 0)
        {
            Debug.Log("Enemy percent spawn hindrance");
            enemySpawnChance -= enemyHindrance;
            if (enemySpawnChance < 5)
            {
                // We cap the reductions to spawn chance at 5% for now...
                enemySpawnChance = 5;
            }
            enemyHindrance--;
        }

        if (Random.Range(0, 100) <= enemySpawnChance)
        {
            // WRAP THIS IN A % CHANCE CHECK
            if (enemySpawnBlock > 0)
            {
                enemySpawnBlock--;
                Debug.Log("Blocked spawning an enemy");
            }
            else
            {
                Debug.Log("Spawn an Enemy");
                mapGrid.AttemptToSpawnAnEnemy(securityLevel);
                // TODO: Spawn an enemy?
            }
        } else
        {
            Debug.Log("Enemy spawn failed...");
        }
    }

    private void RaiseSecurityLevel(MapSquare currentSquare)
    {
        if (currentSquare.GetIsVentilationMapped())
        {
            Debug.Log("Player Using Vents, Less or No Security Penalty");
            // TODO: THIS???
        } else
        {
            Debug.Log("Raise Security Level");
            AdjustSecurityLevel(5);
        }
    }

    public void SetUpMap()
    {
        SetupPlayerPortraits();
        runner.MapSetup();

        mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.SetupPipManagers(runner, setupTimeInSeconds, securityLevel);

        mapGrid = FindObjectOfType<MapGrid>();
        mapGrid.SetupGrid(mapType, mapSize);
    }

    public void SetUpMapFromBattle()
    {
        SetupPlayerPortraits();
        runner.MapSetup();

        mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.SetupPipManagers(runner, setupTimeInSeconds, securityLevel);

        MapSquare[] mapSquares = FindObjectsOfType<MapSquare>();
        foreach (MapSquare square in mapSquares)
        {
            if (square.GetIsPlayerPresent())
            {
                square.SetPlayerStart();
                break;
            }
        }
    }

    public void SetUpMapFromHack(MapSquare currentSquare, HackTarget currentHackTarget)
    {
        SetupPlayerPortraits();
        runner.MapSetup();

        mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.SetupPipManagers(runner, setupTimeInSeconds, securityLevel);

        MapSquare[] mapSquares = FindObjectsOfType<MapSquare>();
        foreach (MapSquare square in mapSquares)
        {
            if (square.GetIsPlayerPresent())
            {
                square.SetPlayerStart();
                break;
            }
        }

        currentSquare.ReopenHackMenu(currentHackTarget);
    }

    public void SetMapData(CharacterData characterToSet, HackerData hackerToSet, Job.JobArea newMapType, int newSecurityLevel, int newMapSize)
    {
        runner = characterToSet;
        hacker = hackerToSet;
        mapType = newMapType;
        securityLevel = newSecurityLevel;
        mapSize = newMapSize;
    }

    private void SetupPlayerPortraits()
    {
        PlayerPortrait[] portraits = FindObjectsOfType<PlayerPortrait>();
        foreach (PlayerPortrait portrait in portraits)
        {
            if (portrait.name == "RunnerPortrait")
            {
                portrait.SetRunnerPortrait(runner.getId());
            }
            else if (portrait.name == "HackerPortrait")
            {
                portrait.SetHackerPortrait(hacker.GetId());
            }
        }
    }

    public void AdjustSecurityLevel(int adjustment)
    {
        securityLevel += adjustment;
        if (securityLevel < 0)
            securityLevel = 0;
        if (securityLevel > 100)
            securityLevel = 100;
        mapConfig.GetSecurityPipManager().ChangeValue(securityLevel);
    }

    public void ChangeMoney(int amount)
    {
        float percentMultiplier = creditsMultiplier / 100;
        float multipliedAmount = amount * percentMultiplier;
        creditsEarned += amount + Mathf.FloorToInt(multipliedAmount);
    }

    public int GetEarnedMoneyAmount()
    {
        return creditsEarned;
    }

    public void ChangeMoneyMultiplier(int amount)
    {
        creditsMultiplier += amount;
    }

    public void ChangeGoalMultiplier(int amount)
    {
        goalMultiplier += amount;
    }

    private void CheckMapConfigExists()
    {
        if (mapConfig == null)
        {
            mapConfig = FindObjectOfType<MapConfig>();
        }
    }

    public float GetSetupTimeInSeconds()
    {
        return setupTimeInSeconds;
    }

    public Job.JobArea GetMapType()
    {
        return mapType;
    }

    public void RaiseEnemyHindrance(int amount)
    {
        enemyHindrance += amount;
        // We max this value out at 40% for now
        if (enemyHindrance > 40)
        {
            enemyHindrance = 40;
        }
    }

    public void RaiseBlockEnemySpawn(int amount)
    {
        enemySpawnBlock += amount;
    }

    public void AddDurationToHealthRegen(int amount)
    {
        playerHealthRegenDuration += amount;
    }

    public void AddDurationToEnergyRegen(int amount)
    {
        playerEnergyRegenDuration += amount;
    }

    public void GainHealth(int percentToGain)
    {
        float maxHealth = runner.GetMaximumHealth();
        float amount = (float)percentToGain / 100;
        int amountToHeal = Mathf.FloorToInt(maxHealth * amount);
        if (amountToHeal < 1)
            amountToHeal = 1;
        runner.GainHealthOnMap(amountToHeal);
    }

    public void GainEnergy(int percentToGain)
    {
        float maxEnergy = runner.GetMaximumEnergy();
        float amount = (float)percentToGain / 100;
        int amountToGain = Mathf.FloorToInt(maxEnergy * amount);
        if (amountToGain < 1)
            amountToGain = 1;
        runner.GainEnergyOnMap(amountToGain);
    }

    public CharacterData GetRunner()
    {
        return runner;
    }

    public void RaiseHandSizeBoostChance()
    {
        if (handSizeBoostChance <= 0)
        {
            handSizeBoostChance = 25;
        } else
        {
            float boost = (float)handSizeBoostChance * 0.5f;
            handSizeBoostChance = handSizeBoostChance + Mathf.FloorToInt(boost);
        }
        Debug.Log("Hand size boost chance: " + handSizeBoostChance.ToString());
    }

    public int GetHandSizeBoostChance()
    {
        return handSizeBoostChance;
    }

    public bool ShouldGoalWindowOpenAfterCombat()
    {
        if (!hasGoalBeenReached && wasPlayerOnGoalBeforeCombat)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void SetHasGoalBeenReached(bool newSetting)
    {
        hasGoalBeenReached = newSetting;
    }

    public bool GetHasGoalBeenReached()
    {
        return hasGoalBeenReached;
    }

    public void SetWasPlayerOnGoalBeforeCombat(bool newSetting)
    {
        wasPlayerOnGoalBeforeCombat = newSetting;
    }

    public void SetWasPlayerOnExtractionBeforeCombat(bool setting)
    {
        wasPlayerOnExtractionBeforeCombat = setting;
    }

    public bool GetShouldExtractionWindowOpenAfterCombat()
    {
        return wasPlayerOnExtractionBeforeCombat;
    }
}
