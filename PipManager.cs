using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipManager : MonoBehaviour
{
    [SerializeField] GameObject pip;
    List<GameObject> pipList = new List<GameObject>();

    ConfigData configData;
    float pipStartX;
    float pipStartY;
    float distanceBetweenPips;

    float pipValue;

    CharacterData character;

    public void Setup(ConfigData newConfigData, CharacterData newCharacter)
    {
        configData = newConfigData;
        pipStartX = configData.GetHealthPipStartX();
        pipStartY = configData.GetHealthPipStartY();
        distanceBetweenPips = configData.GetDistanceBetweenPips();

        character = newCharacter;
        float setupTimeInSeconds = FindObjectOfType<BattleData>().GetSetupTimeInSeconds();
        StartCoroutine(SetupPips(setupTimeInSeconds));
    }

    private IEnumerator SetupPips(float setupTimeInSeconds)
    {
        pipValue = (float)character.GetMaximumHealth() / configData.GetTotalHealthPips();
        int numberOfPips = Mathf.CeilToInt(character.GetMaximumHealth() / pipValue);
        float timePerPip = setupTimeInSeconds / numberOfPips;
        float distanceBetweenPip = configData.GetDistanceBetweenPips();

        yield return new WaitForSeconds(2);
        Debug.Log("test");
        Vector2 pipLocation = new Vector2(transform.position.x, transform.position.y);
        GameObject newPip = Instantiate(pip, pipLocation, Quaternion.identity);
        newPip.transform.parent = gameObject.transform;
    }
}
