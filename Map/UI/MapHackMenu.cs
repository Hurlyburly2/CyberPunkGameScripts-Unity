using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapHackMenu : MonoBehaviour
{
    [SerializeField] Image hackTypeImage;
    [SerializeField] Image hackTypeIcon;
    [SerializeField] HackOptionLine[] hackOptionLine;
    HackTarget hackTarget;

    public void InitializeMapHackMenu(HackTarget newHackTarget)
    {
        hackTarget = newHackTarget;
        MapSquareImageHolder imageHolder = FindObjectOfType<MapSquareImageHolder>();
        hackTypeImage.sprite = imageHolder.GetImageForHackOrPOI(hackTarget.getHackType());
        hackTypeIcon.sprite = imageHolder.GetButtonImageByName(hackTarget.getHackType(), true);
    }

    public void StartHack()
    {
        Debug.Log("Start Hack");
    }

    public void CloseMapMenu()
    {
        gameObject.SetActive(false);
    }
}
