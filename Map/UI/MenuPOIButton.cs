using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPOIButton : MonoBehaviour
{
    public void SetupButton(MapObject mapObject)
    {
        Debug.Log("mapobject type: " + mapObject.GetObjectType());
        Debug.Log("mapobject isActive: " + mapObject.GetObjectType());
        GetComponent<Image>().sprite = FindObjectOfType<MapSquareImageHolder>().GetButtonImageByName(mapObject.GetObjectType(), mapObject.GetIsActive());
    }

    public void SetupButton()
    {
        GetComponent<Image>().sprite = FindObjectOfType<MapSquareImageHolder>().GetButtonImageByName("none", false);
    }
}
