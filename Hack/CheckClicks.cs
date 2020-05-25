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
    CheckClickController checkClickController;
    HackBattleData hackBattleData;

    void Awake()
    {
        // Get both of the components we need to do this
        this.raycaster = GetComponent<GraphicRaycaster>();
        checkClickController = FindObjectOfType<CheckClickController>();
        hackBattleData = FindObjectOfType<HackBattleData>();
    }

    void Update()
    {
        if (checkClickController.GetState() != "tilePicker")
        {
            switch (clickItemName)
            {
                case "HackDeck":
                    CaseHackDeckUpdate();
                    break;
                case "DiscardZone":
                    CaseDiscardZoneUpdate();
                    break;
                case "TopLeftZone":
                    CaseTopLeftZoneUpdate();
                    break;
                case "DeckZone":
                    CaseDeckZoneUpdate();
                    break;
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

    public void SetDraggingState()
    {
        state = "dragging";
    }

    public void SetDiscardingState()
    {
        state = "discarding";
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

    private void CaseTopLeftZoneUpdate()
    {
        if (state == "normal")
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //Set up the new Pointer Event
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                pointerData.position = Input.mousePosition;
                this.raycaster.Raycast(pointerData, results);

                bool areWeOverTopLeftZone = false;
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.name == "TopLeftBase")
                    {
                        areWeOverTopLeftZone = true;
                    }
                }
                if (areWeOverTopLeftZone)
                {
                    checkClickController.SetTopLeftZoneResult("overTopLeftZone");
                }
                else
                {
                    checkClickController.SetTopLeftZoneResult("notOverTopLeftZone");
                }
            }
        }
    }

    private void CaseDiscardZoneUpdate()
    {
        if (state == "normal")
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //Set up the new Pointer Event
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                pointerData.position = Input.mousePosition;
                this.raycaster.Raycast(pointerData, results);

                bool areWeOverDiscardZone = false;
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.name == "DiscardZone")
                    {
                        areWeOverDiscardZone = true;
                    }
                }
                if (areWeOverDiscardZone)
                {
                    checkClickController.SetDiscardClickResult("overDiscardZone");
                } else
                {
                    checkClickController.SetDiscardClickResult("notOverDiscardZone");
                }
            }
        }
    }

    private void CaseHackDeckUpdate()
    {
        if (state == "normal")
        {
            //Check if the left Mouse button is clicked
            if (Input.GetKeyDown(KeyCode.Mouse0) && hackBattleData.IsPlayerAllowedToDragCard())
            {
                //Set up the new Pointer Event
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                pointerData.position = Input.mousePosition;
                this.raycaster.Raycast(pointerData, results);

                if (AreWeClickingOnCard(results) && !hackDeck.IsDeckEmpty())
                {
                    checkClickController.SetDraggingDeckState();
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

                checkClickController.SetDeckClickResult("attemptPlaceCard");
                checkClickController.ListenForClickResults();
                state = "goingback";
            }
        }
    }

    private void CaseDeckZoneUpdate()
    {
        if (state == "normal")
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //Set up the new Pointer Event
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                pointerData.position = Input.mousePosition;
                this.raycaster.Raycast(pointerData, results);

                bool areWeOverDeckZone = false;
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.name == "DeckZone")
                    {
                        areWeOverDeckZone = true;
                    }
                }
                if (areWeOverDeckZone)
                {
                    checkClickController.SetDeckZoneResult("overDeckZone");
                }
                else
                {
                    checkClickController.SetDeckZoneResult("notOverDeckZone");
                }
            }
        }
    }
}
