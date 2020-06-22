using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        MapCanvas[] mapCanvas = FindObjectsOfType<MapCanvas>();

        if (mapCanvas.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
