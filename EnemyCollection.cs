using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollection : MonoBehaviour
{
    [SerializeField] Enemy[] allEnemies;

    public Enemy[] GetEnemyArray()
    {
        return allEnemies;
    }
}
