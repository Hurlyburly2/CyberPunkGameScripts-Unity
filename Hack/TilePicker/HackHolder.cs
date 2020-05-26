using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackHolder : MonoBehaviour
{
    [SerializeField] HackTilePicker hackTilePicker;

    public HackTilePicker GetHackTilePicker()
    {
        return hackTilePicker;
    }
}
