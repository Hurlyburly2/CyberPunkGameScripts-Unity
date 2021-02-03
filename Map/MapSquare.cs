using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquare : MonoBehaviour
{
    //config
    [SerializeField] int rowPosition; // 0-19 position in row
    [SerializeField] MapSquareRow parentRow;
    [SerializeField] MetroButton metroButton;
    [SerializeField] SpriteRenderer extractionMarker;
    int minSquareRow = 0;
    int maxSquareRow = 19;
    int minSquareColumn = 0;
    int maxSquareColumn = 19;
    float defaultYPos;
    float targetYPos;
    float squareMoveSpeed = 1f;
    Color defaultColor;
    MapConfig mapConfig;
    Sprite locationImage;

    //state
        // state can be normal, movingDown
    bool isActive;
    bool isGoal = false;
    bool isStart = false;
    bool isExtraction = false;
    int temporaryDistanceMeasurement;
    string state;
    bool playerPresent;
    bool shroud;
    int poiScoutLevel;
    int enemyScoutLevel;
    float step;
    // 1 = normal (default) no knowledge, 2 = know how many items, 3 = know what everything is
    bool hasTransportationNode = false;
    bool isTransportationNodeUnlocked = false;
    bool isVentilationMapped = false;

    bool explored = false; // has player visited this square? Used primarily to determine enemy spawns

    MapObject.TrapTypes triggeredTrapType;
    int triggeredTrapAmount;

    // objects and hacks
    List<HackTarget> hackTargets;
    List<MapObject> mapObjects;
    List<string> availableHackTypes;
    List<string> availableObjectTypes;

    // Enemy
    Enemy enemy;
    // TODO: THESE BUFFS AND DEBUFFS ARE CURRENTLY ALWAYS EMPTY
    List<int> enemyBuffs;
    List<int> enemyDebuffs;

    // player/enemy status effects. New effects here need to also be represented in the
    // ShouldEffectsButtonAppear() and GetEffectsListStrings() methods
    int playerDodgeBuff = 0;
    int enemyVulnerability = 0;
    int playerCritBuff = 0;
    int playerHandSizeBuff = 0;
    int playerDefenseBuff = 0;
    int enemyHandSizeDebuff = 0;
    int enemyFizzleChance = 0;
    int percentDamageToEnemy = 0;
    int dotDamageToEnemy = 0;
    int enemyDamageDebuff = 0;

    private void OnMouseUpAsButton()
    {
        if (mapConfig == null)
        {
            mapConfig = FindObjectOfType<MapConfig>();
        }
        if (!shroud && !mapConfig.GetIsAMenuOpen())
        {
            if (playerPresent)
            {
                mapConfig.SetIsAMenuOpen(true);
                FindObjectOfType<MapSFX>().PlayMapSoundSFX(MapSFX.MapSoundEffect.MapSquarePress);
                CurrentNodeMenu menu = mapConfig.GetCurrentNodeMenu();
                menu.InitializeMenu(this);
            } else
            {
                mapConfig.SetIsAMenuOpen(true);
                FindObjectOfType<MapSFX>().PlayMapSoundSFX(MapSFX.MapSoundEffect.MapSquarePress);
                NeighboringNodeMenu menu = mapConfig.GetNeighboringNodeMenu();
                menu.InitializeMenu(this);
            }
        }
    }

    public int GetDistanceToTargetSquare(MapSquare targetSquare)
    {
        int x = rowPosition;
        int y = parentRow.GetRowNumber();

        int targetX = targetSquare.GetRowPosition();
        int targetY = targetSquare.GetParentRow().GetRowNumber();

        int distance = Mathf.Abs(x - targetX) + Mathf.Abs(y - targetY);
        return distance;
    }

    public void OpenMetroStation()
    {
        metroButton.gameObject.SetActive(true);
        isTransportationNodeUnlocked = true;
        Debug.Log("Open the metro station...");
    }

    public void ReopenHackMenu(HackTarget hackTarget)
    {
        if (mapConfig == null)
        {
            mapConfig = FindObjectOfType<MapConfig>();
        }
        mapConfig.SetIsAMenuOpen(true);
        CurrentNodeMenu menu = mapConfig.GetCurrentNodeMenu();
        menu.InitializeMenu(this);
        menu.ReopenHackMenu(hackTarget);
    }

    private void OnMouseDown()
    {
        mapConfig = FindObjectOfType<MapConfig>();
        if (!shroud && !mapConfig.GetIsAMenuOpen())
        {
            SetState("movingDown");
        }
    }

    private void OnMouseUp()
    {
        if (!shroud)
        {
            SetState("movingUp");
        }
    }

    public List<MapSquare> GetAdjacentSquares()
    {
        List<MapSquare> adjacentSquares = new List<MapSquare>();

        if (rowPosition > minSquareRow)
        {
            adjacentSquares.Add(parentRow.GetSquareByRowPosition(rowPosition - 1));
        }
        if (rowPosition < maxSquareRow)
        {
            adjacentSquares.Add(parentRow.GetSquareByRowPosition(rowPosition + 1));
        }
        if (parentRow.GetRowNumber() > minSquareColumn)
        {
            adjacentSquares.Add(parentRow.GetMapGrid().GetRowByNumber(parentRow.GetRowNumber() - 1).GetSquareByRowPosition(rowPosition));
        }
        if (parentRow.GetRowNumber() < maxSquareColumn)
        {
            adjacentSquares.Add(parentRow.GetMapGrid().GetRowByNumber(parentRow.GetRowNumber() + 1).GetSquareByRowPosition(rowPosition));
        }

        return adjacentSquares;
    }

    public MapSquare GetUpSquare()
    {
        if (parentRow.GetRowNumber() < maxSquareColumn)
        {
            return parentRow.GetMapGrid().GetRowByNumber(parentRow.GetRowNumber() + 1).GetSquareByRowPosition(rowPosition);
        }
        return null;
    }

    public void SetPlayerStart()
    {
        // If any player marker still exists, destroy it
        PlayerMarker[] oldPlayerMarkers = FindObjectsOfType<PlayerMarker>();
        foreach (PlayerMarker oldMarker in oldPlayerMarkers)
            Destroy(oldMarker.gameObject);

        SetPlayerPosition();
        PlayerMarker playerMarkerPrefab = parentRow.GetMapGrid().GetPlayerMarkerPrefab();
        Vector3 markerPosition = GetPlayerMarkerPosition();
        PlayerMarker newPlayerMarker = Instantiate(playerMarkerPrefab, markerPosition, Quaternion.identity);
        newPlayerMarker.SetCurrentSquare(this);
    }

    public Vector3 GetPlayerMarkerPosition()
    {
        return new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
    }

    public void UnsetPlayerPosition()
    {
        playerPresent = false;
    }

    public void SetPlayerPosition()
    {
        Debug.Log("player present()");
        playerPresent = true;
        explored = true;
        enemyScoutLevel = 3;
        poiScoutLevel = 3;
        RemoveShroud();
        RemoveAdjacentShrouds();
    }

    public void InitializeSquare(Sprite newImage, Sprite newLocationImage, bool isFirstSquare)
    {
        enemy = null;
        enemyScoutLevel = 1;
        poiScoutLevel = 1;
        playerPresent = false;
        isActive = true;
        GetComponent<PolygonCollider2D>().enabled = true;
        parentRow.AddInitializedSquareToList(this);
        GetComponent<SpriteRenderer>().sprite = newImage;
        locationImage = newLocationImage;
        enemyBuffs = new List<int>();
        enemyDebuffs = new List<int>();

        SetupHacksAndObjects(isFirstSquare);
    }

    private void SetupHacksAndObjects(bool isFirstSquare)
    {
        SetupHackObjectSpawnLists(isFirstSquare);

        // random 1-3
        int objectsToSpawn = Random.Range(1, 4);
        while (objectsToSpawn > 0)
        {
            int random = Random.Range(0, 100);
            if (random <= 45)
            {
                HackTarget newHackTarget = ScriptableObject.CreateInstance<HackTarget>();
                string newHackType = availableHackTypes[Random.Range(0, availableHackTypes.Count)];

                if (newHackType == "Transportation")
                {
                    hasTransportationNode = true;
                    parentRow.GetMapGrid().AddATransportationNode();
                }

                newHackTarget.SetupHackTarget(newHackType);
                availableHackTypes.Remove(newHackType);

                hackTargets.Add(newHackTarget);
                objectsToSpawn--;
            } else if (random > 45)
            {
                MapObject newMapObject = ScriptableObject.CreateInstance<MapObject>();
                string newMapObjectType = availableObjectTypes[Random.Range(0, availableObjectTypes.Count)];
                newMapObject.SetupMapObject(newMapObjectType);
                availableObjectTypes.Remove(newMapObjectType);

                mapObjects.Add(newMapObject);
                objectsToSpawn--;
            }
        }
    }

    public void SpawnTransportationNode()
    {
        HackTarget newHackTarget = ScriptableObject.CreateInstance<HackTarget>();
        newHackTarget.SetupHackTarget("Transportation");
        hackTargets.Add(newHackTarget);
    }

    public void SpawnEnemy(Job.JobArea mapType, int securityLevel, Job job, bool isBoss=false)
    {
        // TODO: THIS SHOULD ALSO TAKE INTO ACCOUNT DIFFICULTY AND SECURITY LEVEL
        enemy = FindObjectOfType<EnemyCollection>().GetAnEnemyByArea(mapType, securityLevel, job, isBoss);
        //EmptyEnemyForTesting();
    }

    private void EmptyEnemyForTesting()
    {
        enemy = null;
    }

    public void DespawnEnemy()
    {
        enemy = null;
    }

    private void SetupHackObjectSpawnLists(bool isFirstSquare)
    {
        hackTargets = new List<HackTarget>();
        mapObjects = new List<MapObject>();

        availableHackTypes = new List<string>();
        string[] hackTypes = { "Security Camera", "Combat Server", "Database", "Defense System", "Transportation", "Medical Server" };
        availableHackTypes.AddRange(hackTypes);

        availableObjectTypes = new List<string>();
        string[] objectTypes = { "Trap", "Reward", "PowerUp", "PowerUp", "Upgrade", "First Aid Station" };
        availableObjectTypes.AddRange(objectTypes);

        if (isFirstSquare)
        {
            availableObjectTypes.Remove("Trap");
        }
    }

    public void AddShroud()
    {
        shroud = true;
        GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 0.25f, 1);
    }

    public void RemoveShroud()
    {
        shroud = false;
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    private void RemoveAdjacentShrouds()
    {
        List<MapSquare> adjacentSquares = GetAdjacentSquares();
        foreach (MapSquare square in adjacentSquares)
        {
            square.RemoveShroud();
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void InitialSetup(int layerNumber)
    {
        GetComponent<SpriteRenderer>().sortingOrder = layerNumber;
        GetComponent<PolygonCollider2D>().enabled = false;
        defaultYPos = transform.position.y;
        targetYPos = defaultYPos - 0.35f;
        state = "normal";
        isActive = false;
    }

    private void Start()
    {
        step = 2.5f * Time.deltaTime;
        defaultColor = GetComponent<SpriteRenderer>().color;
        AddShroud();
        mapConfig = FindObjectOfType<MapConfig>();
    }

    void Update()
    {
        if (state == "movingDown")
        {
            if (Input.touchCount == 3 || Input.touchCount == 2)
            {
                // the click was accidental, user actually means to drag the camera if they're
                // touching with three fingers, or to zoom if they're touching with two
                SetState("movingUp");
            }
            Vector3 targetPos = new Vector3(transform.position.x, targetYPos, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        } else if (state == "movingUp")
        {
            Vector3 targetPos = new Vector3(transform.position.x, defaultYPos, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            if (transform.position.y == defaultYPos)
            {
                SetState("normal");
            }
        }
    }

    public void SetPOIScoutLevel(int newScoutLevel)
    {
        if (newScoutLevel > poiScoutLevel)
        {
            poiScoutLevel = newScoutLevel;
            if (poiScoutLevel > 1)
            {
                RemoveShroud();
            }
        }
    }

    public void SetEnemyScoutLevel(int newScoutLevel)
    {
        if (newScoutLevel > enemyScoutLevel)
        {
            enemyScoutLevel = newScoutLevel;
            if (enemyScoutLevel > 1)
            {
                RemoveShroud();
            }
        }
    }

    private void SetState(string newState)
    {
        state = newState;
    }

    public void AdjustPlayerDodge(int amount)
    {
        // max player dodge is currently 80
        if (playerDodgeBuff + amount <= 80)
        {
            playerDodgeBuff += amount;
        } else
        {
            playerDodgeBuff = 80;
        }
    }

    public void AdjustEnemyVulnerability(int amount)
    {
        enemyVulnerability += amount;
    }

    public void AdjustPlayerCritChance(int amount)
    {
        playerCritBuff += amount;
    }

    public void AdjustPlayerHandSize(int amount)
    {
        playerHandSizeBuff += amount;
    }

    public void AdjustPlayerDefenseBuff(int amount)
    {
        playerDefenseBuff += amount;
    }

    public void AdjustEnemyHandSizeDebuff(int amount)
    {
        enemyHandSizeDebuff += amount;
    }

    public void AdjustEnemyFizzleChance(int amount)
    {
        enemyFizzleChance = Mathf.Clamp(enemyFizzleChance + amount, 0, 75);
    }

    public void AdjustPercentDamageToEnemy(int amount)
    {
        if (amount + percentDamageToEnemy <= 50)
        {
            percentDamageToEnemy += amount;
        } else
        {
            percentDamageToEnemy = 50;
        }
    }

    public void AdjustDotDamageToEnemy(int amount)
    {
        dotDamageToEnemy += amount;
    }

    public void AdjustEnemyDamageDebuff(int amount)
    {
        enemyDamageDebuff += amount;
    }

    public List<int> GetPackageOfModifiers()
    {
        // list order: playerDodge, enemyVulnerability, playerCrit, playerHandSize, playerDefenseBuff
            // percentDamageToEnemy, dotDamageToEnemy, enemyDamageDebuff
        List<int> mapModifiers = new List<int>();
        mapModifiers.Add(playerDodgeBuff);
        mapModifiers.Add(enemyVulnerability);
        mapModifiers.Add(playerCritBuff);
        mapModifiers.Add(playerHandSizeBuff);
        mapModifiers.Add(playerDefenseBuff);
        mapModifiers.Add(enemyHandSizeDebuff);
        mapModifiers.Add(enemyFizzleChance);
        mapModifiers.Add(percentDamageToEnemy);
        mapModifiers.Add(dotDamageToEnemy);
        mapModifiers.Add(enemyDamageDebuff);

        return mapModifiers;
    }

    public Sprite GetLocationImage()
    {
        return locationImage;
    }

    public List<HackTarget> GetHackTargets()
    {
        return hackTargets;
    }

    public List<MapObject> GetMapObjects()
    {
        return mapObjects;
    }

    public Enemy GetEnemy()
    {
        return enemy;
    }

    public List<int> GetEnemyBuffs()
    {
        return enemyBuffs;
    }

    public List<int> GetEnemyDebuffs()
    {
        return enemyDebuffs;
    }

    public int GetPOIScoutLevel()
    {
        return poiScoutLevel;
    }

    public int GetEnemyScoutLevel()
    {
        return enemyScoutLevel;
    }

    public bool GetIsPlayerPresent()
    {
        return playerPresent;
    }

    public MapSquareRow GetParentRow()
    {
        return parentRow;
    }

    public bool DoesSquareHaveTransportationNode()
    {
        return hasTransportationNode;
    }

    public bool GetIsTransportationNodeUnlocked()
    {
        return isTransportationNodeUnlocked;
    }

    public bool GetIsVentilationMapped()
    {
        return isVentilationMapped;
    }

    public void MapVentilation()
    {
        isVentilationMapped = true;
    }

    public int GetRowPosition()
    {
        return rowPosition;
    }

    public void SetTemporaryPositionMeasurement(int newMeasurement)
    {
        // used to measure and check position information, intended to be overwritten whenever
        temporaryDistanceMeasurement = newMeasurement;
    }

    public int GetDistanceMeasurement()
    {
        return temporaryDistanceMeasurement;
    }

    public void SetStartSquare()
    {
        isStart = true;
    }

    public void SetGoalSquare()
    {
        isGoal = true;
        Debug.Log("Goal Square is x: " + rowPosition.ToString() + ", y: " + parentRow.GetRowNumber());

        // Spawn a boss enemy if the mission is of assassination type
        MapData mapData = FindObjectOfType<MapData>();
        Job job = mapData.GetJob();
        if (job.GetJobType() == Job.JobType.Assassination)
        {
            SpawnEnemy(job.GetJobArea(), mapData.GetSecurityLevel(), job, true);
        }
    }

    public bool GetIsGoal()
    {
        return isGoal;
    }

    public void SetExtractionSquare()
    {
        isExtraction = true;
        extractionMarker.gameObject.SetActive(true);
        Debug.Log("Extraction Square is x: " + rowPosition.ToString() + ", y: " + parentRow.GetRowNumber());
    }

    public bool GetIsExtraction()
    {
        return isExtraction;
    }

    public bool GetIsExplored()
    {
        return explored;
    }

    public void SetTriggeredTrapType(MapObject.TrapTypes newTrapType)
    {
        triggeredTrapType = newTrapType;
    }

    public void SetTriggeredTrapType(MapObject.TrapTypes newTrapType, int newTrapAmount)
    {
        triggeredTrapType = newTrapType;
        triggeredTrapAmount = newTrapAmount;
    }

    public MapObject.TrapTypes GetTriggeredTrapType()
    {
        return triggeredTrapType;
    }

    public int GetTriggeredTrapAmount()
    {
        return triggeredTrapAmount;
    }

    public int GetPlayerDodgeBuff()
    {
        return playerDodgeBuff;
    }

    public int GetEnemyVulnerability()
    {
        return enemyVulnerability;
    }

    public int GetPlayerCritBuff()
    {
        return playerCritBuff;
    }

    public int GetPlayerHandSizeBuff()
    {
        return playerHandSizeBuff;
    }

    public int GetPlayerDefenseBuff()
    {
        return playerDefenseBuff;
    }

    public int GetEnemyHandSizeDebuff()
    {
        return enemyHandSizeDebuff;
    }

    public int GetEnemyFizzleChance()
    {
        return enemyFizzleChance;
    }

    public int GetDotDamageToEnemy()
    {
        return dotDamageToEnemy;
    }

    public int GetEnemyDamageDebuff()
    {
        return enemyDamageDebuff;
    }
    
    public List<string> GetEffectsListStrings()
    {
        List<string> effects = new List<string>();

        if (GetIsVentilationMapped())
            effects.Add("Moving here will not raise the security level");
        if (triggeredTrapType == MapObject.TrapTypes.ElectrifiedZone)
            effects.Add("Take " + triggeredTrapAmount + " damage when moving onto this square");
        if (triggeredTrapType == MapObject.TrapTypes.EMP)
            effects.Add("Your Cyber and Tech cards have " + triggeredTrapAmount + "% chance to have no effect on this square");
        if (triggeredTrapType == MapObject.TrapTypes.FaradayCage)
            effects.Add("Hacker cards are blocked on this square");
        if (triggeredTrapType == MapObject.TrapTypes.NerveGas)
            effects.Add("Your Bio cards have a " + triggeredTrapAmount + "% chance to have no effect on this square");
        if (triggeredTrapType == MapObject.TrapTypes.NeurotoxinCloud)
            effects.Add("In combat on this location, take " + triggeredTrapAmount + " damage each turn");
        if (triggeredTrapType == MapObject.TrapTypes.ParalysisAgent)
            effects.Add("Arm and Leg cards are blocked on this square");
        if (playerDodgeBuff > 0)
            effects.Add("Gain " + playerDodgeBuff.ToString() + "% Dodge on this square");
        if (enemyVulnerability > 0)
            effects.Add("Enemies take +" + enemyVulnerability.ToString() + " damage on this square");
        if (playerCritBuff > 0)
            effects.Add("Gain " + playerCritBuff.ToString() + "% crit chance on this square");
        if (playerHandSizeBuff > 0)
            effects.Add("Hand size increased by " + playerHandSizeBuff + " on this square");
        if (playerDefenseBuff > 0)
            effects.Add("Take -" + playerDefenseBuff + " damage on this location");
        if (enemyHandSizeDebuff > 0)
            effects.Add("Enemy hand size decreased by " + enemyHandSizeDebuff + " on this location");
        if (enemyFizzleChance > 0)
            effects.Add("Enemy cards have a " + enemyFizzleChance.ToString() + "% chance to have no effect on this square");
        if (dotDamageToEnemy > 0)
            effects.Add("In Combat: Enemies take " + dotDamageToEnemy.ToString() + "% of their health as damage each turn");
        if (enemyDamageDebuff > 0)
            effects.Add("Enemies deal -" + enemyDamageDebuff.ToString() + " damage on this square");

        return effects;
    }

    public bool ShouldEffectsButtonAppear()
    {
        if (GetIsVentilationMapped() ||
            GetTriggeredTrapType() == MapObject.TrapTypes.ElectrifiedZone ||
            GetTriggeredTrapType() == MapObject.TrapTypes.EMP ||
            GetTriggeredTrapType() == MapObject.TrapTypes.FaradayCage ||
            GetTriggeredTrapType() == MapObject.TrapTypes.NerveGas ||
            GetTriggeredTrapType() == MapObject.TrapTypes.NeurotoxinCloud ||
            GetTriggeredTrapType() == MapObject.TrapTypes.ParalysisAgent ||
            GetPlayerDodgeBuff() > 0 ||
            GetEnemyVulnerability() > 0 ||
            GetPlayerCritBuff() > 0 ||
            GetPlayerHandSizeBuff() > 0 ||
            GetPlayerDefenseBuff() > 0 ||
            GetEnemyHandSizeDebuff() > 0 ||
            GetEnemyFizzleChance() > 0 ||
            GetDotDamageToEnemy() > 0 ||
            GetEnemyDamageDebuff() > 0)
        {
            return true;
        }
        return false;
    }
}
