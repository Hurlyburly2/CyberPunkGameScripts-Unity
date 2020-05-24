using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllHackAbilityIcons : MonoBehaviour
{
    [SerializeField] Sprite[] allHackAbilityIcons;

    public Sprite GetAbilityIconById(int abilityId)
    {
        return allHackAbilityIcons[abilityId];
    }
}
