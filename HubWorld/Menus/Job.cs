using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : ScriptableObject
{
    public enum JobType { GetItem, Assassination };
    public enum JobArea { Slums, HomeBase };

    string jobName;
    string jobDescription;
    JobType jobType;
    JobArea jobArea;
    int jobDifficulty; // 1-5 stars

    Item.ItemTypes rewardItem;
    int rewardMoney;

    public void GenerateJob(int playerLevel)
    {
        jobArea = GenerateJobArea(playerLevel);
        jobType = GenerateJobType(playerLevel);
        GenerateJobNameAndDescription();
        jobDifficulty = GenerateJobDifficulty(playerLevel);

        rewardItem = GetRandomItemType();
        rewardMoney = GetRewardMoneyAmount();
    }

    private void GenerateJobNameAndDescription()
    {
        switch (jobArea)
        {
            case JobArea.Slums:
                switch (jobType)
                {
                    case JobType.Assassination:
                        string[] slumAssassinationJobNames =
                        {
                            "Assassinate %personName%",
                            "Kill Leader of %gangName%"
                        };
                        string[] slumAssassinationJobDescriptions =
                        {
                            "%personName% owes us money. What a dick. Kill him.",
                            "%gangName% have been encroaching on our turf. Kill their boss, %personName% to teach them a lesson."
                        };
                        int index = Random.Range(0, slumAssassinationJobNames.Length);
                        jobName = slumAssassinationJobNames[index];
                        jobDescription = slumAssassinationJobDescriptions[index];
                        break; ;
                    case JobType.GetItem:
                        string[] slumGetItemJobNames =
                        {
                            "Steal Black Market %modName%",
                            "Obtain %drugName% shipment"
                        };
                        string[] slumGetItemJobDescriptions =
                        {
                            "%gangName% have gotten their hands on a valuable %modName%. Take it off their hands.",
                            "An incoming shipment of %drugName% will flood the market and cut into our profits. Destroy it so we remain the only source."
                        };
                        index = Random.Range(0, slumGetItemJobNames.Length);
                        jobName = slumGetItemJobNames[index];
                        jobDescription = slumGetItemJobDescriptions[index];
                        break;
                }
                break;
        }
        FillInTheBlanks();
    }

    private int GetRewardMoneyAmount()
    {
        switch (jobDifficulty)
        {
            case 1:
                return Random.Range(500, 1001);
            case 2:
                return Random.Range(1000, 1501);
            case 3:
                return Random.Range(1500, 3001);
            case 4:
                return Random.Range(3000, 4501);
            case 5:
                return Random.Range(5000, 7001);
            default:
                return 0;
        }
    }

    private Item.ItemTypes GetRandomItemType()
    {
        Item.ItemTypes[] itemTypes =
        {
            Item.ItemTypes.Arm,
            Item.ItemTypes.Chipset,
            Item.ItemTypes.Exoskeleton,
            Item.ItemTypes.Head,
            Item.ItemTypes.Leg,
            Item.ItemTypes.NeuralImplant,
            Item.ItemTypes.Rig,
            Item.ItemTypes.Software,
            Item.ItemTypes.Torso,
            Item.ItemTypes.Uplink,
            Item.ItemTypes.Weapon,
            Item.ItemTypes.Wetware
        };

        return itemTypes[Random.Range(0, itemTypes.Length)];
    }

    private int GenerateJobDifficulty(int playerLevel)
    {
        int maxDifficulty = 0;
        if (playerLevel > 0)
            maxDifficulty++;

        return Random.Range(1, maxDifficulty + 1);
    }

    private JobArea GenerateJobArea(int playerLevel)
    {
        List<JobArea> potentialJobAreas = new List<JobArea>();
        if (playerLevel >= 0)
            potentialJobAreas.Add(JobArea.Slums);

        return potentialJobAreas[Random.Range(0, potentialJobAreas.Count)];
    }

    private JobType GenerateJobType(int playerLevel)
    {
        List<JobType> potentialJobTypes = new List<JobType>();
        if (playerLevel >= 0)
        {
            potentialJobTypes.Add(JobType.GetItem);
            potentialJobTypes.Add(JobType.Assassination); // TODO: GET RID OF THIS AND MOVE IT TO LEVEL ONE AFTER DONE TESTING
        }
            

        return potentialJobTypes[Random.Range(0, potentialJobTypes.Count)];
    }

    private void FillInTheBlanks()
    {
        if (jobName.Contains("%personName%") || jobDescription.Contains("%personName%"))
        {
            string personName = GenerateName();
            jobName.Replace("%personName%", personName);
            jobDescription.Replace("%personName%", personName);
        }
        if (jobName.Contains("%gangName%") || jobDescription.Contains("%gangName%"))
        {
            string gangName = GenerateGangName();
            jobName.Replace("%gangName%", gangName);
            jobDescription.Replace("%gangName%", gangName);
        }
        if (jobName.Contains("%drugName%") || jobDescription.Contains("%drugName%"))
        {
            string drugName = GenerateDrugName();
            jobName.Replace("%drugName%", drugName);
            jobDescription.Replace("%drugName%", drugName);
        }
        if (jobName.Contains("%modName%") || jobDescription.Contains("%modName%"))
        {
            string modName = GenerateModName();
            jobName.Replace("%modName%", modName);
            jobDescription.Replace("%modName%", modName);
        }
    }

    private string GenerateModName()
    {
        // TODO: NAME GENERATION (do this by area)
        return "stimFast";
    }

    private string GenerateDrugName()
    {
        // TODO: NAME GENERATION (do this by area...?)
        return "slowMo";
    }

    private string GenerateName()
    {
        // TODO: NAME GENERATION (do this by area)
        return "Bad Guy";
    }

    private string GenerateGangName()
    {
        // TODO: NAME GENERATION (do this by area)
        return "The Clowns";
    }

    // Getters

    public string GetJobName()
    {
        return jobName;
    }

    public string GetJobDescription()
    {
        return jobDescription;
    }

    public JobType GetJobType()
    {
        return jobType;
    }

    public JobArea GetJobArea()
    {
        return jobArea;
    }

    public int GetJobDifficulty()
    {
        return jobDifficulty;
    }

    public Item.ItemTypes GetRewardItemType()
    {
        return rewardItem;
    }

    public int GetRewardMoney()
    {
        return rewardMoney;
    }
}
