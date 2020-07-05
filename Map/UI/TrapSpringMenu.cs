using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapSpringMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;
    MapSquare currentSquare;
    MapConfig mapConfig;

    public void OpenMenu(MapSquare newSquare)
    {
        gameObject.SetActive(true);

        currentSquare = newSquare;
        mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.SetIsAMenuOpen(true);
        currentSquare = newSquare;

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
        FindObjectOfType<MapData>().StartBattleIfEnemyExists(currentSquare);
        mapConfig.SetIsAMenuOpen(false);
        gameObject.SetActive(false);
    }
}
