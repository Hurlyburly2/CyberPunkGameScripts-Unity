using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackUIAcceptButton : MonoBehaviour
{
    [SerializeField] HackCardUIHolder hackCardUIHolder;

    private void OnMouseUp()
    {
        hackCardUIHolder.TurnOffCardUIHolder();
    }
}
