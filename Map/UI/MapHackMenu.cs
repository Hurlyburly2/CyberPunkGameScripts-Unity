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
    [SerializeField] TextMeshProUGUI greenPointsText;
    [SerializeField] MapPopupMessage mapPopupMessage;
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
            Debug.Log("hack is done");
            startHackButton.gameObject.SetActive(false);
            pointsZone.SetActive(true);
            SetPointsTextFields();
        } else
        {
            Debug.Log("hack is not done");
            startHackButton.gameObject.SetActive(true);
            pointsZone.SetActive(false);
        }
    }

    public void SetPointsTextFields()
    {
        redPointsText.text = hackTarget.GetRedPoints().ToString();
        bluePointsText.text = hackTarget.GetBluePoints().ToString();
        greenPointsText.text = hackTarget.GetGreenPoints().ToString();
    }

    private void SetupHackOptionsLines()
    {
        int count = 0;
        foreach(HackOptionLine line in hackOptionLine)
        {
            string color = hackTarget.GetColor(count);
            string description = hackTarget.GetDescription(count);
            int cost = hackTarget.GetCost(count);
            string helperText = hackTarget.GetHackHelperText(count);
            line.InitializeHackOptionsLine(color, cost, description, hackTarget, helperText);
            count++;
        }
    }

    public void CheckHackCosts()
    {
        bool canPlayerAffordAnything = false;
        foreach (HackOptionLine line in hackOptionLine)
        {
            bool canAffordAbility = line.UpdateCanPlayerAffordAbility();
            if (canAffordAbility)
                canPlayerAffordAnything = true;

        }
        hackTarget.SetCanPlayerAffordAnything(canPlayerAffordAnything);
    }

    public void StartHack()
    {
        // TODO
        // ADD THE HACK OBJECT HERE
        FindObjectOfType<SceneLoader>().LoadHackFromMap(mapSquare, hackTarget);

        FindObjectOfType<CurrentNodeMenu>().gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void CloseMapMenu()
    {
        CheckHackCosts();
        Debug.Log("is hack still active: " + hackTarget.GetIsActive());
        List<HackTarget> hackTargets = FindObjectOfType<CurrentNodeMenu>().GetMapSquare().GetHackTargets();
        FindObjectOfType<CurrentNodeMenuHacks>().SetupButtons(hackTargets);
        FindObjectOfType<CurrentNodeMenu>().UpdateEffectsButton();
        gameObject.SetActive(false);
    }

    public MapSquare GetMapSquare()
    {
        return mapSquare;
    }

    public MapPopupMessage GetMapPopupMessage()
    {
        return mapPopupMessage;
    }
}
