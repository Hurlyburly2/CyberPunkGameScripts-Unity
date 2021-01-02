using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipManager : MonoBehaviour
{
    [SerializeField] GameObject pip;
    List<GameObject> pipList = new List<GameObject>();

    ConfigData configData;
    MapConfig mapConfig;
    float distanceBetweenPips;

    int maximumNumberOfPips;
    int currentNumberOfPips;

    int maximumValue;
    int currentValue;

    float pipValue;
    bool isAnimatingDamage;
    int targetNumberOfPips;

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

    public void Setup(MapConfig mapConfigData, int newMaximumValue, int newCurrentValue)
    {
        mapConfig = mapConfigData;
        maximumValue = newMaximumValue;
        currentValue = newCurrentValue;

        maximumNumberOfPips = mapConfig.GetMaximumNumberOfPips();
        if (gameObject.name == mapConfig.GetSecurityPipManagerName())
        {
            maximumNumberOfPips += 7;
        }
        distanceBetweenPips = mapConfig.GetDistanceBetweenPips();

        pipValue = (float)maximumValue / maximumNumberOfPips;

        float setupTimeInSeconds = FindObjectOfType<MapData>().GetSetupTimeInSeconds();
        StartCoroutine(ChangeNumberOfPips(setupTimeInSeconds));
    }

    private IEnumerator ChangeNumberOfPips(float transitionTime)
    {
        targetNumberOfPips = Mathf.CeilToInt(currentValue / pipValue);

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
    }

    public void ChangeValue(int newValue)
    {
        currentValue = newValue;
        if (isAnimatingDamage)
        {
            // If it was already running, we allow the target to change on the fly
            targetNumberOfPips = Mathf.CeilToInt(currentValue / pipValue);
            // TODO: We may also need to change the time per pip on the fly as well!
            return;
        }
        isAnimatingDamage = true;
        StartCoroutine(ChangeNumberOfPips(.05f));
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

            pipLocation = new Vector2(previousLastPip.transform.position.x + distanceBetweenPips, transform.position.y);
        } else
        {
            if (this.name == "EnemyHealthPipManager")
            {
                pipLocation = new Vector2(0, 0);
            } else
            {
                pipLocation = new Vector2(transform.position.x, transform.position.y);
            }
            
        }

        GameObject newLastPip = Instantiate(pip, pipLocation, Quaternion.identity);
        newLastPip.transform.SetParent(gameObject.transform);

        float currentPipScale = newLastPip.transform.localScale.x;
        float scaleMultiplier = 1 / currentPipScale;

        float nudge = distanceBetweenPips - distanceBetweenPips * scaleMultiplier;
        newLastPip.transform.position = new Vector2(newLastPip.transform.position.x - nudge, newLastPip.transform.position.y);

        newLastPip.transform.localScale = new Vector3(1, 1, 1);
        pipList.Add(newLastPip);
    }
}
