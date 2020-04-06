using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupHolder : MonoBehaviour
{
    [SerializeField] Popup[] popups;
    // 0 - discard cards

    //state:
    List<Popup> currentPopups = new List<Popup>();

    public void SpawnDiscardPopup(int amountOfCardsToDiscard)
    {
        string message = "Discard " + amountOfCardsToDiscard + " cards";
        SpawnPopup(message);
    }

    public void SpawnStatusPopup(string message)
    {
        SpawnPopup(message);
    }

    private void SpawnPopup(string message)
    {
        DestroyAllPopups();
        Popup newPopup = Instantiate(popups[0], new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        newPopup.transform.SetParent(this.transform);
        newPopup.SetText(message);
        currentPopups.Add(newPopup);
    }

    public void DestroyAllPopups()
    {
        foreach(Popup popup in currentPopups)
        {
            popup.DestroySelf();
        }
        currentPopups = new List<Popup>();
    }
}
