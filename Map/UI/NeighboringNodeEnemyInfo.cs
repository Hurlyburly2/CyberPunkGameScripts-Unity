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
    [SerializeField] GameObject scoutLevel1;
    [SerializeField] GameObject scoutLevel2;
    [SerializeField] GameObject scoutLevel3;
    [SerializeField] GameObject noEnemyPresent;
    int starRating;

    public void SetupEnemyInfo(Enemy newEnemy, MapSquare square, int enemyScoutLevel)
    {
        enemy = newEnemy;
        Vector3 dummyPosition = new Vector3(-100, -100, -100);

        // we have to do this so we can get data from the prefab...
        Enemy dummyEnemy = Instantiate(enemy, dummyPosition, Quaternion.identity);

        switch (enemyScoutLevel)
        {
            case 1:
                noEnemyPresent.SetActive(false);
                scoutLevel2.SetActive(false);
                scoutLevel3.SetActive(false);
                break;
            case 2:
                noEnemyPresent.SetActive(false);
                scoutLevel2.SetActive(true);
                scoutLevel3.SetActive(false);
                break;
            case 3:
                noEnemyPresent.SetActive(false);
                scoutLevel2.SetActive(false);
                scoutLevel3.SetActive(true);
                EnableEnemyFieldsLevel3();
                portraitImage.sprite = dummyEnemy.GetThumbnailImage();
                enemyName.text = dummyEnemy.GetEnemyName();
                starRating = dummyEnemy.GetStarRating();


                Destroy(dummyEnemy);
                SetupStars();
                break;
        }

        SetupBuffsAndDebuffs(square);
    }

    public void SetupEmptyEnemy(MapSquare square)
    {
        noEnemyPresent.SetActive(true);
        scoutLevel2.SetActive(false);
        scoutLevel3.SetActive(false);
        DisableFields();

        SetupBuffsAndDebuffs(square);
    }

    private void EnableEnemyFieldsLevel3()
    {
        portraitImage.enabled = true;
        portraitBorderFront.enabled = true;
        portraitBorderBack.enabled = true;
        enemyName.enabled = true;
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
