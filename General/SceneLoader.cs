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

    // Scene names
    [SerializeField] string battleSceneName = "Battle";
    [SerializeField] string hackSceneName = "Hack";
    [SerializeField] string mapSceneName = "Map";

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
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadMap(string mapType, int mapSize)
    {
        currentRunner = TestData.SetTestCharacterOne();
        currentHacker = TestData.SetTestHackerOne();
        currentMap = Instantiate(mapData);
        currentMap.SetMapData(currentRunner, currentHacker, mapType, 10, mapSize);
        ChangeMusicTrack("slums");

        SceneManager.LoadScene(mapSceneName);

        StartCoroutine(WaitForMapLoad(mapSceneName));
    }

    public void LoadMapFromBattle()
    {
        MapGrid mapGrid = FindObjectOfType<BattleData>().GetMapGrid();
        SceneManager.LoadScene(mapSceneName);
        BattleData previousBattle = FindObjectOfType<BattleData>();

        MapSquare currentSquare = previousBattle.GetMapSquare();
        currentSquare.DespawnEnemy();

        Destroy(previousBattle.gameObject);

        StartCoroutine(WaitForMapToLoadFromBattle(mapGrid));
    }

    private IEnumerator WaitForMapToLoadFromBattle(MapGrid mapGrid)
    {
        while (SceneManager.GetActiveScene().name != mapSceneName)
        {
            yield return null;
        }
        mapGrid.gameObject.SetActive(true);
        currentMap.SetUpMapFromBattle();
        DestroyExtraGrids();
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
        currentBattle.SetMapGrid(mapGrid);

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
        currentRunner = TestData.SetTestCharacterOne();
        currentHacker = TestData.SetTestHackerOne();
        LoadBattle();
    }

    public void LoadHackTestOne()
    {
        currentRunner = TestData.SetTestCharacterOne();
        currentHacker = TestData.SetTestHackerOne();
        LoadHack();
    }

    public void LoadMapTestOne()
    {
        currentRunner = TestData.SetTestCharacterOne();
        currentHacker = TestData.SetTestHackerOne();
        LoadMap("slums", 20);
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

    public void LoadMapFromHack(int redPoints, int bluePoints, int purplePoints, MapSquare currentSquare, HackTarget currentHackTarget)
    {
        MapGrid mapGrid = FindObjectOfType<HackBattleData>().GetMapGrid();
        SceneManager.LoadScene(mapSceneName);

        HackBattleData previousHack = FindObjectOfType<HackBattleData>();
        Destroy(previousHack.gameObject);

        StartCoroutine(WaitForMapToLoadFromHack(mapGrid, currentSquare, currentHackTarget, redPoints, bluePoints, purplePoints));
    }

    private IEnumerator WaitForMapToLoadFromHack(MapGrid mapGrid, MapSquare currentSquare, HackTarget currentHackTarget, int redPoints, int bluePoints, int purplePoints)
    {
        while (SceneManager.GetActiveScene().name != mapSceneName)
        {
            yield return null;
        }
        mapGrid.gameObject.SetActive(true);
        currentHackTarget.SetPoints(redPoints, bluePoints, purplePoints);
        currentMap.SetUpMapFromHack(currentSquare, currentHackTarget);
        DestroyExtraGrids();
    }

    private void SetupRunnerAndHacker()
    {
        // TODO THIS METHOD IS FOR USE ONLY UNTIL SOME OUT OF BATTLE SETUP IS READY

    }

    private void ChangeMusicTrack(string trackName)
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
}
