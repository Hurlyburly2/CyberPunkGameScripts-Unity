using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : ScriptableObject
{
    // LEVEL 0 ITEMS: These items will have special behaviors because they're your defaults
    // Make them unsellable???
    public List<string> lvl0MechItems = new List<string> {
        "Spanner",
    };
    public List<string> lvl0TechItems = new List<string>();
    public List<string> lvl0CyberItems = new List<string> {
        "Basic Rig",
        "Basic Cranial Dock",
        "Basic Uplink",
        "Cheap Ghost",
        "JuryRigged QwikThink",
        "Salvaged Router",
    };
    public List<string> lvl0BioItems = new List<string> {
        "Human Eyes",
        "Unmodded Torso",
        "Human Skin",
        "Unmodded Arm",
        "Unmodded Leg",
    };

    // LEVEL 1 ITEMS:
    public List<string> lvl1MechItems = new List<string>();
    public List<string> lvl1TechItems = new List<string> {
        "Adaptable CranioPatch",
        "Sensory Regulator",
        "Automated Digits",
        "Polymorphic Support",
        "Tornado Handgun T-492",
        "Volt HandCannon V-1"
    };
    public List<string> lvl1CyberItems = new List<string>();
    public List<string> lvl1BioItems = new List<string> {
        "Adrenal Injector",
        "Polymorphic Support",
    };

    public List<string> GetItemListByLevelAndType(int level, ShopMenu.ShopForSaleType shopType)
    {
        List<string> itemResult = new List<string>();
        switch (shopType)
        {
            case ShopMenu.ShopForSaleType.Bio:
                if (level >= 0)
                    itemResult.AddRange(lvl0BioItems);
                if (level >= 1)
                    itemResult.AddRange(lvl1BioItems);
                break;
            case ShopMenu.ShopForSaleType.Cyber:
                if (level >= 0)
                    itemResult.AddRange(lvl0CyberItems);
                if (level >= 1)
                    itemResult.AddRange(lvl1CyberItems);
                break;
            case ShopMenu.ShopForSaleType.Mech:
                if (level >= 0)
                    itemResult.AddRange(lvl0MechItems);
                if (level >= 1)
                    itemResult.AddRange(lvl1MechItems);
                break;
            case ShopMenu.ShopForSaleType.Tech:
                if (level >= 0)
                    itemResult.AddRange(lvl0TechItems);
                if (level >= 1)
                    itemResult.AddRange(lvl1TechItems);
                break;
        }

        return itemResult;
    }
}
