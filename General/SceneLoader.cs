using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // State
    int currentScene;
    BattleData currentBattle;
    HackBattleData currentHack;
    MapData currentMap;

    // config
    [SerializeField] BattleData battleData;
    [SerializeField] HackBattleData hackBattleData;
    [SerializeField] MapData mapData;
    [SerializeField] MusicPlayer musicPlayer;
    [SerializeField] PlayerData playerData;

    // Scene names
    [SerializeField] string battleSceneName = "Battle";
    [SerializeField] string hackSceneName = "Hack";
    [SerializeField] string mapSceneName = "Map";
    [SerializeField] string hubSceneName = "HubWorld";

    // Character/h@cker
    CharacterData currentRunner;
    HackerData currentHacker;

    private void Awake()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();

        // TODO: THIS MAY BE REDUNDANT AND WRONG, MAY BREAK STUFF WHEN EVERYTHING'S TIED TOGETHER
        int count = FindObjectsOfType<BattleData>().Length;
        if (count > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        int hackCount = FindObjectsOfType<HackBattleData>().Length;
        if (hackCount > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        int mapCount = FindObjectsOfType<MapData>().Length;
        if (mapCount > 1)
        {
            Destroy(gameObject);
        }

        int sceneLoaderCount = FindObjectsOfType<SceneLoader>().Length;
        if (sceneLoaderCount > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadMap(Job.JobArea mapType, int mapSize, Job job)
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
        currentRunner = playerData.GetCurrentRunner();
        currentHacker = playerData.GetCurrentHacker();
        currentMap = Instantiate(mapData);
        currentMap.SetMapData(currentRunner, currentHacker, mapType, 10, mapSize, job);
        ChangeMusicTrack(Job.JobArea.Slums);

        SceneManager.LoadScene(mapSceneName);

        StartCoroutine(WaitForMapLoad(mapSceneName));
    }

    public void LoadMapFromBattle()
    {
        MapGrid mapGrid = FindObjectOfType<BattleData>().GetMapGrid();
        SceneManager.LoadScene(mapSceneName);
        BattleData previousBattle = FindObjectOfType<BattleData>();

        currentMap = FindObjectOfType<MapData>();
        currentMap.TrackDefeatedEnemy();

        MapSquare currentSquare = previousBattle.GetMapSquare();
        currentSquare.DespawnEnemy();

        Destroy(previousBattle.gameObject);

        StartCoroutine(WaitForMapToLoadFromBattle(mapGrid, currentSquare));
    }

    private IEnumerator WaitForMapToLoadFromBattle(MapGrid mapGrid, MapSquare currentSquare)
    {
        while (SceneManager.GetActiveScene().name != mapSceneName)
        {
            yield return null;
        }
        mapGrid.gameObject.SetActive(true);
        currentMap.SetUpMapFromBattle();
        CenterCameraOnPlayer();
        DestroyExtraGrids();

        // if player landed on goal or extraction spaces, we open the appropriate window after the battle
        MapData mapData = FindObjectOfType<MapData>();
        if (mapData.ShouldGoalWindowOpenAfterCombat())
        {
            FindObjectOfType<MapConfig>().GetGoalWindow().OpenGoalWindow(currentSquare);
        } else if (mapData.GetShouldExtractionWindowOpenAfterCombat())
        {
            FindObjectOfType<MapConfig>().GetExtractionWindow().OpenExtractionWindow();
        }

        FindObjectOfType<MapData>().AdjustSecurityLevel(5);
    }

    private void DestroyExtraGrids()
    {
        MapGrid[] grids = FindObjectsOfType<MapGrid>();

        for (int i = 0; i < grids.Length; i++)
        {
            if (!grids[i].GetIsInitialized())
            {
                Destroy(grids[i].gameObject);
            }
        }
    }

    private IEnumerator WaitForMapLoad(string mapName)
    {
        while (SceneManager.GetActiveScene().name != mapSceneName)
        {
            yield return null;
        }
        currentMap.SetUpMap();
    }

    public void LoadBattleFromMap(MapSquare currentSquare)
    {
        MapGrid mapGrid = FindObjectOfType<MapGrid>();

        currentBattle = Instantiate(battleData);
        currentBattle.SetCharacterData(currentRunner, currentHacker);
        currentBattle.GetDataFromMapSquare(currentSquare);
        currentBattle.GetDataFromMapData(FindObjectOfType<MapData>());
        currentBattle.SetMapGrid(mapGrid);
        currentBattle.LoadModifiersFromMap(currentBattle.GetMapSquare().GetPackageOfModifiers());

        SceneManager.LoadScene(battleSceneName);

        StartCoroutine(WaitForBattleLoad(battleSceneName));
    }

    public void LoadBattle()
    {
        currentBattle = Instantiate(battleData);
        currentBattle.SetCharacterData(currentRunner, currentHacker);

        SceneManager.LoadScene(battleSceneName);

        StartCoroutine(WaitForBattleLoad(battleSceneName));
    }

    private IEnumerator WaitForBattleLoad(string sceneName)
    {
        while (SceneManager.GetActiveScene().name != battleSceneName)
        {
            yield return null;
        }
        currentBattle.SetUpBattle();
    }

    public void LoadBattleTestOne()
    {
        currentRunner = playerData.GetCurrentRunner();
        currentHacker = playerData.GetCurrentHacker();
        LoadBattle();
    }

    public void LoadHackTestOne()
    {
        currentRunner = playerData.GetCurrentRunner();
        currentHacker = playerData.GetCurrentHacker();
        LoadHack();
    }

    public void LoadMapTestOne()
    {
        currentRunner = playerData.GetCurrentRunner();
        currentHacker = playerData.GetCurrentHacker();
        Job job = ScriptableObject.CreateInstance<Job>();
        job.GenerateJob(0);
        LoadMap(Job.JobArea.Slums, 20, job);
    }

    public void LoadHack()
    {
        currentHack = Instantiate(hackBattleData);
        currentHack.SetCharacterData(currentRunner, currentHacker);

        SceneManager.LoadScene(hackSceneName);
        StartCoroutine(WaitForHackLoad());
    }

    private IEnumerator WaitForHackLoad()
    {
        while (SceneManager.GetActiveScene().name != hackSceneName)
        {
            yield return null;
        }
        currentHack.GetHacker().LogHackerData();
        currentHack.SetupHack(2, "default");
    }

    public void LoadHackFromMap(MapSquare mapSqare, HackTarget hackTarget)
    {
        MapGrid mapGrid = FindObjectOfType<MapGrid>();

        currentHack = Instantiate(hackBattleData);
        currentHack.SetCharacterData(currentRunner, currentHacker);
        currentHack.SetMapData(mapGrid, mapSqare, hackTarget);

        Destroy(FindObjectOfType<MapGrid>());
        SceneManager.LoadScene(hackSceneName);
        StartCoroutine(WaitForHackToLoadFromMap());
    }

    private IEnumerator WaitForHackToLoadFromMap()
    {
        while (SceneManager.GetActiveScene().name != hackSceneName)
        {
            yield return null;
        }
        currentHack.SetupHack(2, "default");
    }

    public void LoadMapFromHack(int redPoints, int bluePoints, int greenPoints, MapSquare currentSquare, HackTarget currentHackTarget)
    {
        MapGrid mapGrid = FindObjectOfType<HackBattleData>().GetMapGrid();
        SceneManager.LoadScene(mapSceneName);

        currentMap = FindObjectOfType<MapData>();
        currentMap.TrackCompletedHack();

        HackBattleData previousHack = FindObjectOfType<HackBattleData>();
        Destroy(previousHack.gameObject);

        StartCoroutine(WaitForMapToLoadFromHack(mapGrid, currentSquare, currentHackTarget, redPoints, bluePoints, greenPoints));
    }

    private IEnumerator WaitForMapToLoadFromHack(MapGrid mapGrid, MapSquare currentSquare, HackTarget currentHackTarget, int redPoints, int bluePoints, int greenPoints)
    {
        while (SceneManager.GetActiveScene().name != mapSceneName)
        {
            yield return null;
        }
        mapGrid.gameObject.SetActive(true);
        currentHackTarget.SetPoints(redPoints, bluePoints, greenPoints);
        currentMap.SetUpMapFromHack(currentSquare, currentHackTarget);
        CenterCameraOnPlayer();
        DestroyExtraGrids();
    }

    public void LoadHubFromCompletedMap(Job job, int creditsEarned, int goalModifier, int enemiesDefeated, int hacksCompleted)
    {
        // TODO: THIS
        SceneManager.LoadScene(hubSceneName);
        StartCoroutine(WaitForHubToLoadFromMap(job, creditsEarned, goalModifier, enemiesDefeated, hacksCompleted));
    }

    private IEnumerator WaitForHubToLoadFromMap(Job job, int creditsEarned, int goalModifier, int enemiesDefeated, int hacksCompleted)
    {
        while (SceneManager.GetActiveScene().name != hubSceneName)
        {
            yield return null;
        }

        ReturnToHubWorld(job, creditsEarned, goalModifier, enemiesDefeated, hacksCompleted);
    }

    private void ReturnToHubWorld(Job job, int creditsEarned, int goalModifier, int enemiesDefeated, int hacksCompleted)
    {
        musicPlayer.ChangeTrack(Job.JobArea.HomeBase);

        Destroy(FindObjectOfType<MapGrid>().gameObject);
        Destroy(currentMap);

        MissionCompleteMenu missionCompleteMenu = FindObjectOfType<HubMenuButton>().GetMissionCompleteMenu();
        missionCompleteMenu.gameObject.SetActive(true);
        missionCompleteMenu.SetupMissionCompleteMenu(job, creditsEarned, goalModifier, enemiesDefeated, hacksCompleted);
    }

    private void ChangeMusicTrack(Job.JobArea trackName)
    {
        musicPlayer.ChangeTrack(trackName);
    }

    public void SaveTest()
    {
        int randomNumber = Random.Range(0, 100);
        Debug.Log("random number befoer save: " + randomNumber);
        SaveData data = new SaveData(randomNumber);
        SaveSystem.SaveGame(data);
    }

    public void LoadTest()
    {
        SaveData data = SaveSystem.LoadSaveData();
        Debug.Log("random number loaded: " + data.GetRandomNumber());
        Debug.Log("random number list:");
        foreach(int number in data.GetRandomNumbers())
        {
            Debug.Log(number);
        }
    }

    [SerializeField] MapSquareImageHolder imageHolder;

    public void DisableObject()
    {
        Destroy(imageHolder.gameObject);
    }

    private void CenterCameraOnPlayer()
    {
        PlayerMarker player = FindObjectOfType<PlayerMarker>();
        Vector3 newCameraPosition = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
        Camera.main.transform.position = newCameraPosition;
    }
}
