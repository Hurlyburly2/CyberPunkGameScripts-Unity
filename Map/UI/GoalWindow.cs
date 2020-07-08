using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionText;

    public void OpenGoalWindow()
    {
        gameObject.SetActive(true);
        descriptionText.text = GenerateDescriptionText(FindObjectOfType<MapGrid>().GetMapType());
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(true);
    }

    private string GenerateDescriptionText(string mapType)
    {
        switch (mapType)
        {
            case "slums":
                return "You have obtained the item. Flavor & Story text will go here. Enemies are on high alert. Hurry to the extraction point!";
        }
        return "";
    }

    public void CloseGoalWindow()
    {
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        FindObjectOfType<MapData>().SetHasGoalBeenReached(true);
        gameObject.SetActive(false);
    }
}
