using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPicker : MonoBehaviour
{
    [SerializeField] DummyCard dummycard;
    [SerializeField] CardPickerCardHolder cardHolder;

    public void Initialize()
    {
        gameObject.SetActive(true);
    }
}
