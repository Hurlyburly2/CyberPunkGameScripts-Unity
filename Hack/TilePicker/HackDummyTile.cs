using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackDummyTile : MonoBehaviour
{
    Image selectedImage;
    [SerializeField] Sprite selectedImageSprite;
    [SerializeField] Sprite emptyImageSprite;

    // config
    int id;
    Sprite circuitLeft;
    Sprite circuitTop;
    Sprite circuitRight;
    Sprite circuitDown;

    Sprite spikeTopLeft;
    Sprite spikeTopRight;
    Sprite spikeBottomLeft;
    Sprite spikeBottomRight;

    Sprite cardImage;

    // state
    bool selected = false;
}
