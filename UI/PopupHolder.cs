using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupHolder : MonoBehaviour
{
    [SerializeField] Popup[] popups;
    // 0 - discard cards

    string whichPopup;

    //state:
    List<Popup> currentPopups = new List<Popup>();

    public void SpawnDiscardPopup(int amountOfCardsToDiscard)
    {
        string message = "Discard " + amountOfCardsToDiscard + " cards";
        whichPopup = "discard";
        SpawnPopup(message);
    }

    public void SpawnStatusPopup(string message)
    {
        whichPopup = "statusHelper";
        SpawnPopup(message);
    }

    public void SpawnWeaknessesInHandPopup()
    {
        whichPopup = "weaknessInHand";
        SpawnPopup("You must play all weaknesses before ending your turn");
        StartCoroutine(DestroyTemporaryMessageAfterTime());
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

    private IEnumerator DestroyTemporaryMessageAfterTime()
    {
        yield return new WaitForSeconds(4);
        if (whichPopup == "weaknessInHand")
        {
            DestroyAllPopups();
        }
    }
}
