using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapSpringMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;
    MapSquare currentSquare;
    MapConfig mapConfig;

    // state
    bool goalAfterTrap = false;
    bool extractAfterTrap = false;

    public void OpenMenu(MapSquare newSquare, bool isGoalReady, bool isExtractionReady)
    {
        gameObject.SetActive(true);

        currentSquare = newSquare;
        mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.SetIsAMenuOpen(true);
        currentSquare = newSquare;
        goalAfterTrap = isGoalReady;

        textField.text = GetTrapTextFromSquare();
    }

    private string GetTrapTextFromSquare()
    {
        List<MapObject> mapObjects = currentSquare.GetMapObjects();
        foreach (MapObject mapObject in mapObjects)
        {
            if (mapObject.GetObjectType() == "Trap")
            {
                return mapObject.GetTrapTextFromSquare();
            }
        }

        return "";
    }

    public void OkButtonClicked()
    {
        MapData mapData = FindObjectOfType<MapData>();
        if (currentSquare.GetIsGoal() && !mapData.GetHasGoalBeenReached())
        {
            mapData.SetWasPlayerOnGoalBeforeCombat(true);
        } else if (currentSquare.GetIsExtraction())
        {
            mapData.SetWasPlayerOnExtractionBeforeCombat(true);
        }
        mapData.StartBattleIfEnemyExists(currentSquare);

        if (goalAfterTrap)
        {
            mapConfig.GetGoalWindow().OpenGoalWindow(currentSquare);
        }
        else if (extractAfterTrap) {
            mapConfig.GetExtractionWindow().OpenExtractionWindow();
        }
        else
        {
            FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        }
        gameObject.SetActive(false);
    }
}
