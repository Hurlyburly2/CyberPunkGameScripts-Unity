using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEffects : MonoBehaviour
{
    [SerializeField] GameObject radialEffect;

    public GameObject GetRadialEffect()
    {
        return radialEffect;
    }
}
