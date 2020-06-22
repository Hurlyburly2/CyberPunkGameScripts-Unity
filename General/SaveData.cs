using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    // sample save game, not yet used
    public int randomNumber;
    public List<int> randomNumbers;

    public SaveData(int newRandomNumber)
    {
        randomNumber = newRandomNumber;
        randomNumbers = new List<int>();
        randomNumbers.Add(Random.Range(0, 100));
        randomNumbers.Add(Random.Range(0, 100));
        foreach(int number in randomNumbers)
        {
            Debug.Log(number);
        }
    }

    public int GetRandomNumber()
    {
        return randomNumber;
    }

    public List<int> GetRandomNumbers()
    {
        return randomNumbers;
    }
}
