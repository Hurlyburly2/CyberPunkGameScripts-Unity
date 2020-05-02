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

        FindAndStoreAdjacentSquares();

        return true;
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
