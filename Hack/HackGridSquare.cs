using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackGridSquare : MonoBehaviour
{
    [SerializeField] int squareNumber;
    [SerializeField] Sprite[] imageOptions;
    [SerializeField] GameObject hackholder;

    GridRow parentRow;
    GridRowHolder gridRowHolder;
    bool active = false;

    HackGridSquare leftSquare = null;
    HackGridSquare aboveSquare = null;
    HackGridSquare rightSquare = null;
    HackGridSquare belowSquare = null;

    HackCard attachedHackCard;

    private void Start()
    {
        GameObject parentRowObject = transform.parent.gameObject;
        parentRow = parentRowObject.GetComponent<GridRow>();
        gridRowHolder = FindObjectOfType<GridRowHolder>();
    }

    private void FindAndStoreAdjacentSquares()
    {
        if (squareNumber != 0)
            leftSquare = parentRow.GetSquareByNumber(squareNumber - 1);
        if (parentRow.GetRowNumber() != 0)
            aboveSquare = gridRowHolder.GetRowByNumber(parentRow.GetRowNumber() - 1).GetSquareByNumber(squareNumber);
        if (squareNumber != 12)
            rightSquare = parentRow.GetSquareByNumber(squareNumber + 1);
        if (parentRow.GetRowNumber() != 16)
            belowSquare = gridRowHolder.GetRowByNumber(parentRow.GetRowNumber() + 1).GetSquareByNumber(squareNumber);
    }

    private void OnMouseOver()
    {
        active = true;
    }

    public bool AttachCardToSquare(HackCard hackCard)
    {
        int timesToRotate = GetCountToNextLegalRotation(hackCard);
        if (timesToRotate != -1)
        {
            HackCard newHackCard = Instantiate(hackCard, new Vector2(hackholder.transform.position.x, hackholder.transform.position.y), Quaternion.identity);
            newHackCard.transform.SetParent(hackholder.transform);
            newHackCard.transform.localScale = new Vector3(1, 1, 1);

            for (int i = 0; i < timesToRotate; i++)
            {
                RotateCardNinetyDegrees(newHackCard);
                newHackCard.RotateCircuitsAndSpikesNinetyDegrees();
            }

            newHackCard.SetupUI(GetCountToPreviousLegalRotation(newHackCard, 1), GetCountToNextLegalRotation(newHackCard, 1));
            attachedHackCard = newHackCard;
            TurnOffSquareImage();
            return true;
        } else
        {
            return false;
        }
    }

    private void TurnOffSquareImage()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private int GetCountToPreviousLegalRotation(HackCard hackcard, int startingRotation = 0)
    {
        FindAndStoreAdjacentSquares();

        for (int rotations = startingRotation; rotations < 4; rotations++)
        {
            string rotatedLeftCircuit = hackcard.GetLeftCircuit();
            string rotatedTopCircuit = hackcard.GetTopCircuit();
            string rotatedRightCircuit = hackcard.GetRightCircuit();
            string rotatedBottomCircuit = hackcard.GetBottomCircuit();
            for (int i = 0; i < rotations; i++)
            {
                string previousLeftCircuit = rotatedLeftCircuit;
                rotatedLeftCircuit = rotatedTopCircuit;
                rotatedTopCircuit = rotatedRightCircuit;
                rotatedRightCircuit = rotatedBottomCircuit;
                rotatedBottomCircuit = previousLeftCircuit;
            }
            bool isCurrentRotationLegal = CheckRotation(rotatedLeftCircuit, rotatedTopCircuit, rotatedRightCircuit, rotatedBottomCircuit);
            if (isCurrentRotationLegal)
            {
                return rotations;
            }
        }
        return -1;
    }

    private int GetCountToNextLegalRotation(HackCard hackcard, int startingRotation = 0)
    {
        FindAndStoreAdjacentSquares();

        // We check all four rotations for a match and allow placing the square if there are any
        // If there are, we return the number of rotations necessary for later processing
        // If there are none, we return -1
        // Starting rotation is set to 1 if we want to find the next legal rotation and skip 'no rotation'
        for (int rotations = startingRotation; rotations < 4; rotations++) {
            string rotatedLeftCircuit = hackcard.GetLeftCircuit();
            string rotatedTopCircuit = hackcard.GetTopCircuit();
            string rotatedRightCircuit = hackcard.GetRightCircuit();
            string rotatedBottomCircuit = hackcard.GetBottomCircuit();
            for (int i = 0; i < rotations; i++)
            {
                string previousLeftCircuit = rotatedLeftCircuit;
                rotatedLeftCircuit = rotatedBottomCircuit;
                rotatedBottomCircuit = rotatedRightCircuit;
                rotatedRightCircuit = rotatedTopCircuit;
                rotatedTopCircuit = previousLeftCircuit;
            }
            bool isCurrentRotationLegal = CheckRotation(rotatedLeftCircuit, rotatedTopCircuit, rotatedRightCircuit, rotatedBottomCircuit);
            if (isCurrentRotationLegal)
            {
                return rotations;
            }
        }
        return -1;
    }

    private string IsConnectionOk(string currentLeftCircuit, string leftCardRightCircuit)
    {
        if (currentLeftCircuit == leftCardRightCircuit)
        {
            return "connected";
        }
        return "mismatch";
    }

    private void RotateCardNinetyDegrees(HackCard cardToRotate)
    {
        // Variables must be shifted one to the right (eg left => top, top => right, right => bottom, bottom => left)
        // this must be done for circuits AND spikes
        cardToRotate.transform.Find("Card").gameObject.transform.Rotate(0, 0, 270);
        //cardToRotate.transform.Rotate(0, 0, 270);
    }

    private bool CheckRotation(string rotatedLeftCircuit, string rotatedTopCircuit, string rotatedRightCircuit, string rotatedBottomCircuit)
    {
        string leftConnectionCheck;
        if (leftSquare && leftSquare.DoesSquareHaveCard())
        {
            leftConnectionCheck = IsConnectionOk(rotatedLeftCircuit, leftSquare.GetAttachedCard().GetRightCircuit());
        }
        else
        {
            leftConnectionCheck = "ok";
        }
        //Debug.Log("Left connection check: " + leftConnectionCheck);

        string aboveConnectionCheck;
        if (aboveSquare && aboveSquare.DoesSquareHaveCard())
        {
            aboveConnectionCheck = IsConnectionOk(rotatedTopCircuit, aboveSquare.GetAttachedCard().GetBottomCircuit());
        }
        else
        {
            aboveConnectionCheck = "ok";
        }
        //Debug.Log("Above connection check: " + aboveConnectionCheck);

        string rightConnectionCheck;
        if (rightSquare && rightSquare.DoesSquareHaveCard())
        {
            rightConnectionCheck = IsConnectionOk(rotatedRightCircuit, rightSquare.GetAttachedCard().GetLeftCircuit());
        }
        else
        {
            rightConnectionCheck = "ok";
        }
        //Debug.Log("Right connection check: " + rightConnectionCheck);

        string belowConnectionCheck;
        if (belowSquare && belowSquare.DoesSquareHaveCard())
        {
            belowConnectionCheck = IsConnectionOk(rotatedBottomCircuit, belowSquare.GetAttachedCard().GetTopCircuit());
        }
        else
        {
            belowConnectionCheck = "ok";
        }

        if (leftConnectionCheck == "mismatch" || aboveConnectionCheck == "mismatch" || rightConnectionCheck == "mismatch" || belowConnectionCheck == "mismatch")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public HackCard GetAttachedCard()
    {
        return GetComponentInChildren<HackCard>();
    }

    public bool DoesSquareHaveCard()
    {
        HackCard attachedCard = GetComponentInChildren<HackCard>();
        if (attachedCard)
        {
            return true;
        }
        return false;
    }

    private void OnMouseExit()
    {
        active = false;
    }

    public bool IsActive()
    {
        return active;
    }

    public void LogId()
    {
        active = false;
    }

    public string SquareGridLocation()
    {
        return "x: " + squareNumber + " y: " + parentRow.GetRowNumber().ToString();
    }
}
