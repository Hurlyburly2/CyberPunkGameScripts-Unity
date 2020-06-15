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

    public void LoadMap(string mapType)
    {
        currentMap = Instantiate(mapData);
        currentMap.SetCharacterData(currentRunner, currentHacker, mapType, 10);
        ChangeMusicTrack("slums");

        SceneManager.LoadScene(mapSceneName);

        StartCoroutine(WaitForMapLoad(mapSceneName));
    }

    private IEnumerator WaitForMapLoad(string mapName)
    {
        while (SceneManager.GetActiveScene().name != mapSceneName)
        {
            yield return null;
        }
        currentMap.SetUpMap();
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
        LoadMap("city");
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

    private void SetupRunnerAndHacker()
    {
        // TODO THIS METHOD IS FOR USE ONLY UNTIL SOME OUT OF BATTLE SETUP IS READY

    }

    private void ChangeMusicTrack(string trackName)
    {
        musicPlayer.ChangeTrack(trackName);
    }
}
