using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] string enemyName;

    public void SetEnemyData()
    {

    }

    public void BattleSetup(float setupTimeInSeconds)
    {

    }

    public int GetEnemyId()
    {
        return id;
    }

    public string GetEnemyName()
    {
        return enemyName;
    }
}
