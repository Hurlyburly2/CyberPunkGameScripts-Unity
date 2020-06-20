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
    [SerializeField] Image enemyBuffIcon;
    [SerializeField] Image enemyDebuffIcon;
    int starRating;

    public void SetupEnemyInfo(Enemy newEnemy, MapSquare square)
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
        SetupBuffsAndDebuffs(square);
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
        enemyBuffIcon.enabled = true;
        enemyDebuffIcon.enabled = true;
        foreach (Image star in stars)
        {
            star.enabled = true;
        }
    }

    private void DisableFields()
    {
        portraitImage.enabled = false;
        portraitBorderFront.enabled = false;
        portraitBorderBack.enabled = false;
        enemyName.enabled = false;
        enemyBuffIcon.enabled = false;
        enemyDebuffIcon.enabled = false;
        foreach (Image star in stars)
        {
            star.enabled = false;
        }
    }

    private void SetupBuffsAndDebuffs(MapSquare square)
    {
        if (square.GetEnemyBuffs().Count <= 0)
        {
            enemyBuffIcon.color = new Color(0.5f, 0.5f, 0.5f, 1);
        } else
        {
            enemyBuffIcon.color = new Color(1, 1, 1, 1);
        }

        if (square.GetEnemyDebuffs().Count <= 0)
        {
            enemyDebuffIcon.color = new Color(0.5f, 0.5f, 0.5f, 1);
        } else
        {
            enemyDebuffIcon.color = new Color(1, 1, 1, 1);
        }
    }

    private void SetupStars()
    {
        int counter = 1;
        foreach (Image star in stars)
        {
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
