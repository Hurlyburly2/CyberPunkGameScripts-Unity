using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointOfInterestLine : MonoBehaviour
{
    [SerializeField] Image mainIcon;
    [SerializeField] Image tabbedIcon;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI tabbedText;
    [SerializeField] GameObject counterSquare;

    bool tabbed;
        // tabbed determines which set of gameobjects load images, and whether or not we also load a number
    string label;
    int number;

    public void SetupLine(bool newTabbed, string newLabel, int count)
    {
        tabbed = newTabbed;
        label = newLabel;
        number = count;

        SetupVisualElements();
    }

    public void SetupLine(bool newTabbed, string newLabel)
    {
        tabbed = newTabbed;
        label = newLabel;

        SetupVisualElements();
    }

    public void SetBlankLine()
    {
        tabbedIcon.enabled = false;
        mainIcon.enabled = false;
        tabbedText.text = "";
        mainText.text = "";
        counterSquare.SetActive(false);
    }

    private void SetupVisualElements()
    {
        MapSquareImageHolder imageHolder = FindObjectOfType<MapSquareImageHolder>();

        if (!tabbed)
        {
            tabbedIcon.enabled = false;
            mainIcon.enabled = true;

            mainIcon.sprite = imageHolder.GetMenuIconByName(label);
            tabbedText.text = "";
            mainText.text = label;

            counterSquare.SetActive(true);
            counterSquare.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();
        } else
        {
            tabbedIcon.enabled = true;
            mainIcon.enabled = false;

            tabbedIcon.sprite = imageHolder.GetMenuIconByName(label);
            tabbedText.text = label;
            mainText.text = "";

            counterSquare.SetActive(false);
        }
    }
}
