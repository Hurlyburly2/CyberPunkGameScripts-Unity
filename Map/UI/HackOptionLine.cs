using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HackOptionLine : MonoBehaviour
{
    [SerializeField] Image pointColorIcon;
    [SerializeField] TextMeshProUGUI pointCostField;
    [SerializeField] TextMeshProUGUI descriptionField;
    [SerializeField] Image buttonImage;
    [SerializeField] Button button;
    [SerializeField] Sprite buttonActive;
    [SerializeField] Sprite buttonInactive;
    [SerializeField] MapHackMenu mapHackMenu;
    HackTarget hackTarget;
    int cost;
    string color;
    string description;

    public void InitializeHackOptionsLine(string newColor, int newCost, string newDescription, HackTarget newHackTarget)
    {
        hackTarget = newHackTarget;
        color = newColor;
        cost = newCost;
        description = newDescription;

        pointColorIcon.sprite = FindObjectOfType<MapSquareImageHolder>().GetPointSquareByColor(color);
        pointCostField.text = cost.ToString();
        descriptionField.text = description;
        buttonImage.sprite = buttonActive;
    }

    public void UpdateCanPlayerAffordAbility()
    {
        bool playerCanAffordAbility = hackTarget.CanPlayerAffordAbility(color, cost);
        if (playerCanAffordAbility)
        {
            button.enabled = true;
            buttonImage.sprite = buttonActive;
        } else
        {
            buttonImage.sprite = buttonInactive;
            button.enabled = false;
        }
        Debug.Log("player can afford ability: " + playerCanAffordAbility);
    }
}
