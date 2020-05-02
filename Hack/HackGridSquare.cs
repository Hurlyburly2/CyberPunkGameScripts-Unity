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
        if (IsPlacementLegal(hackCard))
        {
            HackCard newHackCard = Instantiate(hackCard, new Vector2(hackholder.transform.position.x, hackholder.transform.position.y), Quaternion.identity);
            newHackCard.transform.SetParent(hackholder.transform);
            newHackCard.transform.localScale = new Vector3(1, 1, 1);
            return true;
        } else
        {
            return false;
        }
    }

    private bool IsPlacementLegal(HackCard hackcard)
    {
        FindAndStoreAdjacentSquares();

        string leftConnectionCheck;
        if (leftSquare && leftSquare.DoesSquareHaveCard())
        {
            leftConnectionCheck = IsConnectionOk(hackcard.GetLeftCircuit(), leftSquare.GetAttachedCard().GetRightCircuit());
        } else
        {
            leftConnectionCheck = "ok";
        }
        Debug.Log("Left connection check: " + leftConnectionCheck);

        string aboveConnectionCheck;
        if (aboveSquare && aboveSquare.DoesSquareHaveCard())
        {
            aboveConnectionCheck = IsConnectionOk(hackcard.GetTopCircuit(), aboveSquare.GetAttachedCard().GetBottomCircuit());
        } else
        {
            aboveConnectionCheck = "ok";
        }
        Debug.Log("Above connection check: " + aboveConnectionCheck);

        string rightConnectionCheck;
        if (rightSquare && rightSquare.DoesSquareHaveCard())
        {
            rightConnectionCheck = IsConnectionOk(hackcard.GetRightCircuit(), rightSquare.GetAttachedCard().GetLeftCircuit());
        } else
        {
            rightConnectionCheck = "ok";
        }
        Debug.Log("Right connection check: " + rightConnectionCheck);

        string belowConnectionCheck;
        if (belowSquare && belowSquare.DoesSquareHaveCard())
        {
            belowConnectionCheck = IsConnectionOk(hackcard.GetBottomCircuit(), belowSquare.GetAttachedCard().GetTopCircuit());
        } else
        {
            belowConnectionCheck = "ok";
        }
        Debug.Log("Below connection check: " + belowConnectionCheck);

        // Check all four directions, at minimum all connections must be "ok" with one "connected"
            // More than one "connected" is possible
            // Cannot place with no "connected" UNLESS its the first card placed
        // We check this for all four card orientations and keep track of the amount of ok orientations
            // If more than one, we allow the user to rotate the card
            // Disable card dragging during rotation
        // If the card can only be placed in a rotated configuration, then rotate it and
            // update all spike/circuit values

        if (leftConnectionCheck == "mismatch" || aboveConnectionCheck == "mismatch" || rightConnectionCheck == "mismatch" || belowConnectionCheck == "mismatch")
        {
            return false;
        }
        return true;
    }

    private string IsConnectionOk(string currentLeftCircuit, string leftCardRightCircuit)
    {
        if (currentLeftCircuit == leftCardRightCircuit)
        {
            return "connected";
        }
        return "mismatch";
    }

    private void RotateCardNinetyDegrees(int times = 1)
    {
        // Variables must be shifted one to the right (eg left => top, top => right, right => bottom, bottom => left)
            // this must be done for circuits AND spikes
        // We perform the rotation x times, 2 = 180 degrees, 3 = 270
    }

    private void CheckRotation(int times = 1)
    {
        // Store the variables for a card rotation and pass them into the connection ok
            // methods
        // This is done so we can test rotations without mutating the cards actual values
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
