using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpListControl : MonoBehaviour
{
    [SerializeField] PowerUpListItem powerUpListItemTemplate;

    List<PowerUpListItem> itemsInList = new List<PowerUpListItem>();

    public void GenerateList(List<PowerUp> powerUps)
    {
        foreach (PowerUp powerUp in powerUps)
        {
            PowerUpListItem powerUpItem = Instantiate(powerUpListItemTemplate);
            powerUpItem.gameObject.SetActive(true);
            powerUpItem.Setup(powerUp);

            powerUpItem.transform.SetParent(powerUpListItemTemplate.transform.parent, false);
            itemsInList.Add(powerUpItem);
        }
    }

    public void ClearList()
    {
        foreach (PowerUpListItem listItem in itemsInList)
        {
            Destroy(listItem.gameObject);
        }
        itemsInList = new List<PowerUpListItem>();
    }
}
