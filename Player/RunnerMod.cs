using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerMod : Item
{
    List<int> levelOneCards = new List<int>();
    List<int> levelTwoCards = new List<int>();
    List<int> levelThreeCards = new List<int>();
    List<int> levelFourCards = new List<int>();
    List<int> levelFiveCards = new List<int>();

    public bool SetupMod(string newModName)
    {
        bool success = GetMod(newModName);
        itemLevel = 1;
        itemMaxLevel = 5;
        hackerOrRunner = HackerRunner.Runner;

        return success;
    }

    private bool GetMod(string newModName)
    {
        itemName = newModName;
        List<int> cards = SetVariablesAndGetCards(itemName);
        if (cards.Count == 0)
            return false;
        else
            return true;
    }

    public List<int> GetCardIds()
    {
        switch (itemLevel)
        {
            case 1:
                return levelOneCards;
            case 2:
                return levelTwoCards;
            case 3:
                return levelThreeCards;
            case 4:
                return levelFourCards;
            case 5:
                return levelFiveCards;
            default:
                return levelOneCards;
        }
    }

    private List<int> SetVariablesAndGetCards(string modName)
    {
        switch (modName)
        {
            case "Human Eyes":
                itemType = ItemTypes.Head;
                itemDescription = "Unmodified human eyes.";
                levelOneCards.Add(1); // Awareness 1
                levelOneCards.Add(2); // Observe

                levelTwoCards.Add(1); // Awareness 1
                levelTwoCards.Add(22); // Observe 2

                levelThreeCards.Add(20); // Awareness 2
                levelThreeCards.Add(23); // Observe 3

                levelFourCards.Add(20); // Awareness 2
                levelFourCards.Add(24); // Observe

                levelFiveCards.Add(21); // Awareness 3
                levelFiveCards.Add(25); // Observe
                break;
            case "Unmodded Torso":
                itemType = ItemTypes.Torso;
                itemDescription = "Unmodified human torso.";
                levelOneCards.Add(3); // Deep Breath
                levelOneCards.Add(4); // Weak Spot

                levelTwoCards.Add(26); // Deep Breath 2
                levelTwoCards.Add(29); // Weak Spot

                levelThreeCards.Add(27); // Deep Breath 3
                levelThreeCards.Add(30); // Weak Spot

                levelFourCards.Add(27); // Deep Breath 3
                levelFourCards.Add(6); // Brace 1

                levelFiveCards.Add(28); // Deep Breath 4
                levelFiveCards.Add(6); // Brace 1
                break;
            case "Human Skin":
                itemType = ItemTypes.Exoskeleton;
                itemDescription = "Unmodified human skin.";
                levelOneCards.Add(5); // Shake Off
                levelOneCards.Add(6); // Brace

                levelTwoCards.Add(31); // Shake Off 2
                levelTwoCards.Add(6); // Brace

                levelThreeCards.Add(32); // Shake Off
                levelThreeCards.Add(35); // Brace 2

                levelFourCards.Add(33); // Shake Off
                levelFourCards.Add(36); // Brace 3

                levelFiveCards.Add(34); // Shake Off
                levelFiveCards.Add(37); // Brace 4
                break;
            case "Unmodded Arm":
                itemType = ItemTypes.Arm;
                itemDescription = "Unmodified human arm.";
                levelOneCards.Add(7); // Punch
                levelOneCards.Add(7); // Punch
                levelOneCards.Add(8); // Quickdraw

                levelTwoCards.Add(38); // Punch 2
                levelTwoCards.Add(38); // Punch 2
                levelTwoCards.Add(8); // Quickdraw

                levelThreeCards.Add(39); // Punch 3
                levelThreeCards.Add(39); // Punch 3
                levelThreeCards.Add(42); // Quickdraw 2

                levelFourCards.Add(40); // Punch 4
                levelFourCards.Add(40); // Punch 4
                levelFourCards.Add(43); // Quickdraw 3

                levelFiveCards.Add(41); // Punch 5
                levelFiveCards.Add(41); // Punch 5
                levelFiveCards.Add(44); // Quickdraw 4
                break;
            case "Unmodded Leg":
                itemType = ItemTypes.Leg;
                itemDescription = "Unmodified human leg.";
                levelOneCards.Add(9); // Kick
                levelOneCards.Add(9); // Kick
                levelOneCards.Add(10);    // Sprint

                levelTwoCards.Add(45); // Kick 2
                levelTwoCards.Add(45); // Kick 2
                levelTwoCards.Add(49);    // Sprint 2

                levelThreeCards.Add(46); // Kick 3
                levelThreeCards.Add(46); // Kick 3
                levelThreeCards.Add(50);    // Sprint 3

                levelFourCards.Add(47); // Kick 4
                levelFourCards.Add(47); // Kick 4
                levelFourCards.Add(50);    // Sprint 3

                levelFiveCards.Add(48); // Kick 5
                levelFiveCards.Add(48); // Kick 5
                levelFiveCards.Add(51);    // Sprint 4
                break;
            case "Spanner":
                itemType = ItemTypes.Weapon;
                itemDescription = "Heavy wrench. It'll do in a pinch, until you find something better.";
                levelOneCards.Add(11);    // Whack
                levelOneCards.Add(11);    // Whack
                levelOneCards.Add(12);    // Kneecap
                levelOneCards.Add(13);    // Bruise

                levelTwoCards.Add(52);    // Whack 2
                levelTwoCards.Add(52);    // Whack 2
                levelTwoCards.Add(56);    // Kneecap 2
                levelTwoCards.Add(60);    // Bruise 2

                levelThreeCards.Add(53);    // Whack 3
                levelThreeCards.Add(53);    // Whack 3
                levelThreeCards.Add(57);    // Kneecap 3
                levelThreeCards.Add(61);    // Bruise 3

                levelFourCards.Add(54);    // Whack 4
                levelFourCards.Add(54);    // Whack 4
                levelFourCards.Add(58);    // Kneecap
                levelFourCards.Add(62);    // Bruise 4

                levelFiveCards.Add(55);    // Whack 5
                levelFiveCards.Add(55);    // Whack 5
                levelFiveCards.Add(59);    // Kneecap
                levelFiveCards.Add(63);    // Bruise 5
                break;
            case "Adaptable CranioPatch":
                itemType = ItemTypes.Head;
                itemDescription = "The headpiece of Vance CryptoTronix's popular Pointman line of Pistolier augmentations. For when you really need to make your shots count.";
                levelOneCards.Add(73);  // Quick Targetting 1
                levelOneCards.Add(73);  // Quick Targetting 1
                levelOneCards.Add(78); // Pinpoint Accuracy 1
                levelOneCards.Add(83); // Radar Ghost 1

                levelTwoCards.Add(74);  // Quick Targetting 2
                levelTwoCards.Add(74);  // Quick Targetting 2
                levelTwoCards.Add(79); // Pinpoint Accuracy 2
                levelTwoCards.Add(84); // Radar Ghost 2

                levelThreeCards.Add(75);  // Quick Targetting 3
                levelThreeCards.Add(75);  // Quick Targetting 3
                levelThreeCards.Add(80); // Pinpoint Accuracy 3
                levelThreeCards.Add(85); // Radar Ghost 3

                levelFourCards.Add(76);  // Quick Targetting 4
                levelFourCards.Add(76);  // Quick Targetting 4
                levelFourCards.Add(81); // Pinpoint Accuracy 4
                levelFourCards.Add(86); // Radar Ghost 4

                levelFiveCards.Add(77);  // Quick Targetting 5
                levelFiveCards.Add(77);  // Quick Targetting 5
                levelFiveCards.Add(82); // Pinpoint Accuracy 5
                break;
            case "Adrenal Injector":
                itemType = ItemTypes.Torso;
                itemDescription = "One of the more experiemental reflex enhancers designed by Vance CryptoTronix. Signing a waiver is required before installation, as cardiac side-effects are not uncommon.";

                levelOneCards.Add(87); // Stim Injection 1
                levelOneCards.Add(92); // Raise Heartrate 1
                levelOneCards.Add(92); // Raise Heartrate 1
                levelOneCards.Add(97); // Cardiac Arrest 1

                levelTwoCards.Add(88); // Stim Injection 2
                levelTwoCards.Add(93); // Raise Heartrate 2
                levelTwoCards.Add(93); // Raise Heartrate 2
                levelTwoCards.Add(98); // Cardiac Arrest 2

                levelThreeCards.Add(89); // Stim Injection 3
                levelThreeCards.Add(94); // Raise Heartrate 3
                levelThreeCards.Add(94); // Raise Heartrate 3
                levelThreeCards.Add(99); // Cardiac Arrest 3

                levelFourCards.Add(90); // Stim Injection 4
                levelFourCards.Add(95); // Raise Heartrate 4
                levelFourCards.Add(95); // Raise Heartrate 4
                levelFourCards.Add(100); // Cardiac Arrest 4

                levelFiveCards.Add(91); // Stim Injection 5
                levelFiveCards.Add(89); // Stim Injection 3
                levelFiveCards.Add(96); // Raise Heartrate 5
                levelFiveCards.Add(96); // Raise Heartrate 5
                break;
            case "Sensory Regulator":
                itemType = ItemTypes.Exoskeleton;
                itemDescription = "Enhance the best parts of life, and depress the worst, with Vance CyptoTronix' popular, lightweight nerve graft.";

                levelOneCards.Add(101); // Deaden Senses 1
                levelOneCards.Add(106); // HEIGHTENED RECEPTORS 1
                levelOneCards.Add(111); // SENSORY OVERLOAD 1

                levelTwoCards.Add(102); // Deaden Senses 2
                levelTwoCards.Add(107); // HEIGHTENED RECEPTORS 2
                levelTwoCards.Add(112); // SENSORY OVERLOAD 2

                levelThreeCards.Add(103); // Deaden Senses 3
                levelThreeCards.Add(108); // HEIGHTENED RECEPTORS 3
                levelThreeCards.Add(108); // HEIGHTENED RECEPTORS 3
                levelThreeCards.Add(113); // SENSORY OVERLOAD 3

                levelFourCards.Add(104); // Deaden Senses 4
                levelFourCards.Add(109); // HEIGHTENED RECEPTORS 4
                levelFourCards.Add(109); // HEIGHTENED RECEPTORS 4
                levelFourCards.Add(114); // SENSORY OVERLOAD 4

                levelFiveCards.Add(105); // Deaden Senses 5
                levelFiveCards.Add(110); // HEIGHTENED RECEPTORS 5
                levelFiveCards.Add(110); // HEIGHTENED RECEPTORS 5
                levelFiveCards.Add(115); // ZEN CONTROL 1
                break;
            case "Automated Digits":
                itemType = ItemTypes.Arm;
                itemDescription = "Premiere Vance CyptoTronix hand replacement. Stronger, and more dexterous than any organic hand, with a hidden defensive blade.";

                levelOneCards.Add(116); // LIGHTNING RELOAD 1
                levelOneCards.Add(121); // AUTO-UNHOLSTER 1
                levelOneCards.Add(126); // IMPLANTED QUIKBLADE 1
                levelOneCards.Add(131); // JAMMED SERVOS 1

                levelTwoCards.Add(117); // LIGHTNING RELOAD 2
                levelTwoCards.Add(122); // AUTO-UNHOLSTER 2
                levelTwoCards.Add(127); // IMPLANTED QUIKBLADE 2
                levelTwoCards.Add(132); // JAMMED SERVOS 2

                levelThreeCards.Add(118); // LIGHTNING RELOAD 3
                levelThreeCards.Add(123); // AUTO-UNHOLSTER 3
                levelThreeCards.Add(128); // IMPLANTED QUIKBLADE 3
                levelThreeCards.Add(133); // JAMMED SERVOS 3

                levelFourCards.Add(119); // LIGHTNING RELOAD 4
                levelFourCards.Add(124); // AUTO-UNHOLSTER 4
                levelFourCards.Add(129); // IMPLANTED QUIKBLADE 4
                levelFourCards.Add(134); // JAMMED SERVOS 4

                levelFiveCards.Add(120); // LIGHTNING RELOAD 5
                levelFiveCards.Add(125); // AUTO-UNHOLSTER 5
                levelFiveCards.Add(130); // IMPLANTED QUIKBLADE 5
                break;
            case "Polymorphic Support":
                itemType = ItemTypes.Leg;
                itemDescription = "TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO";

                levelOneCards.Add(135); // STABILIZED STANCE 1
                levelOneCards.Add(140); // PREPARED STANCE 1
                levelOneCards.Add(145); // NIMBLE STANCE 1
                levelOneCards.Add(150); // READY FOR ANYTHING 1
                levelOneCards.Add(155); // MISSTEP 1

                levelTwoCards.Add(136); // STABILIZED STANCE 1
                levelTwoCards.Add(141); // PREPARED STANCE 2
                levelTwoCards.Add(146); // NIMBLE STANCE 2
                levelTwoCards.Add(151); // READY FOR ANYTHING 2
                levelTwoCards.Add(156); // MISSTEP 2

                levelThreeCards.Add(137); // STABILIZED STANCE 1
                levelThreeCards.Add(142); // PREPARED STANCE 3
                levelThreeCards.Add(147); // NIMBLE STANCE 3
                levelThreeCards.Add(152); // READY FOR ANYTHING 3
                levelThreeCards.Add(157); // MISSTEP 3

                levelFourCards.Add(138); // STABILIZED STANCE 1
                levelFourCards.Add(143); // PREPARED STANCE 4
                levelFourCards.Add(148); // NIMBLE STANCE 4
                levelFourCards.Add(153); // READY FOR ANYTHING 4
                levelFourCards.Add(158); // MISSTEP 4

                levelFiveCards.Add(139); // STABILIZED STANCE 1
                levelFiveCards.Add(144); // PREPARED STANCE 5
                levelFiveCards.Add(149); // NIMBLE STANCE 5
                levelFiveCards.Add(154); // READY FOR ANYTHING 5
                break;
            case "Tornado Handgun T-492":
                itemType = ItemTypes.Weapon;
                itemDescription = "TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO";

                levelOneCards.Add(159); // BARRAGE 1
                levelOneCards.Add(164); // SHOOT 1
                levelOneCards.Add(164); // SHOOT 1
                levelOneCards.Add(169); // HAIR TRIGGER 1
                levelOneCards.Add(169); // HAIR TRIGGER 1
                levelOneCards.Add(174); // RELOAD 1
                levelOneCards.Add(179); // MISFIRE 1

                levelTwoCards.Add(160); // BARRAGE 1
                levelTwoCards.Add(165); // SHOOT 2
                levelTwoCards.Add(165); // SHOOT 2
                levelTwoCards.Add(170); // HAIR TRIGGER 2
                levelTwoCards.Add(170); // HAIR TRIGGER 2
                levelTwoCards.Add(175); // RELOAD 2
                levelTwoCards.Add(180); // MISFIRE 2

                levelThreeCards.Add(161); // BARRAGE 1
                levelThreeCards.Add(166); // SHOOT 3
                levelThreeCards.Add(166); // SHOOT 3
                levelThreeCards.Add(171); // HAIR TRIGGER 3
                levelThreeCards.Add(171); // HAIR TRIGGER 3
                levelThreeCards.Add(176); // RELOAD 3
                levelThreeCards.Add(181); // MISFIRE 3

                levelFourCards.Add(162); // BARRAGE 1
                levelFourCards.Add(167); // SHOOT 4
                levelFourCards.Add(167); // SHOOT 4
                levelFourCards.Add(172); // HAIR TRIGGER 4
                levelFourCards.Add(172); // HAIR TRIGGER 4
                levelFourCards.Add(177); // RELOAD 4
                levelFourCards.Add(182); // MISFIRE 4

                levelFiveCards.Add(163); // BARRAGE 1
                levelFiveCards.Add(168); // SHOOT 5
                levelFiveCards.Add(168); // SHOOT 5
                levelFiveCards.Add(173); // HAIR TRIGGER 5
                levelFiveCards.Add(173); // HAIR TRIGGER 5
                levelFiveCards.Add(178); // RELOAD 5
                levelFiveCards.Add(183); // MISFIRE 5
                break;
            case "Volt HandCannon V-1":
                itemType = ItemTypes.Weapon;
                itemDescription = "TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO";

                levelOneCards.Add(184); // CHARGED SHOT 1
                levelOneCards.Add(184); // CHARGED SHOT 1
                levelOneCards.Add(189); // VENT HEAT 1
                levelOneCards.Add(194); // DOUBLE TAP 1
                levelOneCards.Add(199); // SHOOT 1
                levelOneCards.Add(199); // SHOOT 1
                levelOneCards.Add(204); // RELOAD 1

                levelTwoCards.Add(185); // CHARGED SHOT 2
                levelTwoCards.Add(185); // CHARGED SHOT 2
                levelTwoCards.Add(190); // VENT HEAT 2
                levelTwoCards.Add(195); // DOUBLE TAP 2
                levelTwoCards.Add(200); // SHOOT 2
                levelTwoCards.Add(200); // SHOOT 2
                levelTwoCards.Add(205); // RELOAD 2

                levelThreeCards.Add(186); // CHARGED SHOT 3
                levelThreeCards.Add(186); // CHARGED SHOT 3
                levelThreeCards.Add(191); // VENT HEAT 3
                levelThreeCards.Add(196); // DOUBLE TAP 3
                levelThreeCards.Add(201); // SHOOT 3
                levelThreeCards.Add(201); // SHOOT 3
                levelThreeCards.Add(206); // RELOAD 3

                levelFourCards.Add(187); // CHARGED SHOT 4
                levelFourCards.Add(187); // CHARGED SHOT 4
                levelFourCards.Add(192); // VENT HEAT 4
                levelFourCards.Add(197); // DOUBLE TAP 4
                levelFourCards.Add(202); // SHOOT 4
                levelFourCards.Add(202); // SHOOT 4
                levelFourCards.Add(207); // RELOAD 4

                levelFiveCards.Add(188); // CHARGED SHOT 5
                levelFiveCards.Add(188); // CHARGED SHOT 5
                levelFiveCards.Add(193); // VENT HEAT 5
                levelFiveCards.Add(198); // DOUBLE TAP 5
                levelFiveCards.Add(203); // SHOOT 5
                levelFiveCards.Add(203); // SHOOT 5
                levelFiveCards.Add(208); // RELOAD 5
                break;
            default:
                // Indicates a failure to set up the runner mod
                List<int> emptyList = new List<int>();
                return emptyList;
        }

        return levelOneCards;
    }

    public List<int> GetLevelOneCardIds()
    {
        return levelOneCards;
    }

    public List<int> GetLevelTwoCardIds()
    {
        return levelTwoCards;
    }

    public List<int> GetLevelThreeCardIds()
    {
        return levelThreeCards;
    }

    public List<int> GetLevelFourCardIds()
    {
        return levelFourCards;
    }

    public List<int> GetLevelFiveCardIds()
    {
        return levelFiveCards;
    }
}
