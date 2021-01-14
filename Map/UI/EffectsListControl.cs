using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsListControl : MonoBehaviour
{
    [SerializeField] EffectsListItem effectsListItemTemplate;
    List<EffectsListItem> itemsInList = new List<EffectsListItem>();

    public void GenerateEffectsList(MapSquare square)
    {
        ClearList();

        List<string> effects = square.GetEffectsListStrings();

        foreach (string effect in effects)
        {
            EffectsListItem effectsListItem = Instantiate(effectsListItemTemplate);
            effectsListItem.gameObject.SetActive(true);
            effectsListItem.SetupItem(effect);

            effectsListItem.transform.SetParent(effectsListItemTemplate.transform.parent, false);
            itemsInList.Add(effectsListItem);
        }
    }

    public void ClearList()
    {
        foreach (EffectsListItem effectsListItem in itemsInList)
        {
            Destroy(effectsListItem.gameObject);
        }
        itemsInList = new List<EffectsListItem>();
    }
}
