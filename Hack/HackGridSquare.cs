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

    public void AttachCardToSquare(HackCard hackCard)
    {
        AllHackCards allHackCards = FindObjectOfType<AllHackCards>();
        HackCard newHackCard = Instantiate(hackCard, new Vector2(hackholder.transform.position.x, hackholder.transform.position.y), Quaternion.identity);
        newHackCard.transform.SetParent(hackholder.transform);
        newHackCard.transform.localScale = new Vector3(1, 1, 1);
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
