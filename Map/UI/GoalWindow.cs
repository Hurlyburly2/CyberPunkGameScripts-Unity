using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionText;
    MapSquare currentSquare;

    public void OpenGoalWindow(MapSquare newSquare)
    {
        gameObject.SetActive(true);
        descriptionText.text = GenerateDescriptionText();
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(true);
        currentSquare = newSquare;
    }

    private string GenerateDescriptionText()
    {
        return FindObjectOfType<MapData>().GetJob().GetJobMiddleTextTwo();
    }

    public void CloseGoalWindow()
    {
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        FindObjectOfType<MapData>().SetHasGoalBeenReached(true);

        MapSquare extractionSquare = SetExtractionPoint();
        FindObjectOfType<PanZoomMap>().ShowExtractionSquare(extractionSquare);

        gameObject.SetActive(false);
    }

    private MapSquare SetExtractionPoint()
    {
        MapSquare[] allSquares = FindObjectsOfType<MapSquare>();
        List<MapSquare> activeSquares = new List<MapSquare>();

        // get the distance for each square
        foreach (MapSquare square in allSquares)
        {
            if (square.IsActive())
            {
                square.SetTemporaryPositionMeasurement(currentSquare.GetDistanceToTargetSquare(square));
                activeSquares.Add(square);
            }
        }

        activeSquares.Sort(SortByDistance);
        int longestDistance = activeSquares[activeSquares.Count - 1].GetDistanceMeasurement();

        // then narrow down our options to distance = the most, or distance = the most - 1
        List<MapSquare> potentialExtractionSquares = new List<MapSquare>();
        foreach (MapSquare square in activeSquares)
        {
            if (square.GetDistanceMeasurement() >= longestDistance - 1)
            {
                potentialExtractionSquares.Add(square);
            }
        }

        MapSquare extractionSquare = potentialExtractionSquares[Random.Range(0, potentialExtractionSquares.Count)];
        return extractionSquare;
    }

    private int SortByDistance(MapSquare square1, MapSquare square2)
    {
        return square1.GetDistanceMeasurement().CompareTo(square2.GetDistanceMeasurement());
    }
}
