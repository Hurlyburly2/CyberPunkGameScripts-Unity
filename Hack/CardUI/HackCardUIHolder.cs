using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackCardUIHolder : MonoBehaviour
{
    [SerializeField] HackCard parentCard;
    [SerializeField] HackRotateButton leftArrow;
    [SerializeField] HackRotateButton rightArrow;
    [SerializeField] HackUIUndoButton undoButton;

    public void SendCardBackToDeck()
    {
        Debug.Log("Send card back to deck");
        HackDeck hackDeck = FindObjectOfType<HackDeck>();
        hackDeck.ReAttachTopCard();
        parentCard.GetCurrentSquareHolder().RemoveCardFromSquare();
    }

    public void SetRotationsOfArrows(int previousRotationAmount, int nextRotationAmount)
    {
        SetupArrow(leftArrow, previousRotationAmount);
        SetupArrow(rightArrow, nextRotationAmount);
    }

    public void SendCardRotationToSquare(int currentRotationAmount, string leftOrRight)
    {
        parentCard.GetCurrentSquareHolder().RotateButtonPressed(currentRotationAmount, leftOrRight);
    }

    private void SetupArrow(HackRotateButton currentArrow, int currentRotation)
    {
        if (currentRotation == -1)
        {
            currentArrow.gameObject.SetActive(false);
        } else
        {
            currentArrow.gameObject.SetActive(true);
            currentArrow.SetRotationAmount(currentRotation);
        }
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -100);
    }

    public void TurnOffCardUIHolder()
    {
        gameObject.SetActive(false);
    }

    public HackCard GetParentCard()
    {
        return parentCard;
    }
}
