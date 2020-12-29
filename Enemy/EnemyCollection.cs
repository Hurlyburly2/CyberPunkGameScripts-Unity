using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollection : MonoBehaviour
{
    public Enemy GetAnEnemyByArea(Job.JobArea area, int securityLevel, Job job, bool isBoss)
    {
        return GetEnemy(area, securityLevel, job, isBoss);
    }

    private Enemy GetEnemy(Job.JobArea area, int securityLevel, Job job, bool isBoss)
    {
        List<int> enemyIds = new List<int>();
        switch (area)
        {
            case Job.JobArea.Slums:
                int[] slumEnemyIds = { 0, 1, 2 };
                enemyIds.AddRange(slumEnemyIds);
                break;
            case Job.JobArea.Downtown:
                //TODO: REMOVE 3 ONCE A BOSS FOR CITY IS CREATED
                int[] downtownEnemyIds = { 3, 2, 4 };
                enemyIds.AddRange(downtownEnemyIds);
                break;
        }

        List<Job.EnemyType> jobEnemyTypes = job.GetEnemyTypes();
        List<Enemy> enemyPossibilities = new List<Enemy>();
        foreach (int id in enemyIds)
        {
            Enemy newEnemy = Resources.Load<Enemy>("Enemies/Enemy" + id.ToString());

            foreach (Job.EnemyType type in jobEnemyTypes)
            {
                if (newEnemy.GetEnemyTypes().Contains(type) && newEnemy.GetIsBoss() == isBoss && newEnemy.GetStarRating() <= job.GetJobDifficulty())
                {
                    enemyPossibilities.Add(newEnemy);
                    break;
                }
            }
        }
        return enemyPossibilities[Random.Range(0, enemyPossibilities.Count)];
    }
}
