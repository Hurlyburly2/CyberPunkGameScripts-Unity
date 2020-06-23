using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapHackMenu : MonoBehaviour
{
    [SerializeField] Image hackTypeImage;
    [SerializeField] Image hackTypeIcon;
    [SerializeField] HackOptionLine[] hackOptionLine;
    [SerializeField] CurrentNodeMenu currentNodeMenu;
    [SerializeField] Button startHackButton;
    [SerializeField] GameObject pointsZone;
    [SerializeField] TextMeshProUGUI redPointsText;
    [SerializeField] TextMeshProUGUI bluePointsText;
    [SerializeField] TextMeshProUGUI purplePointsText;
    HackTarget hackTarget;
    MapSquare mapSquare;

    public void InitializeMapHackMenu(HackTarget newHackTarget)
    {
        mapSquare = currentNodeMenu.GetMapSquare();
        hackTarget = newHackTarget;
        MapSquareImageHolder imageHolder = FindObjectOfType<MapSquareImageHolder>();
        hackTypeImage.sprite = imageHolder.GetImageForHackOrPOI(hackTarget.getHackType());
        hackTypeIcon.sprite = imageHolder.GetButtonImageByName(hackTarget.getHackType(), true);

        SetupHackButtonOrPoints();
        SetupHackOptionsLines();
        CheckHackCosts();
    }

    private void SetupHackButtonOrPoints()
    {
        if (hackTarget.GetIsHackDone())
        {
            startHackButton.gameObject.SetActive(false);
            pointsZone.SetActive(true);
            SetPointsTextFields();
        } else
        {
            startHackButton.gameObject.SetActive(true);
            pointsZone.SetActive(false);
        }
    }

    private void SetPointsTextFields()
    {
        redPointsText.text = hackTarget.GetRedPoints().ToString();
        bluePointsText.text = hackTarget.GetBluePoints().ToString();
        purplePointsText.text = hackTarget.GetPurplePoints().ToString();
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
        // TODO
        // ADD THE HACK OBJECT HERE
        FindObjectOfType<SceneLoader>().LoadHackFromMap(mapSquare, hackTarget);
    }

    public void CloseMapMenu()
    {
        gameObject.SetActive(false);
    }
}
