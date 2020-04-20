using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // State
    int currentScene;
    BattleData currentBattle;

    // config
    [SerializeField] BattleData battleData;

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
        currentBattle.GetCharacter().LogCharacterData();

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
        SceneManager.LoadScene(hackSceneName);
        StartCoroutine(WaitForHackLoad());
    }

    private IEnumerator WaitForHackLoad()
    {
        while (SceneManager.GetActiveScene().name != hackSceneName)
        {
            yield return null;
        }
        Debug.Log("Hack scene loaded");
    }
}
