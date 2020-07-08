using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalWindow : MonoBehaviour
{
    public void OpenGoalWindow()
    {
        gameObject.SetActive(true);
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(true);
    }

    public void CloseGoalWindow()
    {
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        FindObjectOfType<MapData>().SetHasGoalBeenReached(true);
        gameObject.SetActive(false);
    }
}
