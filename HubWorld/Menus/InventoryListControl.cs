using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryListControl : MonoBehaviour
{
    [SerializeField] GameObject buttonTemplate;

    private void Start()
    {
        for (int i = 1; i <= 20; i++)
        {
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);

            button.GetComponent<InventoryListItem>().SetText("Button #" + i);

            button.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }
}
