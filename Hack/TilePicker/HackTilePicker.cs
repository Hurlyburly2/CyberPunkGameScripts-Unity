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

    public void Initialize(List<HackCard> cards, int newAmountToPick, string newTypeOfPicker)
    {
        actualCardObjects = cards;
        contentWidth = 0;
        gameObject.SetActive(true);
        ClearDummyTiles();
        SetTileSize();

        type = newTypeOfPicker;
        amountToPick = newAmountToPick;
        amountPicked = 0;
        UpdateText();

        int counter = 0;
        foreach (HackCard card in cards)
        {
            CreateCardOption(card, counter);
            counter++;
        }

        HorizontalLayoutGroup horizontalLayoutGroup = tileHolder.GetComponent<HorizontalLayoutGroup>();
        horizontalLayoutGroup.spacing = cardWidth * 1.5f;
        horizontalLayoutGroup.padding.left = Mathf.CeilToInt(cardWidth);
        horizontalLayoutGroup.padding.right = Mathf.CeilToInt(cardWidth);
    }

    private void CreateCardOption(HackCard card, int counter)
    {
        float imageOffset = cardWidth;
        contentWidth += cardWidth;
        if (counter > 0)
        {
            imageOffset += cardWidth / 4;
            contentWidth += cardWidth / 4;
        }
        imageOffset *= counter;
        HackDummyTile newDummyCard = Instantiate(hackDummyTile, new Vector2(transform.position.x + imageOffset, transform.position.y), Quaternion.identity);
        newDummyCard.transform.SetParent(tileHolder.transform);
        newDummyCard.SetupDummyTile(card);

        tileOptions.Add(newDummyCard);
    }

    private void UpdateText()
    {
        string textFieldText = "";
        switch (type)
        {
            case "pickAndDiscard":
                textFieldText = "Select " + (amountToPick - amountPicked) + " cards to top of your deck, discard the others";
                break;
        }
        selectTextField.text = textFieldText;
    }

    private void SetTileSize()
    {
        Image[] images = hackDummyTile.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            if (image.name == "BackImage")
            {
                RectTransform rt = (RectTransform)image.transform;
                cardWidth = rt.rect.width;
            }
        }
    }

    private void ClearDummyTiles()
    {
        HackDummyTile[] dummyTiles = FindObjectsOfType<HackDummyTile>();
        for (int i = 0; i < dummyTiles.Length; i++)
        {
            dummyTiles[i].DestroyDummyTile();
        }
        tileOptions = new List<HackDummyTile>();
    }

    public void SelectOne()
    {
        amountPicked++;
        UpdateText();
        SelectedMax();
    }

    public void UnSelectOne()
    {
        amountPicked--;
        UpdateText();
        SelectedMax();
    }

    private void SelectedMax()
    {
        if (amountPicked >= amountToPick)
        {
            finishSelectionButton.interactable = true;
        }
        else
        {
            finishSelectionButton.interactable = false;
        }
    }

    public bool CanSelectMore()
    {
        if (amountToPick - amountPicked > 0)
        {
            return true;
        }
        return false;
    }
}
