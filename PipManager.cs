using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipManager : MonoBehaviour
{
    [SerializeField] GameObject pip;
    List<GameObject> pipList = new List<GameObject>();

    ConfigData configData;
    float distanceBetweenPips;

    int maximumNumberOfPips;
    int currentNumberOfPips;

    int maximumValue;
    int currentValue;

    float pipValue;

    public void Setup(ConfigData newConfigData, int newMaximumValue, int newCurrentValue)
    {
        configData = newConfigData;
        maximumValue = newMaximumValue;
        currentValue = newCurrentValue;
        maximumNumberOfPips = configData.GetMaximumNumberOfPips();
        distanceBetweenPips = configData.GetDistanceBetweenPips();

        float setupTimeInSeconds = FindObjectOfType<BattleData>().GetSetupTimeInSeconds();
        StartCoroutine(SetupPips(setupTimeInSeconds));
    }

    private IEnumerator SetupPips(float setupTimeInSeconds)
    {
        pipValue = (float)maximumValue / maximumNumberOfPips;
        int currentNumberOfPips = Mathf.CeilToInt(currentValue / pipValue);
        float timePerPip = setupTimeInSeconds / currentNumberOfPips;

        yield return new WaitForSeconds(2);
        Vector2 pipLocation = new Vector2(transform.position.x, transform.position.y);
        GameObject newPip = Instantiate(pip, pipLocation, Quaternion.identity);
        newPip.transform.parent = gameObject.transform;
    }
}
