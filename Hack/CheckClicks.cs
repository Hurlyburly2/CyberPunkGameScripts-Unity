using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CheckClicks : MonoBehaviour
{
    [SerializeField] HackDeck hackDeck;
    [SerializeField] string clickItemName;

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
        switch(clickItemName)
        {
            case "HackDeck":
                CaseHackDeckUpdate();
                break;
            case "DiscardZone":
                CaseDiscardZoneUpdate();
                break;
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

    private bool AreWeClickingOnCard(List<RaycastResult> raycastResults)
    {
        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.name == "HackDeckCardBack")
            {
                return true;
            }
        }
        return false;
    }

    private void AttachCardToSquare()
    {
        HackGridSquare[] allSquares = FindObjectsOfType<HackGridSquare>();
        foreach (HackGridSquare square in allSquares)
        {
            if (square.IsActive())
            {
                bool wasAttachmentSuccessful = square.AttachCardToSquare(hackDeck.GetTopCard());
                if (wasAttachmentSuccessful)
                {
                    hackDeck.RemoveTopCardFromDeck();
                }
                return;
            }
        }
    }

    private void CaseDiscardZoneUpdate()
    {
        if (state == "normal")
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Debug.Log("Discardzone keyup");
                //Set up the new Pointer Event
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                pointerData.position = Input.mousePosition;
                this.raycaster.Raycast(pointerData, results);

                foreach (RaycastResult result in results)
                {
                    Debug.Log(result.gameObject.name);
                }
            }
        }
    }

    private void CaseHackDeckUpdate()
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

                if (AreWeClickingOnCard(results) && !hackDeck.IsDeckEmpty())
                {
                    state = "dragging";
                }
            }
        }
        else if (state == "dragging")
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //Set up the new Pointer Event
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                pointerData.position = Input.mousePosition;
                this.raycaster.Raycast(pointerData, results);

                state = "goingback";
                AttachCardToSquare();
            }
        }
    }
}
