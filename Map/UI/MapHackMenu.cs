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

        SetupHackOptionsLines();
        CheckHackCosts();
    }

    private void SetupHackOptionsLines()
    {
        int count = 0;
        foreach(HackOptionLine line in hackOptionLine)
        {
            string color = hackTarget.GetColor(count);
            string description = hackTarget.GetDescription(count);
            int cost = hackTarget.GetCost(count);
            line.InitializeHackOptionsLine(color, cost, description, hackTarget);
            count++;
        }
    }

    private void CheckHackCosts()
    {
        foreach (HackOptionLine line in hackOptionLine)
        {
            line.UpdateCanPlayerAffordAbility();
        }
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
