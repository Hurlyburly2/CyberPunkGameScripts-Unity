using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    public void SetupShopMenu()
    {
        Debug.Log("Setup Shop Menu");
    }

    public void CloseShopMenu()
    {
        gameObject.SetActive(false);
    }
}
