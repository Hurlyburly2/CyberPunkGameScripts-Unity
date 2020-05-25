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

    public void SetupDummyTile(HackCard card)
    {
        Image[] images = GetComponentsInChildren<Image>();
        foreach(Image image in images)
        {
            switch(image.name)
            {
                case "LeftCircuit":
                    circuitLeft = card.GetImageByGameObjectName("Left");
                    if (circuitLeft != null)
                        image.sprite = circuitLeft;
                    else
                        image.sprite = emptyImageSprite;
                    break;
                case "TopCircuit":
                    circuitTop = card.GetImageByGameObjectName("Up");
                    if (circuitTop != null)
                        image.sprite = circuitTop;
                    else
                        image.sprite = emptyImageSprite;
                    break;
                case "RightCircuit":
                    circuitRight = card.GetImageByGameObjectName("Right");
                    if (circuitRight != null)
                        image.sprite = circuitRight;
                    else
                        image.sprite = emptyImageSprite;
                    break;
                case "DownCircuit":
                    circuitDown = card.GetImageByGameObjectName("Down");
                    if (circuitDown != null)
                        image.sprite = circuitDown;
                    else
                        image.sprite = emptyImageSprite;
                    break;
                case "TopLeftSpike":
                    spikeTopLeft = card.GetImageByGameObjectName("TopLeft");
                    if (spikeTopLeft != null)
                        image.sprite = spikeTopLeft;
                    else
                        image.sprite = emptyImageSprite;
                    break;
                case "TopRightSpike":
                    spikeTopRight = card.GetImageByGameObjectName("TopRight");
                    if (spikeTopRight != null)
                        image.sprite = spikeTopRight;
                    else
                        image.sprite = emptyImageSprite;
                    break;
                case "BottomLeftSpike":
                    spikeBottomLeft = card.GetImageByGameObjectName("BottomLeft");
                    if (spikeBottomLeft != null)
                        image.sprite = spikeBottomLeft;
                    else
                        image.sprite = emptyImageSprite;
                    break;
                case "BottomRightSpike":
                    spikeBottomRight = card.GetImageByGameObjectName("BottomRight");
                    if (spikeBottomRight != null)
                        image.sprite = spikeBottomRight;
                    else
                        image.sprite = emptyImageSprite;
                    break;
                case "FrontImage":
                    cardImage = card.GetImageByGameObjectName("Image");
                    if (cardImage != null)
                        image.sprite = cardImage;
                    else
                        image.sprite = emptyImageSprite;
                    break;
                case "Selected":
                    selectedImage = image;
                    break;
            }
        }
    }

    public void ToggleSelect()
    {
        HackTilePicker tilePicker = FindObjectOfType<HackTilePicker>();
        if (selected)
        {
            selected = false;
            selectedImage.sprite = emptyImageSprite;
            tilePicker.UnSelectOne();
        } else
        {
            if (tilePicker.CanSelectMore())
            {
                selected = true;
                selectedImage.sprite = selectedImageSprite;
                tilePicker.SelectOne();
            }
        }
    }

    //CardPicker cardPicker = FindObjectOfType<CardPicker>();
    //    if (selected)
    //    {
    //        selected = false;
    //        selectedImage.sprite = emptyImageSprite;
    //        cardPicker.UnSelectOne();
    //    } else
    //    {
    //        if (cardPicker.CanSelectMore())
    //        {
    //            selected = true;
    //            selectedImage.sprite = selectedImageSprite;
    //            cardPicker.SelectOne();
    //        }
    //    }

    public void DestroyDummyTile()
    {
        Destroy(gameObject);
    }
}
