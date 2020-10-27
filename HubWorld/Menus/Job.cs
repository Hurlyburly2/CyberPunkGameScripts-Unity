using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : ScriptableObject
{
    public enum JobType { GetItem, Assassination };
    public enum JobArea { Slums, HomeBase };
    public enum EnemyType { Gang };

    string jobName;
    string jobDescription;
    JobType jobType;
    JobArea jobArea;
    List<EnemyType> enemyTypes;
    int jobDifficulty; // 1-5 stars

    string jobIntroText = ""; // Displayed at beginning of mission
    string jobMiddleTextOne = ""; // Displayed pre-battle, used for assassination missions where the boss is special
    string jobMiddleTextTwo = ""; // Displayed post-battle
    string jobEndText = ""; // Displayed upon job completion

    Item.ItemTypes rewardItem;
    int rewardMoney;
    int mapSize;

    bool isStoryMission;

    public void GenerateJob(int playerLevel)
    {
        enemyTypes = new List<EnemyType>();
        isStoryMission = false;

        jobArea = GenerateJobArea(playerLevel);
        jobType = GenerateJobType(playerLevel);
        GenerateJobNameAndDescription();
        jobDifficulty = GenerateJobDifficulty(playerLevel);
        mapSize = GenerateMapSize();

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
                        enemyTypes.Add(EnemyType.Gang);

                        jobIntroText = "You've arrived in %gangName%' territory. Kill their leader and get out quick, before they're onto you.";
                        jobMiddleTextOne = "After making your way through %gangName% streets, you finally come across their leader, %personName%. Time to take them down.";
                        jobMiddleTextTwo = "%personName% is dead. Your client will no doubt be thrilled to hear the news. Proceed to extraction.";
                        jobEndText = "You know the power vacuum left by %personName%'s death will be quickly filled, but for now the chaos you've left behind you indicates a job well done. Time to get paid.";
                        break;
                    case JobType.GetItem:
                        string[] slumGetItemJobNames =
                        {
                            "Steal Black Market %modName%",
                            "Obtain %drugName% shipment"
                        };
                        string[] slumGetItemJobDescriptions =
                        {
                            "The %gangName% have gotten their hands on a valuable %modName%. Take it off their hands.",
                            "An incoming shipment of %drugName% will flood the market and cut into our profits. Destroy it so we remain the only source."
                        };
                        index = Random.Range(0, slumGetItemJobNames.Length);
                        jobName = slumGetItemJobNames[index];
                        jobDescription = slumGetItemJobDescriptions[index];
                        enemyTypes.Add(EnemyType.Gang);

                        switch (index)
                        {
                            case 0: // steal black marked mod
                                jobIntroText = "Your contact has said the %gangName% are keeping the %modName% mod in this area. They're attempting to reverse engineer it, steal it back before their clumsy attempts ruin it for good.";
                                jobMiddleTextOne = ""; // LEFT BLANK
                                jobMiddleTextTwo = "The %gangName% are on high alert as you locate the %modName% mod deep in their territory. Get it to extraction before they're able to muster enough forces to take it back.";
                                jobEndText = "You don't know if your client wants to study it, copy it, or destroy it, but that doesn't matter. You've delivered the black market mod and earned your payment.";
                                break;
                            case 1: // obtain drug shipment
                                jobIntroText = "The shipment of %drugName% will be passing through this area. Intercept and destroy it.";
                                jobMiddleTextOne = ""; // LEFT BLANK
                                jobMiddleTextTwo = "You've found the shipment and after some quick work the pallettes of product are set ablaze. The %gangName% won't be cutting into your client's business anytime soon. Get to extraction with the news.";  
                                jobEndText = "Your client is satisfied with the work you've done. After deliverying payment they hint that you may be able to earn a place in their organization. You know it's just a way to get more out of you for less, and tactfully refuse.";
                                break;
                        }
                        break;
                }
                break;
        }
        FillInTheBlanks();
    }

    private int GenerateMapSize()
    {
        int minimumSquares = 20;
        int maximumSquares = 25;
        switch (jobDifficulty)
        {
            case 1:
                // defaults already set above
                break;
            case 2:
                minimumSquares = 20;
                maximumSquares = 30;
                break;
            case 3:
                minimumSquares = 25;
                maximumSquares = 35;
                break;
            case 4:
                minimumSquares = 30;
                maximumSquares = 40;
                break;
            case 5:
                minimumSquares = 40;
                maximumSquares = 50;
                break;
        }
        return Random.Range(minimumSquares, maximumSquares);
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
        int maxDifficulty = playerLevel + 1;
        //if (playerLevel > 0)
        //    maxDifficulty++;

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
        // Process the job strings to fill in the blanks like mad libs!
        string[] jobTextFields = { jobName, jobDescription, jobIntroText, jobMiddleTextOne, jobMiddleTextTwo, jobEndText };

        for (int i = 0; i < jobTextFields.Length; i++)
        {
            string personName = GenerateName();
            jobTextFields[i] = jobTextFields[i].Replace("%personName%", personName);
            string gangName = GenerateGangName();
            jobTextFields[i] = jobTextFields[i].Replace("%gangName%", gangName);
            string drugName = GenerateDrugName();
            jobTextFields[i] = jobTextFields[i].Replace("%drugName%", drugName);
            string modName = GenerateModName();
            jobTextFields[i] = jobTextFields[i].Replace("%modName%", modName);
        }

        jobName = jobTextFields[0];
        jobDescription = jobTextFields[1];
        jobIntroText = jobTextFields[2];
        jobMiddleTextOne = jobTextFields[3];
        jobMiddleTextTwo = jobTextFields[4];
        jobEndText = jobTextFields[5];
    }

    public Sprite GetJobIcon()
    {
        Sprite icon = Resources.Load<Sprite>("Icons/JobIcons/" + jobType.ToString());
        return icon;
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
        return "Clowns";
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

    public int GetMapSize()
    {
        return mapSize;
    }

    public List<EnemyType> GetEnemyTypes()
    {
        return enemyTypes;
    }

    public bool GetIsStoryMission()
    {
        return isStoryMission;
    }

    public string GetJobIntroText()
    {
        return jobIntroText;
    }

    public string GetJobMiddleTextOne()
    {
        return jobMiddleTextOne;
    }

    public string GetJobMiddleTextTwo()
    {
        return jobMiddleTextTwo;
    }

    public string GetJobEndText()
    {
        return jobEndText;
    }
}
