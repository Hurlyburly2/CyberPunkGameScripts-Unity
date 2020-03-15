using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    int currentScene;

    [SerializeField] BattleData battleData;

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
        BattleData currentBattle = Instantiate(battleData);
        currentBattle.SetCharacterData(character);
        currentBattle.GetCharacter().LogCharacterData();
        SceneManager.LoadScene("Battle");
    }

    public void LoadBattleTestOne()
    {
        CharacterData currentCharacter = TestData.SetTestCharacterOne();
        LoadBattle(currentCharacter);
    }
}
