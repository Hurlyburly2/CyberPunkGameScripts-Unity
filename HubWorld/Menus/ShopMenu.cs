using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentMoneyField;

    PlayerData playerData;

    public void SetupShopMenu()
    {
        Debug.Log("Setup Shop Menu");
        playerData = FindObjectOfType<PlayerData>();
        currentMoneyField.text = playerData.GetCreditsAmount().ToString();
    }

    public void CloseShopMenu()
    {
        gameObject.SetActive(false);
    }
}
