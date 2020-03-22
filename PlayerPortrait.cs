using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour
{
    [SerializeField] Sprite[] images;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = images[0];
    }

    public void SetPortrait(int characterId)
    {
        GetComponent<Image>().sprite = images[characterId];
    }
}
