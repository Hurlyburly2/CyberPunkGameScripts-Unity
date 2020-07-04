using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBackground : MonoBehaviour
{
    [SerializeField] SpriteRenderer backgroundImageHolder;

    public void SetImage(Sprite imageFromMap)
    {
        if (imageFromMap != null)
        {
            backgroundImageHolder.sprite = imageFromMap;
        }
    }
}
