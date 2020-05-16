using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckClickController : MonoBehaviour
{
    // This class is for coordinating the different CheckClick objects, and allowing
    // them to act in a coordinated fashion

    string state = "normal";
        // possibilities: normal, cameradragging
    string deckClickResult = null;
    string discardClickResult = null;
    string topLeftZoneResult = null;
    string deckZoneResult = null;
    string cameraResult = null;

    public void ListenForClickResults()
    {
        StartCoroutine(WaitForAllResults());
    }

    private IEnumerator WaitForAllResults()
    {
        while (deckClickResult == null || discardClickResult == null || topLeftZoneResult == null || cameraResult == null)
        {
            yield return null;
        }
        if (deckClickResult == "attemptPlaceCard")
        {
            AttemptPlaceCard();
        } else if (cameraResult == "attemptpan"
            && discardClickResult == "notOverDiscardZone"
            && topLeftZoneResult == "notOverTopLeftZone"
            && deckZoneResult == "notOverDeckZone")
        {
            Debug.Log("hit camera if statement");
            FindObjectOfType<PanZoom>().AllowCameraMovement();
        }
        ResetAllResults();
        state = "normal";
        // check click results and then place or discard card as needed
        // place card or discard card
    }

    private void ResetAllResults()
    {
        deckClickResult = null;
        discardClickResult = null;
        topLeftZoneResult = null;
        deckZoneResult = null;
        cameraResult = null;
    }

    private void AttemptPlaceCard()
    {
        if (discardClickResult == "overDiscardZone")
        {
            DiscardCard();
        } else if (topLeftZoneResult == "overTopLeftZone")
        {
            Debug.Log("Over top left zone, do not play or discard card");
        } else if (deckZoneResult == "overDeckZone")
        {
            Debug.Log("Over deck zone, do not play or discard card");
        } else
        {
            AttachCardToSquare();
        }
    }

    public void SetCameraDragResult(string result)
    {
        // possible results = attemptpan
        // Camera drag should be if nothing important is clicked
        cameraResult = result;
    }

    public void SetDeckClickResult(string result)
    {
        // possible results: attemptPlaceCard
        deckClickResult = result;
    }

    public void SetDiscardClickResult(string result)
    {
        // possible results: overDiscardZone, notOverDiscardZone
        discardClickResult = result;
    }

    public void SetTopLeftZoneResult(string result)
    {
        // possible results: overTopLeftZone, notOverTopLeftZone
        topLeftZoneResult = result;
    }

    public void SetDeckZoneResult(string result)
    {
        // possible results: overDeckZone, notOverDeckZone
        deckZoneResult = result;
    }

    private void DiscardCard()
    {
        HackDeck hackDeck = FindObjectOfType<HackDeck>();
        hackDeck.SendTopCardToDiscard();
    }

    private void AttachCardToSquare()
    {
        HackGridSquare[] allSquares = FindObjectsOfType<HackGridSquare>();
        HackDeck hackDeck = FindObjectOfType<HackDeck>();

        foreach (HackGridSquare square in allSquares)
        {
            if (square.IsActive() && square.IsPlacementAllowed())
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

    public string GetBehavior()
    {
        return "iunno";
    }

    public string GetState()
    {
        return state;
    }

    public void SetDraggingDeckState()
    {
        state = "draggingDeck";
    }

    public void SetNormalState()
    {
        state = "normal";
    }
}
