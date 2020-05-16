using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackUIUndoButton : MonoBehaviour
{
    [SerializeField] HackCardUIHolder hackCardUIHolder;

    private void OnMouseUp()
    {
        hackCardUIHolder.SendCardBackToDeck();
    }
}
