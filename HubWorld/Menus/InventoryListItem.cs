using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName;

    public void SetText(string textString)
    {
        itemName.text = textString;
    }

    public void ClickButton()
    {

    }
}
