using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPOIButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;
    MapObject mapObject;

    public void SetupButton(MapObject newMapObject)
    {
        mapObject = newMapObject;
        Image image = GetComponent<Image>();
        image.enabled = true;
        GetComponent<Button>().enabled = true;
        image.sprite = FindObjectOfType<MapSquareImageHolder>().GetButtonImageByName(mapObject.GetObjectType(), mapObject.GetIsActive());
        textField.enabled = true;
        textField.text = mapObject.GetObjectType();
    }

    public void SetupButton()
    {
        Image image = GetComponent<Image>();
        image.enabled = false;
        GetComponent<Button>().enabled = false;
        image.sprite = FindObjectOfType<MapSquareImageHolder>().GetButtonImageByName("none", false);
        textField.enabled = false;
    }
}
