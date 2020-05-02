using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackGridSquare : MonoBehaviour
{
    [SerializeField] int squareNumber;
    [SerializeField] Sprite[] imageOptions;
    [SerializeField] GameObject hackholder;

    bool active = false;

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
        // Check it against the things next to it...
        // Get the left square
        // Get the right square
        // Get the top square
        // Get the bottom square
        GameObject parentRowObject = transform.parent.gameObject;
        GridRow parentRow = parentRowObject.GetComponent<GridRow>();
        parentRow.LogRowNumber();
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
}
