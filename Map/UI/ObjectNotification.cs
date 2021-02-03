using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectNotification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptorText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image iconImage;
    MapConfig mapConfig;
    MapObject currentObject;

    MapSFX mapSFX;

    private void Awake()
    {
        mapSFX = FindObjectOfType<MapSFX>();
    }

    public void OpenMenu(MapObject newObject)
    {
        gameObject.SetActive(true);

        currentObject = newObject;
        mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.SetIsAMenuOpen(true);

        iconImage.sprite = FindObjectOfType<MapSquareImageHolder>().GetButtonImageByName(currentObject.GetObjectType(), true);
        titleText.text = currentObject.GetObjectTypeNameForDisplay();
        descriptorText.text = currentObject.DoObjectAction();
        nameText.text = currentObject.GetName();
    }

    public void CloseMenu()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        MapSquare currentSquare = FindObjectOfType<CurrentNodeMenu>().GetMapSquare();
        FindObjectOfType<CurrentNodeMenu>().InitializeMenu(currentSquare);

        gameObject.SetActive(false);
    }
}
