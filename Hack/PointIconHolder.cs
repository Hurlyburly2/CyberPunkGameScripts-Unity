using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointIconHolder : MonoBehaviour
{
    [SerializeField] string whichColor;
    TextMeshProUGUI pointText;

    private void Start()
    {
        pointText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdatePointDisplay(int currentPoints)
    {
        pointText.text = currentPoints.ToString();
    }

    public string GetWhichColor()
    {
        return whichColor;
    }
}
