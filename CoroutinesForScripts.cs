using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinesForScripts : MonoBehaviour
{
    public static CoroutinesForScripts instance;

    // Start is called before the first frame update
    void Start()
    {
        CoroutinesForScripts.instance = this;
    }

    public CoroutinesForScripts GetInstance()
    {
        return instance;
    }
}
