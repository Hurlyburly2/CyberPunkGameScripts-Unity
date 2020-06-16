using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquareImageHolder : MonoBehaviour
{
    [SerializeField] Sprite[] citySquares;

    List<Sprite> unusedSquares;
    List<Sprite> usedSquares;

    public Sprite GetSquareImage()
    {
        if (unusedSquares.Count <= 0)
        {
            unusedSquares = usedSquares;
            usedSquares = new List<Sprite>();
        }

        Sprite imageToReturn = unusedSquares[Random.Range(0, unusedSquares.Count)];
        usedSquares.Add(imageToReturn);
        unusedSquares.Remove(imageToReturn);
        return imageToReturn;
    }

    public void Initialize(string mapType)
    {
        unusedSquares = new List<Sprite>();
        usedSquares = new List<Sprite>();

        switch (mapType)
        {
            case "city":
                unusedSquares.AddRange(citySquares);
                break;
        }
    }
}
