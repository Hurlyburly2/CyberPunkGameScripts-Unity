using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHackButton : MonoBehaviour
{
    HackTarget hackTarget;

    public void SetupButton(HackTarget newHackTarget)
    {
        Debug.Log("hack button: " + newHackTarget.getHackType());
        hackTarget = newHackTarget;
        Image image = GetComponent<Image>();
        image.enabled = true;
        GetComponent<Button>().enabled = true;
        image.sprite = FindObjectOfType<MapSquareImageHolder>().GetButtonImageByName(hackTarget.getHackType(), hackTarget.GetIsActive());
    }

    public void SetupButton()
    {
        Image image = GetComponent<Image>();
        image.enabled = false;
        GetComponent<Button>().enabled = false;
        image.sprite = FindObjectOfType<MapSquareImageHolder>().GetButtonImageByName("none", false);
    }
}
