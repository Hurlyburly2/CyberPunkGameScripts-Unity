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
        StartCoroutine(DestroyTemporaryMessageAfterTime(4));
    }

    public void SpawnNotEnoughEnergyPopup()
    {
        whichPopup = "notEnoughEnergy";
        SpawnPopup("Not enough energy");
        StartCoroutine(DestroyTemporaryMessageAfterTime(1));
    }

    public void SpawnFizzledPopup()
    {
        whichPopup = "notEnoughEnergy";
        SpawnPopup("Card Fizzled");
        StartCoroutine(DestroyTemporaryMessageAfterTime(0.5f));
    }

    public void SpawnStancePopup()
    {
        whichPopup = "notEnoughEnergy";
        SpawnPopup("You may only play one Stance card per turn.");
        StartCoroutine(DestroyTemporaryMessageAfterTime(1));
    }

    public void SpawnCouldNotPlayPopup()
    {
        whichPopup = "notEnoughEnergy";
        SpawnPopup("Cannot play that card.");
        StartCoroutine(DestroyTemporaryMessageAfterTime(1));
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

    private IEnumerator DestroyTemporaryMessageAfterTime(float amountOfTime)
    {
        yield return new WaitForSeconds(amountOfTime);
        if (whichPopup == "weaknessInHand" || whichPopup == "notEnoughEnergy")
        {
            DestroyAllPopups();
        }
    }
}
