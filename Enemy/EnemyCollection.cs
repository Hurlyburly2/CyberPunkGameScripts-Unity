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

    public Enemy GetAnEnemyByArea(Job.JobArea area, int securityLevel, Job job)
    {
        switch (area)
        {
            case Job.JobArea.Slums:
                return GetEnemy(area, securityLevel, job);
        }
        return allEnemies[0];
    }

    private Enemy GetEnemy(Job.JobArea area, int securityLevel, Job job)
    {
        List<int> enemyIds = new List<int>();
        switch (area)
        {
            case Job.JobArea.Slums:
                int[] slumEnemyIds = { 0 };
                break;
        }

        //List<Job.EnemyType> jobEnemyTypes = job.GetEnemyTypes();
        //List<Enemy> enemyPossibilities = new List<Enemy>();
        //Debug.Log("jobenemytypes count: " + jobEnemyTypes.Count);
        //foreach (int id in enemyIds)
        //{
        //    //Enemy newEnemy = Resources.Load<Enemy>("Enemies/Enemy" + id.ToString());
        //    Enemy newEnemy = Resources.Load<Enemy>("Enemies/Enemy0");
        //    Debug.Log("enemy name: " + newEnemy.GetEnemyName());
        //    foreach (Job.EnemyType type in jobEnemyTypes)
        //    {
        //        if (newEnemy.GetEnemyTypes().Contains(type))
        //        {
        //            Debug.Log("does this happen");
        //            enemyPossibilities.Add(newEnemy);
        //            break;
        //        }
        //    }
        //}

        //Debug.Log("enemypossibilities count: " + enemyPossibilities.Count);
        //return enemyPossibilities[Random.Range(0, enemyPossibilities.Count - 1)];
        return Resources.Load<Enemy>("Enemies/Enemy0");
    }
}
