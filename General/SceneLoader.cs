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

    // config
    [SerializeField] BattleData battleData;
    [SerializeField] HackBattleData hackBattleData;

    // Scene names
    [SerializeField] string battleSceneName = "Battle";
    [SerializeField] string hackSceneName = "Hack";

    private void Awake()
    {
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
    }

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadBattle(CharacterData character)
    {
        currentBattle = Instantiate(battleData);
        currentBattle.SetCharacterData(character);

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
        CharacterData currentCharacter = TestData.SetTestCharacterOne();
        LoadBattle(currentCharacter);
    }

    public void LoadBattleTestTwo()
    {
        CharacterData currentCharacter = TestData.SetTestCharacterTwo();
        LoadBattle(currentCharacter);
    }

    public void LoadHackTestOne()
    {
        HackerData currentHacker = TestData.SetTestHackerOne();
        LoadHack(currentHacker);
    }

    public void LoadHack(HackerData hacker)
    {
        currentHack = Instantiate(hackBattleData);
        currentHack.SetHackerData(hacker);

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
        currentHack.SetupHack();
    }
}
