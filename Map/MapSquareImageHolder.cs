using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSquareImageHolder : MonoBehaviour
{
    [SerializeField] Sprite[] citySquares;

    List<Sprite> unusedSquares;
    List<Sprite> usedSquares;

    [SerializeField] Sprite[] slumImages;

    List<Sprite> unusedImages;
    List<Sprite> usedImages;

    public Sprite GetSquareImage()
    {
        if (unusedSquares.Count == 0)
        {
            unusedSquares = usedSquares;
            usedSquares = new List<Sprite>();
        }

        Sprite imageToReturn = unusedSquares[Random.Range(0, unusedSquares.Count)];
        usedSquares.Add(imageToReturn);
        unusedSquares.Remove(imageToReturn);
        return imageToReturn;
    }

    public Sprite GetLocationImage()
    {
        if (unusedImages.Count == 0)
        {
            unusedImages = usedImages;
            usedImages = new List<Sprite>();
        }

        Sprite imageToReturn = unusedImages[Random.Range(0, unusedImages.Count)];
        usedImages.Add(imageToReturn);
        unusedImages.Remove(imageToReturn);
        return imageToReturn;
    }

    public void Initialize(string mapType)
    {
        unusedSquares = new List<Sprite>();
        usedSquares = new List<Sprite>();

        unusedImages = new List<Sprite>();
        usedImages = new List<Sprite>();

        switch (mapType)
        {
            case "slums":
                unusedSquares.AddRange(citySquares);
                unusedImages.AddRange(slumImages);
                break;
        }
    }
}
