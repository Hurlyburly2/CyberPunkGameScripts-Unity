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

        if (leftSquare != null)
            Debug.Log("left square: " + leftSquare.SquareGridLocation());
        if (aboveSquare != null)
            Debug.Log("top square: " + aboveSquare.SquareGridLocation());
        if (rightSquare != null)
            Debug.Log("right square: " + rightSquare.SquareGridLocation());
        if (belowSquare != null)
            Debug.Log("below square: " + belowSquare.SquareGridLocation());
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
        string[] connections = hackcard.GetConnectionsArray();

        // First, do it without rotation

        // Check all four directions, at minimum all connections must be "ok" with one "connected"
            // More than one "connected" is possible
            // Cannot place with no "connected" UNLESS its the first card placed
        // We check this for all four card orientations and keep track of the amount of ok orientations
            // If more than one, we allow the user to rotate the card
            // Disable card dragging during rotation
        // If the card can only be placed in a rotated configuration, then rotate it and
            // update all spike/circuit values

        return true;
    }

    private string IsLeftConnectionOk()
    {
        // Pass values here, don't use class variables, so we can
            // reuse these checks with the temporary rotations
        // For each direction, we check for a connection
            // Match against a card with the same color
                // return "connected"
            // Match against a grid square with no card
                // return "ok"
        // AND we check for no conflicting connections
        // Conflicting connections include:
            // match against a card with a different color
            // match against a card with no color in that direction
                // return "mismatch"

        return "ok";
    }

    private string IsAboveConnectionOk()
    {
        return "ok";
    }

    private string IsBelowConnectionOk()
    {
        return "ok";
    }

    private string IsRightConnectionOk()
    {
        return "ok";
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
