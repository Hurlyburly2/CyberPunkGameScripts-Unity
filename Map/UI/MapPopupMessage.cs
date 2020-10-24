using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapPopupMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI infoField;

    public void OpenWindow(string description)
    {
        infoField.text = description;
    }
}
