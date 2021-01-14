using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectsListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI effectText;
    [SerializeField] Image icon;

    public void SetupItem(string newText)
    {
        effectText.text = newText;
    }
}
