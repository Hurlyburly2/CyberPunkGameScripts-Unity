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
    Color defaultColor;
    MapConfig mapConfig;

    //state
    bool isActive;
    // state can be normal, movingDown
    string state;
    bool playerPresent;
    bool shroud;

    private void OnMouseUpAsButton()
    {
        if (!shroud && !mapConfig.GetIsAMenuOpen())
        {
            if (playerPresent)
            {

            } else
            {
                mapConfig.SetIsAMenuOpen(true);
                NeighboringNodeMenu menu = mapConfig.GetNeighboringNodeMenu();
                menu.InitializeMenu(this);
            }
        }
    }

    private void OnMouseDown()
    {
        if (!shroud && !mapConfig.GetIsAMenuOpen())
        {
            SetState("movingDown");
        }
    }

    private void OnMouseUp()
    {
        if (!shroud)
        {
            SetState("movingUp");
        }
    }

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

    public void SetPlayerStart()
    {
        RemoveShroud();
        RemoveAdjacentShrouds();
        playerPresent = true;
        PlayerMarker playerMarkerPrefab = parentRow.GetMapGrid().GetPlayerMarkerPrefab();
        Vector3 markerPosition = GetPlayerMarkerPosition();
        PlayerMarker newPlayerMarker = Instantiate(playerMarkerPrefab, markerPosition, Quaternion.identity);
    }

    public Vector3 GetPlayerMarkerPosition()
    {
        return new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
    }

    public void InitializeSquare(Sprite newImage)
    {
        playerPresent = false;
        isActive = true;
        GetComponent<PolygonCollider2D>().enabled = true;
        parentRow.AddInitializedSquareToList(this);
        GetComponent<SpriteRenderer>().sprite = newImage;
    }

    public void AddShroud()
    {
        shroud = true;
        GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 0.25f, 1);
    }

    public void RemoveShroud()
    {
        shroud = false;
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    private void RemoveAdjacentShrouds()
    {
        List<MapSquare> adjacentSquares = GetAdjacentSquares();
        Debug.Log("found adjacent squares: " + adjacentSquares.Count);
        foreach (MapSquare square in adjacentSquares)
        {
            square.RemoveShroud();
        }
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
        targetYPos = defaultYPos - 0.35f;
        state = "normal";
        isActive = false;
    }

    private void Start()
    {
        defaultColor = GetComponent<SpriteRenderer>().color;
        AddShroud();
        mapConfig = FindObjectOfType<MapConfig>();
    }

    void Update()
    {
        float step = 2.5f * Time.deltaTime;

        if (state == "movingDown")
        {
            if (Input.touchCount == 3 || Input.touchCount == 2)
            {
                // the click was accidental, user actually means to drag the camera if they're
                // touching with three fingers, or to zoom if they're touching with two
                SetState("movingUp");
            }
            Vector3 targetPos = new Vector3(transform.position.x, targetYPos, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        } else if (state == "movingUp")
        {
            Vector3 targetPos = new Vector3(transform.position.x, defaultYPos, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            if (transform.position.y == defaultYPos)
            {
                SetState("normal");
            }
        }
    }

    private void SetState(string newState)
    {
        state = newState;
    }
}
