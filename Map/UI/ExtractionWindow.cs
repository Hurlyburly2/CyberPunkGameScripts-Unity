using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionWindow : MonoBehaviour
{
    public void OpenExtractionWindow()
    {
        gameObject.SetActive(true);
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(true);
    }
}
