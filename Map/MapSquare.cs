using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquare : MonoBehaviour
{
    //config
    [SerializeField] int rowPosition; // 0-19 position in row
    [SerializeField] MapSquareRow parentRow;
    int minSquareRow = 0;
    int maxSquareRow = 19;
    int minSquareColumn = 0;
    int maxSquareColumn = 19;
    float defaultYPos;
    float targetYPos;
    float squareMoveSpeed = 1f;

    //state
    bool isActive;
    // state can be normal, movingDown
    string state;

    public List<MapSquare> GetAdjacentSquares()
    {
        List<MapSquare> adjacentSquares = new List<MapSquare>();

        if (rowPosition > minSquareRow)
        {
            adjacentSquares.Add(parentRow.GetSquareByRowPosition(rowPosition - 1));
        }
        if (rowPosition < maxSquareRow)
        {
            adjacentSquares.Add(parentRow.GetSquareByRowPosition(rowPosition + 1));
        }
        if (parentRow.GetRowNumber() > minSquareColumn)
        {
            adjacentSquares.Add(parentRow.GetMapGrid().GetRowByNumber(parentRow.GetRowNumber() - 1).GetSquareByRowPosition(rowPosition));
        }
        if (parentRow.GetRowNumber() < maxSquareColumn)
        {
            adjacentSquares.Add(parentRow.GetMapGrid().GetRowByNumber(parentRow.GetRowNumber() + 1).GetSquareByRowPosition(rowPosition));
        }

        return adjacentSquares;
    }

    public MapSquare GetUpSquare()
    {
        if (parentRow.GetRowNumber() < maxSquareColumn)
        {
            return parentRow.GetMapGrid().GetRowByNumber(parentRow.GetRowNumber() + 1).GetSquareByRowPosition(rowPosition);
        }
        return null;
    }

    public void InitializeSquare(Sprite newImage)
    {
        isActive = true;
        GetComponent<PolygonCollider2D>().enabled = true;
        parentRow.AddInitializedSquareToList(this);
        GetComponent<SpriteRenderer>().sprite = newImage;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void InitialSetup(int layerNumber)
    {
        GetComponent<SpriteRenderer>().sortingOrder = layerNumber;
        GetComponent<PolygonCollider2D>().enabled = false;
        defaultYPos = transform.position.y;
        targetYPos = defaultYPos - 8f;
        state = "normal";
        isActive = false;
    }

    void Update()
    {
        float step = 1 * Time.deltaTime;

        Vector3 targetPos = new Vector3(transform.position.x, targetYPos, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
    }

    private void OnMouseDown()
    {
    }

    private void OnMouseUp()
    {
        Debug.Log("default y pos: " + defaultYPos);
        Debug.Log("target y pos: " + targetYPos);
        Debug.Log("current y pos: " + transform.position.y);
    }

    private void SetState(string newState)
    {
        state = newState;
    }
}
