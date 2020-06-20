using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentNodeMenuPointsOfInterest : MonoBehaviour
{
    [SerializeField] MenuPOIButton[] menuObjectButtons;

    public void SetupButtons(List<MapObject> mapObjects)
    {
        Debug.Log("Does this happen? Length of mapobjects: " + mapObjects.Count);

        int counter = 0;
        foreach (MenuPOIButton button in menuObjectButtons)
        {
            if (counter < mapObjects.Count)
            {
                button.SetupButton(mapObjects[counter]);
            } else
            {
                button.SetupButton();
            }
            counter++;
        }
    }
}
