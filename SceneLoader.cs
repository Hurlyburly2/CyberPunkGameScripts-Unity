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

        StartCoroutine(WaitForSceneLoad(battleSceneName));
    }

    private IEnumerator WaitForSceneLoad(string sceneName)
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
}
