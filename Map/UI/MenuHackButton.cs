using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuHackButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hackName;
    HackTarget hackTarget;

    public void SetupButton(HackTarget newHackTarget)
    {
        hackTarget = newHackTarget;
        Image image = GetComponent<Image>();
        image.enabled = true;
        GetComponent<Button>().enabled = true;
        image.sprite = FindObjectOfType<MapSquareImageHolder>().GetButtonImageByName(hackTarget.getHackType(), hackTarget.GetIsActive());
        hackName.enabled = true;
        hackName.text = hackTarget.getHackType();
    }

    public void SetupButton()
    {
        hackName.enabled = false;
        Image image = GetComponent<Image>();
        image.enabled = false;
        GetComponent<Button>().enabled = false;
        image.sprite = FindObjectOfType<MapSquareImageHolder>().GetButtonImageByName("none", false);
    }

    public void OpenHackMenu()
    {
        MapHackMenu hackMenu = FindObjectOfType<MapConfig>().GetMapHackMenu();
        hackMenu.gameObject.SetActive(true);
        hackMenu.InitializeMapHackMenu(hackTarget);
    }
}
