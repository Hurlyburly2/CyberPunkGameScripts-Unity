using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NeighboringNodeEnemyInfo : MonoBehaviour
{
    Enemy enemy;
    [SerializeField] Image portraitImage;
    [SerializeField] Image portraitBorderFront;
    [SerializeField] Image portraitBorderBack;
    [SerializeField] TextMeshProUGUI enemyName;
    [SerializeField] Image[] stars;
    int starRating;

    public void SetupEnemyInfo(Enemy newEnemy)
    {
        enemy = newEnemy;
        Vector3 dummyPosition = new Vector3(-100, -100, -100);

        // we have to do this so we can get data from the prefab...
        Enemy dummyEnemy = Instantiate(enemy, dummyPosition, Quaternion.identity);

        EnableFields();
        portraitImage.sprite = dummyEnemy.GetThumbnailImage();
        enemyName.text = dummyEnemy.GetEnemyName();
        starRating = dummyEnemy.GetStarRating();
        

        Destroy(dummyEnemy);
        SetupStars();
    }

    public void SetupEmptyEnemy()
    {
        DisableFields();
    }

    private void EnableFields()
    {
        portraitImage.enabled = true;
        portraitBorderFront.enabled = true;
        portraitBorderBack.enabled = true;
        enemyName.enabled = true;
    }

    private void DisableFields()
    {
        portraitImage.enabled = false;
        portraitBorderFront.enabled = false;
        portraitBorderBack.enabled = false;
        enemyName.enabled = false;
        foreach (Image star in stars)
        {
            star.enabled = false;
        }
    }

    private void SetupStars()
    {
        int counter = 1;
        foreach (Image star in stars)
        {
            star.enabled = true;
            if (counter <= starRating)
            {
                star.color = new Color(1, 1, 1, 1);
            }
            else
            {
                star.color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
            counter++;
        }
    }
}
