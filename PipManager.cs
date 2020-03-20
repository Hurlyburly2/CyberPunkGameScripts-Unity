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

        pipValue = (float)maximumValue / maximumNumberOfPips;

        float setupTimeInSeconds = FindObjectOfType<BattleData>().GetSetupTimeInSeconds();
        StartCoroutine(ChangeNumberOfPips(setupTimeInSeconds));
    }

    private IEnumerator ChangeNumberOfPips(float transitionTime)
    {
        int targetNumberOfPips = Mathf.CeilToInt(currentValue / pipValue);

        float timePerPip = transitionTime / targetNumberOfPips;
        while (pipList.Count != targetNumberOfPips)
        {
            Debug.Log("PipList.Count: " + pipList.Count + ", Current Number of Pips: " + targetNumberOfPips);
            if (pipList.Count > targetNumberOfPips)
            {
                RemovePip();
            } else
            {
                AddPip();
            }
            yield return new WaitForSeconds(timePerPip);
        }
    }

    private void RemovePip()
    {
        GameObject previousLastPip = pipList[pipList.Count - 1];
        Destroy(previousLastPip);
    }

    private void AddPip()
    {
        Vector2 pipLocation;
        if (pipList.Count > 0)
        {
            Debug.Log("Greater Than Zero");
            GameObject previousLastPip = pipList[pipList.Count - 1];

            pipLocation = new Vector2(previousLastPip.transform.position.x + distanceBetweenPips, transform.position.y);
        } else
        {
            Debug.Log("Less than Zero");
            pipLocation = new Vector2(transform.position.x, transform.position.y);
        }
        GameObject newLastPip = Instantiate(pip, pipLocation, Quaternion.identity);
        newLastPip.transform.parent = gameObject.transform;
        pipList.Add(newLastPip);
    }
}
