using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    void Start()
    {
        Transitioner.Instance.FinishTransition();
    }
}
