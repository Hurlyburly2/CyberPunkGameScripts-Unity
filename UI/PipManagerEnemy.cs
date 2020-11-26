using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipManagerEnemy : MonoBehaviour
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
    float weirdScale = 0.006f;

    bool isAnimatingDamage;

    public void Setup(ConfigData newConfigData, int newMaximumValue, int newCurrentValue)
    {
        configData = newConfigData;
        maximumValue = newMaximumValue;
        currentValue = newCurrentValue;
        isAnimatingDamage = false;

        maximumNumberOfPips = configData.GetMaximumNumberOfPips();
        distanceBetweenPips = configData.GetDistanceBetweenPips() * (weirdScale + 0.001f);

        pipValue = (float)maximumValue / maximumNumberOfPips;

        float setupTimeInSeconds = FindObjectOfType<BattleData>().GetSetupTimeInSeconds();
        StartCoroutine(ChangeNumberOfPips(setupTimeInSeconds));
    }

    public void ChangeValue(int newValue)
    {
        currentValue = newValue;
        isAnimatingDamage = true;
        StartCoroutine(ChangeNumberOfPips(.05f));
    }

    private IEnumerator ChangeNumberOfPips(float transitionTime)
    {
        int targetNumberOfPips = Mathf.CeilToInt(currentValue / pipValue);

        float timePerPip = transitionTime / targetNumberOfPips;
        if (targetNumberOfPips == 0)
        {
            timePerPip = .05f;
        }
        while (pipList.Count != targetNumberOfPips)
        {
            if (pipList.Count > targetNumberOfPips)
            {
                RemovePip();
            } else
            {
                AddPip();
            }
            yield return new WaitForSeconds(timePerPip);
        }

        isAnimatingDamage = false;

        if (pipList.Count == 0)
        {
            Debug.Log("Enemy Dead!");
            FindObjectOfType<SceneLoader>().LoadMapFromBattle();
        }
    }

    private void RemovePip()
    {
        GameObject previousLastPip = pipList[pipList.Count - 1];
        pipList.RemoveAt(pipList.Count - 1);
        Destroy(previousLastPip);
    }

    private void AddPip()
    {
        Vector2 pipLocation;
        if (pipList.Count > 0)
        {
            GameObject previousLastPip = pipList[pipList.Count - 1];

            pipLocation = new Vector2(transform.position.x, previousLastPip.transform.position.y + distanceBetweenPips);
        } else
        {
                pipLocation = new Vector2(transform.position.x, transform.position.y);
        }

        GameObject newLastPip = Instantiate(pip, pipLocation, Quaternion.Euler(new Vector3(0, 0, 90)));
        newLastPip.transform.SetParent(gameObject.transform);

        float currentPipScale = newLastPip.transform.localScale.y;
        float scaleMultiplier = 1 / currentPipScale;

        float nudge = distanceBetweenPips - distanceBetweenPips * scaleMultiplier;
        newLastPip.transform.position = new Vector2(newLastPip.transform.position.x, newLastPip.transform.position.y + nudge);

        newLastPip.transform.localScale = new Vector3(weirdScale, weirdScale, weirdScale);
        pipList.Add(newLastPip);
    }

    public bool GetIsAnimatingDamage()
    {
        return isAnimatingDamage;
    }
}
