using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeighboringNodeEnemyInfo : MonoBehaviour
{
    Enemy enemy;
    [SerializeField] Image portraitImage;

    public void SetupEnemyInfo(Enemy newEnemy)
    {
        enemy = newEnemy;
        Vector3 dummyPosition = new Vector3(-100, -100, -100);

        // we have to do this so we can get data from the prefab...
        Enemy dummyEnemy = Instantiate(enemy, dummyPosition, Quaternion.identity);
        portraitImage.sprite = dummyEnemy.GetThumbnailImage();
        Destroy(dummyEnemy);
    }
}
