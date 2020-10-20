using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesMenuUpgradeBtn : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI priceField;

    int price;

    public void SetupButton(int newPrice)
    {
        price = newPrice;
        priceField.text = price.ToString();
    }
}
