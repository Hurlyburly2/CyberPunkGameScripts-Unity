using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquare : MonoBehaviour
{
    //config
    [SerializeField] int rowPosition; // 0-19 position in row
    [SerializeField] MapSquareRow parentRow;
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
    string state;
    bool playerPresent;
    bool shroud;
    int poiScoutLevel;
    int enemyScoutLevel;
        // 1 = normal (default) no knowledge, 2 = know how many items, 3 = know what everything is

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
                CurrentNodeMenu menu = mapConfig.GetCurrentNodeMenu();
                menu.InitializeMenu(this);
            } else
            {
                mapConfig.SetIsAMenuOpen(true);
                NeighboringNodeMenu menu = mapConfig.GetNeighboringNodeMenu();
                menu.InitializeMenu(this);
            }
        }
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
        playerPresent = true;
        enemyScoutLevel = 3;
        poiScoutLevel = 3;
        RemoveShroud();
        RemoveAdjacentShrouds();
    }

    public void InitializeSquare(Sprite newImage, Sprite newLocationImage)
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

        SetupHacksAndObjects();
    }

    private void SetupHacksAndObjects()
    {
        SetupHackObjectSpawnLists();

        // random 1-3
        int objectsToSpawn = Random.Range(1, 4);
        while (objectsToSpawn > 0)
        {
            int random = Random.Range(0, 100);
            if (random <= 60)
            {
                HackTarget newHackTarget = ScriptableObject.CreateInstance<HackTarget>();
                string newHackType = availableHackTypes[Random.Range(0, availableHackTypes.Count)];
                newHackTarget.SetupHackTarget(newHackType);
                availableHackTypes.Remove(newHackType);

                hackTargets.Add(newHackTarget);
                objectsToSpawn--;
            } else if (random > 60)
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

    public void SpawnEnemy(string mapType)
    {
        enemy = FindObjectOfType<EnemyCollection>().GetAnEnemyByArea(mapType);
    }

    private void SetupHackObjectSpawnLists()
    {
        hackTargets = new List<HackTarget>();
        mapObjects = new List<MapObject>();

        availableHackTypes = new List<string>();
        string[] hackTypes = { "Security Camera", "Combat Server", "Database", "Defense System", "Transportation", "Medical Server" };
        availableHackTypes.AddRange(hackTypes);

        availableObjectTypes = new List<string>();
        string[] objectTypes = { "Trap", "Reward", "PowerUp", "Shop", "Upgrade", "First Aid Station" };
        availableObjectTypes.AddRange(objectTypes);
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
        Debug.Log("found adjacent squares: " + adjacentSquares.Count);
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
        defaultColor = GetComponent<SpriteRenderer>().color;
        AddShroud();
        mapConfig = FindObjectOfType<MapConfig>();
    }

    void Update()
    {
        float step = 2.5f * Time.deltaTime;

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

    private void SetState(string newState)
    {
        state = newState;
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
}
