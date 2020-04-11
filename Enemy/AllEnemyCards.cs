using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemyCards : MonoBehaviour
{
    [SerializeField] List<EnemyCard> allEnemyCards;

    public EnemyCard GetEnemyCardById(int cardId)
    {
        return allEnemyCards[cardId];
    }
}
