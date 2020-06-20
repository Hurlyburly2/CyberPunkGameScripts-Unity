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

    public Enemy GetAnEnemyByArea(string area)
    {
        switch (area)
        {
            case "slums":
                List<Enemy> slumEnemies = GetSlumEnemies();
                return slumEnemies[Random.Range(0, slumEnemies.Count)];
        }
        return allEnemies[0];
    }

    private List<Enemy> GetSlumEnemies()
    {
        // indices of all enemies who belong to slums
        int[] slumIndexArray = { 0 };
        List<Enemy> slumEnemies = new List<Enemy>();

        foreach (int index in slumIndexArray)
        {
            slumEnemies.Add(allEnemies[index]);
        }

        return slumEnemies;
    }
}
