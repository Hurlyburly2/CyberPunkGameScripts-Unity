using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpListItem : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI powerUpNameField;
    [SerializeField] TextMeshProUGUI powerUpDescriptionField;
    [SerializeField] TextMeshProUGUI levelTextField;

    public void Setup(PowerUp powerUp)
    {
        powerUpNameField.text = powerUp.GetName();
        powerUpDescriptionField.text = powerUp.GetPowerUpDescription();
        levelTextField.text = powerUp.GetLevel().ToString();
    }
}
