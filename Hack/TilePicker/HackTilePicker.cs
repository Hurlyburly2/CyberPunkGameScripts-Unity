using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HackTilePicker : MonoBehaviour
{
    // config
    [SerializeField] HackDummyTile hackDummyTile;
    [SerializeField] HackTilePickerTileHolder tileHolder;
    [SerializeField] TextMeshProUGUI selectTextField;
    [SerializeField] Button finishSelectionButton;

    Image cardBackImage;
    float cardWidth;
    float contentWidth;

    // state
    List<HackDummyTile> tileOptions = new List<HackDummyTile>();
    List<HackCard> actualCardObjects;

    string type;
        // pickAndDiscard
    int amountToPick;
    int amountPicked;


}
