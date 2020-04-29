using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CheckClicks : MonoBehaviour
{
    [SerializeField] HackDeck hackDeck;

    // Normal raycasts do not work on UI elements, they require a special kind
    GraphicRaycaster raycaster;
    string state = "normal";

    void Awake()
    {
        // Get both of the components we need to do this
        this.raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        if (state == "normal")
        {
            //Check if the left Mouse button is clicked
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Set up the new Pointer Event
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                pointerData.position = Input.mousePosition;
                this.raycaster.Raycast(pointerData, results);

                state = "dragging";
            }
        } else if (state == "dragging")
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //Set up the new Pointer Event
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                pointerData.position = Input.mousePosition;
                this.raycaster.Raycast(pointerData, results);

                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                //foreach (RaycastResult result in results)
                //{
                //    Debug.Log("Hit " + result.gameObject.name);
                //}
                state = "goingback";
                FindMouseOver();
            }
        }
    }

    public string GetClickState()
    {
        return state;
    }

    public void SetNormalState()
    {
        state = "normal";
    }

    private void FindMouseOver()
    {
        HackGridSquare[] allSquares = FindObjectsOfType<HackGridSquare>();
        foreach (HackGridSquare square in allSquares)
        {
            if (square.IsActive())
            {
                square.AttachCardToSquare(hackDeck.GetTopCard());
                return;
            }
        }
    }
}
