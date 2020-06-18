using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquareRow : MonoBehaviour
{
    // config
    [SerializeField] int rowNumber;
    [SerializeField] MapSquare[] mapSquares;
    [SerializeField] MapGrid mapGrid;
    MapSquareImageHolder mapSquareImageHolder;

    // state
    List<MapSquare> activeSquares;
    // we use newActiveSquares so squares can tell the row they're being added while we're looping through an active square
    // collection, only then after the loop is completed can the new squares be added
    List<MapSquare> newActiveSquares = new List<MapSquare>();

    private void Start()
    {
        activeSquares = new List<MapSquare>();
        mapSquareImageHolder = FindObjectOfType<MapSquareImageHolder>();
    }

    public int AttemptToSpawnSquares(int percentChange, int remainingSquaresToSpawn, int verticalModifer)
    {
        int newSquareCount = 0;
        foreach (MapSquare square in activeSquares)
        {
            List<MapSquare> adjacentSquares = square.GetAdjacentSquares();
            foreach (MapSquare adjacentSquare in adjacentSquares)
            {
                if (newSquareCount < remainingSquaresToSpawn)
                {
                    int modifier = 0;
                    if (square.GetUpSquare() == adjacentSquare)
                        modifier += 25;

                    if (!adjacentSquare.IsActive() && Random.Range(0, 100) < percentChange + modifier)
                    {
                        adjacentSquare.InitializeSquare(mapSquareImageHolder.GetSquareImage());
                        newSquareCount++;
                    }
                }
            }
        }
        mapGrid.ConsolidateNewActiveSquaresInAllRows();
        return newSquareCount;
    }

    public void ConsolidateNewActiveSquares()
    {
        activeSquares.AddRange(newActiveSquares);
        newActiveSquares = new List<MapSquare>();
    }

    public void InitializeFirstSquare(int squareNumber, Sprite image)
    {
        activeSquares.Add(mapSquares[squareNumber]);

        MapSquare firstSquare = mapSquares[squareNumber];
        firstSquare.InitializeSquare(image);
        firstSquare.SetPlayerStart();
    }

    public void AddInitializedSquareToList(MapSquare newlyInitializedSquare)
    {
        newActiveSquares.Add(newlyInitializedSquare);
    } 

    public MapSquare GetSquareByRowPosition(int rowPosition)
    {
        return mapSquares[rowPosition];
    }

    public void SetLayerOrder(int number)
    {
        foreach (MapSquare square in mapSquares)
        {
            square.InitialSetup(number);
            number++;
        }
    }

    public int GetRowNumber()
    {
        return rowNumber;
    }

    public MapGrid GetMapGrid()
    {
        return mapGrid;
    }

    public bool ContainsActiveSquares()
    {
        if (activeSquares.Count > 0)
        {
            return true;
        }
        return false;
    }

    public int CountActiveSquares()
    {
        return activeSquares.Count;
    }
}
