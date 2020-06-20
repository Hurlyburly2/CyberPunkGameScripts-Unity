using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // sample save game, not yet used
    public int randomNumber;

    public SaveData(int newRandomNumber)
    {
        randomNumber = newRandomNumber;
    }

    public int GetRandomNumber()
    {
        return randomNumber;
    }
}
