using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectNotification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptorText;
    [SerializeField] Image iconImage;
    MapConfig mapConfig;
    MapObject currentObject;

    public void OpenMenu(MapObject newObject)
    {
        gameObject.SetActive(true);

        currentObject = newObject;
        mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.SetIsAMenuOpen(true);

        iconImage.sprite = FindObjectOfType<MapSquareImageHolder>().GetButtonImageByName(currentObject.GetObjectType(), true);
        titleText.text = currentObject.GetObjectTypeNameForDisplay();
        descriptorText.text = currentObject.DoObjectAction();
    }

    public void CloseMenu()
    {
        mapConfig.SetIsAMenuOpen(false);

        MapSquare currentSquare = FindObjectOfType<CurrentNodeMenu>().GetMapSquare();
        FindObjectOfType<CurrentNodeMenu>().InitializeMenu(currentSquare);

        gameObject.SetActive(false);
    }
}
