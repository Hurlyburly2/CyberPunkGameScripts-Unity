using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour
{
    [SerializeField] Sprite[] images;
    // 0 = default
    // 1 = Runner
    // 2 = Hacker

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = images[0];
    }

    public void SetPortrait(string characterName)
    {
        if (characterName == "Runner")
        {
            GetComponent<Image>().sprite = images[1];
        } else if (characterName == "Hacker")
        {
            GetComponent<Image>().sprite = images[2];
        }
    }
}
