using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobSelectSquare : MonoBehaviour
{
    [SerializeField] JobSelectMenu parentMenu;
    [SerializeField] Image holder;
    [SerializeField] Sprite holderInactive;
    [SerializeField] Sprite holderActive;
    [SerializeField] Image[] stars;
    [SerializeField] Image jobIcon;

    Job job;
    bool isSelected;

    public void SetupJobSelectSquare(Job newJob)
    {
        isSelected = false;
        job = newJob;

        SetupStars();
        UpdateHolderImage();
        jobIcon.sprite = job.GetJobIcon();
    }

    public Job GetJob()
    {
        return job;
    }

    public void ButtonPress()
    {
        if (!isSelected)
        {
            isSelected = true;
        }
        UpdateHolderImage();
        parentMenu.HandleJobSquareButtonPress(this);
    }

    public void SetInactive()
    {
        isSelected = false;
        UpdateHolderImage();
    }

    private void UpdateHolderImage()
    {
        if (isSelected)
            holder.sprite = holderActive;
        else
            holder.sprite = holderInactive;
    }

    private void SetupStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            Color starColor = stars[i].color;
            if (i < job.GetJobDifficulty())
            {
                starColor.a = 1;
            } else
            {
                starColor.a = 0.5f;
            }
            stars[i].color = starColor;
        }
    }

    public bool GetIsSelected()
    {
        return isSelected;
    }
}
