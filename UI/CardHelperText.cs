using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardHelperText : MonoBehaviour
{
    public void Activate(string keyword)
    {
        gameObject.SetActive(true);
        string helperText = GetHelperText(keyword);
        SetText(keyword, helperText);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void SetText(string keyword, string helperText)
    {
        TextMeshProUGUI[] textFields = GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI textField in textFields)
        {
            if (textField.name == "Header")
            {
                textField.text = keyword;
            } else if (textField.name == "Body")
            {
                textField.text = helperText;
            }
        }
    }

    private string GetHelperText(string keyword)
    {
        switch (keyword)
        {
            case "Damage Resist":
                return "For each stack of Damage Resist, take -1 damage.";
            case "Dodge":
                return "Gain 35% chance to dodge. Each additional stack has diminished effect.";
            case "Momentum":
                return "For each stack of Momentum, deal +1 damage.";
            case "Vulnerable":
                return "For each stack of Vulnerable, take +1 damage.";
            case "Weakness":
                return "Weaknesses cannot remain in your hand at the end of your turn and must be played.";
            case "Acceleration":
                return "Energy cost -1 for each extra card you draw";
            case "Stance":
                return "You can only play one stance card per turn";
            default:
                return "";
        }
    }
}
