using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckClickController : MonoBehaviour
{
    // This class is for coordinating the different CheckClick objects, and allowing
    // them to act in a coordinated fashion

    string state = "normal";
        // possibilities: normal, 
    string deckClickResult = null;
    string discardClickResult = null;

    public void ListenForClickResults()
    {
        StartCoroutine(WaitForAllResults());
    }

    private IEnumerator WaitForAllResults()
    {
        while (deckClickResult == null || discardClickResult == null)
        {
            yield return null;
        }
        if (deckClickResult == "attemptPlaceCard")
        {
            AttemptPlaceCard();
        }
        deckClickResult = null;
        discardClickResult = null;
        state = "normal";
        // check click results and then place or discard card as needed
        // place card or discard card
    }

    private void AttemptPlaceCard()
    {
        // Add the top UI bar as well
        if (discardClickResult == "overDiscardZone")
        {
            Debug.Log("Discard, do not play, card");
        } else
        {
            AttachCardToSquare();
        }
    }

    public void SetDeckClickResult(string result)
    {
        // possible results008s: attemptPlaceCard
        deckClickResult = result;
    }

    public void SetDiscardClickResult(string result)
    {
        // possible results: overDiscardZone, notOverDiscardZone
        discardClickResult = result;
    }

    private void AttachCardToSquare()
    {
        HackGridSquare[] allSquares = FindObjectsOfType<HackGridSquare>();
        HackDeck hackDeck = FindObjectOfType<HackDeck>();

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
