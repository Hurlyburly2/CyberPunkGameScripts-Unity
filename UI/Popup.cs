using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void SetText(string textToSet)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = textToSet;
    }
}
