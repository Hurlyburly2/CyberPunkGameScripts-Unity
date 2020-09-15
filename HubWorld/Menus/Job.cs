using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : ScriptableObject
{
    string jobName;
    public enum JobType { GetItem, Assassination };
    public enum JobArea { Slums, HomeBase };

    public void GenerateJob(int playerLevel)
    {

    }
}
