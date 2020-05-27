using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackGridSquare : MonoBehaviour
{
    [SerializeField] int squareNumber;
    [SerializeField] Sprite[] imageOptions;
    [SerializeField] GameObject hackholder;

    GridRow parentRow;
    GridRowHolder gridRowHolder;
    bool active = false;
    bool safe = false;
    bool wasSafeWhenPlacementAttempted;
    bool isPlacementAllowed = false;

    HackGridSquare leftSquare = null;
    HackGridSquare aboveSquare = null;
    HackGridSquare rightSquare = null;
    HackGridSquare belowSquare = null;

    HackGridSquare aboveLeftDiagonalSquare = null;
    HackGridSquare aboveRightDiagonalSquare = null;
    HackGridSquare belowLeftDiagonalSquare = null;
    HackGridSquare belowRightDiagonalSquare = null;

    HackCard attachedHackCard;
    Spike emptySpike;
    SpriteRenderer safeSquareIndicatorBlock;
    Color defaultColor;

    private void Start()
    {
        safeSquareIndicatorBlock = GetComponent<SpriteRenderer>();
        defaultColor = safeSquareIndicatorBlock.color;
        GameObject parentRowObject = transform.parent.gameObject;
        parentRow = parentRowObject.GetComponent<GridRow>();
        gridRowHolder = FindObjectOfType<GridRowHolder>();
    }

    private void FindAndStoreAdjacentSquares()
    {
        // Get adjacent squares
        if (squareNumber != 0)
            leftSquare = parentRow.GetSquareByNumber(squareNumber - 1);
        if (parentRow.GetRowNumber() != 0)
            aboveSquare = gridRowHolder.GetRowByNumber(parentRow.GetRowNumber() - 1).GetSquareByNumber(squareNumber);
        if (squareNumber != 12)
            rightSquare = parentRow.GetSquareByNumber(squareNumber + 1);
        if (parentRow.GetRowNumber() != 16)
            belowSquare = gridRowHolder.GetRowByNumber(parentRow.GetRowNumber() + 1).GetSquareByNumber(squareNumber);

        if (squareNumber != 0 && parentRow.GetRowNumber() != 0)
            aboveLeftDiagonalSquare = gridRowHolder.GetRowByNumber(parentRow.GetRowNumber() - 1).GetSquareByNumber(squareNumber - 1);
        if (squareNumber != 12 && parentRow.GetRowNumber() != 0)
            aboveRightDiagonalSquare = gridRowHolder.GetRowByNumber(parentRow.GetRowNumber() - 1).GetSquareByNumber(squareNumber + 1);
        if (squareNumber != 0 && parentRow.GetRowNumber() != 16)
            belowLeftDiagonalSquare = gridRowHolder.GetRowByNumber(parentRow.GetRowNumber() + 1).GetSquareByNumber(squareNumber - 1);
        if (squareNumber != 12 && parentRow.GetRowNumber() != 16)
            belowRightDiagonalSquare = gridRowHolder.GetRowByNumber(parentRow.GetRowNumber() + 1).GetSquareByNumber(squareNumber + 1);
    }

    private void OnMouseOver()
    {
        active = true;
    }

    public void RemoveCardFromSquare()
    {
        HackBattleData hackBattleData = FindObjectOfType<HackBattleData>();
        if (attachedHackCard)
        {
            attachedHackCard.DestroyInstance();
            if (!wasSafeWhenPlacementAttempted)
            {
                safe = wasSafeWhenPlacementAttempted;
                hackBattleData.LowerSecurityLevel();
            }
        }
    }

    public void RotateButtonPressed(int timesToRotate, string directionToRotate)
    {
        if (directionToRotate == "left")
        {
            timesToRotate = 4 - timesToRotate;
        }
        for (int i = 0; i < timesToRotate; i++)
        {
            RotateCardNinetyDegrees(attachedHackCard);
            attachedHackCard.RotateCircuitsAndSpikesNinetyDegrees();
        }
        attachedHackCard.SetupUI(GetCountToPreviousLegalRotation(attachedHackCard, 1), GetCountToNextLegalRotation(attachedHackCard, 1));
    }

    public bool AttachCardToSquare(HackCard hackCard, string[] tempCircuitConnections)
    {
        int timesToRotate = GetCountToNextLegalRotation(hackCard);
        if (timesToRotate != -1)
        {
            HackCard newHackCard = Instantiate(hackCard, new Vector2(hackholder.transform.position.x, hackholder.transform.position.y), Quaternion.identity);

            newHackCard.SetModifiedCircuit(tempCircuitConnections);

            newHackCard.transform.SetParent(hackholder.transform);
            newHackCard.transform.localScale = new Vector3(1, 1, 1);
            newHackCard.SetGridSquareHolder(this);

            for (int i = 0; i < timesToRotate; i++)
            {
                RotateCardNinetyDegrees(newHackCard);
                newHackCard.RotateCircuitsAndSpikesNinetyDegrees();
            }

            HackBattleData hackBattleData = FindObjectOfType<HackBattleData>();
            hackBattleData.SetStateToCardUI();

            newHackCard.SetupUI(GetCountToPreviousLegalRotation(newHackCard, 1), GetCountToNextLegalRotation(newHackCard, 1));
            attachedHackCard = newHackCard;
            UpdateSecurityRating();

            return true;
        } else
        {
            return false;
        }
    }

    private void TurnOffSquareImage()
    {
        safeSquareIndicatorBlock.enabled = false;
    }

    private void TurnOnSquareImage()
    {
        safeSquareIndicatorBlock.enabled = true;
    }

    private int GetCountToPreviousLegalRotation(HackCard hackcard, int startingRotation = 0)
    {
        FindAndStoreAdjacentSquares();

        for (int rotations = startingRotation; rotations < 4; rotations++)
        {
            string rotatedLeftCircuit = hackcard.GetLeftCircuit();
            string rotatedTopCircuit = hackcard.GetTopCircuit();
            string rotatedRightCircuit = hackcard.GetRightCircuit();
            string rotatedBottomCircuit = hackcard.GetBottomCircuit();
            for (int i = 0; i < rotations; i++)
            {
                string previousLeftCircuit = rotatedLeftCircuit;
                rotatedLeftCircuit = rotatedTopCircuit;
                rotatedTopCircuit = rotatedRightCircuit;
                rotatedRightCircuit = rotatedBottomCircuit;
                rotatedBottomCircuit = previousLeftCircuit;
            }
            bool isCurrentRotationLegal = CheckRotation(rotatedLeftCircuit, rotatedTopCircuit, rotatedRightCircuit, rotatedBottomCircuit);
            if (isCurrentRotationLegal)
            {
                return rotations;
            }
        }
        return -1;
    }

    private int GetCountToNextLegalRotation(HackCard hackcard, int startingRotation = 0)
    {
        FindAndStoreAdjacentSquares();

        // We check all four rotations for a match and allow placing the square if there are any
        // If there are, we return the number of rotations necessary for later processing
        // If there are none, we return -1
        // Starting rotation is set to 1 if we want to find the next legal rotation and skip 'no rotation'
        for (int rotations = startingRotation; rotations < 4; rotations++) {
            string rotatedLeftCircuit = hackcard.GetLeftCircuit();
            string rotatedTopCircuit = hackcard.GetTopCircuit();
            string rotatedRightCircuit = hackcard.GetRightCircuit();
            string rotatedBottomCircuit = hackcard.GetBottomCircuit();
            for (int i = 0; i < rotations; i++)
            {
                string previousLeftCircuit = rotatedLeftCircuit;
                rotatedLeftCircuit = rotatedBottomCircuit;
                rotatedBottomCircuit = rotatedRightCircuit;
                rotatedRightCircuit = rotatedTopCircuit;
                rotatedTopCircuit = previousLeftCircuit;
            }
            bool isCurrentRotationLegal = CheckRotation(rotatedLeftCircuit, rotatedTopCircuit, rotatedRightCircuit, rotatedBottomCircuit);
            if (isCurrentRotationLegal)
            {
                return rotations;
            }
        }
        return -1;
    }

    private string IsConnectionOk(string currentLeftCircuit, string leftCardRightCircuit)
    {
        if (currentLeftCircuit == leftCardRightCircuit)
        {
            return "connected";
        }
        return "mismatch";
    }

    private void RotateCardNinetyDegrees(HackCard cardToRotate)
    {
        // Variables must be shifted one to the right (eg left => top, top => right, right => bottom, bottom => left)
        // this must be done for circuits AND spikes
        cardToRotate.transform.Find("Card").gameObject.transform.Rotate(0, 0, 270);
        //cardToRotate.transform.Rotate(0, 0, 270);
    }

    private bool CheckRotation(string rotatedLeftCircuit, string rotatedTopCircuit, string rotatedRightCircuit, string rotatedBottomCircuit)
    {
        string leftConnectionCheck;
        if (leftSquare && leftSquare.DoesSquareHaveCard())
        {
            leftConnectionCheck = IsConnectionOk(rotatedLeftCircuit, leftSquare.GetAttachedCard().GetRightCircuit());
        }
        else
        {
            leftConnectionCheck = "ok";
        }
        // Left Connection Check

        string aboveConnectionCheck;
        if (aboveSquare && aboveSquare.DoesSquareHaveCard())
        {
            aboveConnectionCheck = IsConnectionOk(rotatedTopCircuit, aboveSquare.GetAttachedCard().GetBottomCircuit());
        }
        else
        {
            aboveConnectionCheck = "ok";
        }
        // Above Connection Check

        string rightConnectionCheck;
        if (rightSquare && rightSquare.DoesSquareHaveCard())
        {
            rightConnectionCheck = IsConnectionOk(rotatedRightCircuit, rightSquare.GetAttachedCard().GetLeftCircuit());
        }
        else
        {
            rightConnectionCheck = "ok";
        }
        // Right Connection Check

        string belowConnectionCheck;
        if (belowSquare && belowSquare.DoesSquareHaveCard())
        {
            belowConnectionCheck = IsConnectionOk(rotatedBottomCircuit, belowSquare.GetAttachedCard().GetTopCircuit());
        }
        else
        {
            belowConnectionCheck = "ok";
        }
        // Below Conection Check

        if (leftConnectionCheck == "mismatch" || aboveConnectionCheck == "mismatch" || rightConnectionCheck == "mismatch" || belowConnectionCheck == "mismatch")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public HackCard GetAttachedCard()
    {
        return GetComponentInChildren<HackCard>();
    }

    public bool DoesSquareHaveCard()
    {
        HackCard attachedCard = GetComponentInChildren<HackCard>();
        if (attachedCard)
        {
            return true;
        }
        return false;
    }

    private void OnMouseExit()
    {
        active = false;
    }

    public bool IsActive()
    {
        return active;
    }

    public void LogId()
    {
        active = false;
    }

    public string SquareGridLocation()
    {
        return "x: " + squareNumber + " y: " + parentRow.GetRowNumber().ToString();
    }

    public void UpdateSpikeConnections()
    {
        FindAndStoreAdjacentSquares();

        if (attachedHackCard.GetTopLeftSpike().GetSpikeColor() != "none")
            CheckAndUpdateTopLeftSpike();
        if (attachedHackCard.GetTopRightSpike().GetSpikeColor() != "none")
            CheckAndUpdateTopRightSpike();
        if (attachedHackCard.GetBottomLeftSpike().GetSpikeColor() != "none")
            CheckAndUpdateBottomLeftSpike();
        if (attachedHackCard.GetbottomRightSpike().GetSpikeColor() != "none")
            CheckAndUpdateBottomRightSpike();
    }

    private void CheckAndUpdateBottomRightSpike()
    {
        Spike currentSpike = attachedHackCard.GetbottomRightSpike();
        Spike rightSpike = rightSquare.GetBottomLeftSpike();
        Spike belowRightSpike = belowRightDiagonalSquare.GetTopLeftSpike();
        Spike belowSpike = belowSquare.GetTopRightSpike();

        string color = currentSpike.GetSpikeColor();
        if (rightSpike && belowSpike && belowRightSpike && color == rightSpike.GetSpikeColor() && color == belowSpike.GetSpikeColor() && color == belowRightSpike.GetSpikeColor())
        {
            // ALL FOUR CONNECTED
            currentSpike.SetState("two");
            belowSpike.SetState("two");
            rightSpike.SetState("two");
            belowRightSpike.SetState("two");
            UpdatePoints(color, 4);
        }
        else if (rightSpike && belowRightSpike && color == rightSpike.GetSpikeColor() && color == belowRightSpike.GetSpikeColor())
        {
            // RIGHT, DIAGONAL
            currentSpike.SetState("right");
            rightSpike.SetState("two");
            belowRightSpike.SetState("up");
            UpdatePoints(color, 3);
        }
        else if (belowSpike && belowRightSpike && color == belowSpike.GetSpikeColor() && color == belowRightSpike.GetSpikeColor())
        {
            // DOWN DIAGONAL
            currentSpike.SetState("down");
            belowSpike.SetState("two");
            belowRightSpike.SetState("left");
            UpdatePoints(color, 3);
        }
        else if (rightSpike && belowSpike && color == rightSpike.GetSpikeColor() && color == belowSpike.GetSpikeColor())
        {
            // RIGHT DOWN
            currentSpike.SetState("two");
            rightSpike.SetState("left");
            belowSpike.SetState("up");
            UpdatePoints(color, 3);
        }
        else if (rightSpike && color == rightSpike.GetSpikeColor())
        {
            // RIGHT
            currentSpike.SetState("right");
            rightSpike.SetState("left");
            UpdatePoints(color, 2);
        }
        else if (belowSpike && color == belowSpike.GetSpikeColor())
        {
            // DOWN
            currentSpike.SetState("down");
            belowSpike.SetState("up");
            UpdatePoints(color, 2);
        }
        else
        {
            // NONE
            currentSpike.SetState("closed");
        }

        currentSpike.SetSpikeImage("bottomright");
        if (belowSpike)
            belowSpike.SetSpikeImage("topright");
        if (belowRightSpike)
            belowRightSpike.SetSpikeImage("topleft");
        if (rightSpike)
            rightSpike.SetSpikeImage("bottomleft");
    }

    private void CheckAndUpdateBottomLeftSpike()
    {
        Spike currentSpike = attachedHackCard.GetBottomLeftSpike();
        Spike leftSpike = leftSquare.GetBottomRightSpike();
        Spike belowLeftSpike = belowLeftDiagonalSquare.GetTopRightSpike();
        Spike belowSpike = belowSquare.GetTopLeftSpike();

        string color = currentSpike.GetSpikeColor();
        if (leftSpike && belowSpike && belowLeftSpike && color == leftSpike.GetSpikeColor() && color == belowSpike.GetSpikeColor() && color == belowLeftSpike.GetSpikeColor())
        {
            // ALL FOUR CONNECTED
            currentSpike.SetState("two");
            belowSpike.SetState("two");
            leftSpike.SetState("two");
            belowLeftSpike.SetState("two");
            UpdatePoints(color, 4);
        }
        else if (leftSpike && belowLeftSpike && color == leftSpike.GetSpikeColor() && color == belowLeftSpike.GetSpikeColor())
        {
            // LEFT, DIAGONAL
            currentSpike.SetState("left");
            leftSpike.SetState("two");
            belowLeftSpike.SetState("up");
            UpdatePoints(color, 3);
        }
        else if (belowSpike && belowLeftSpike && color == belowSpike.GetSpikeColor() && color == belowLeftSpike.GetSpikeColor())
        {
            // DOWN DIAGONAL
            currentSpike.SetState("down");
            belowSpike.SetState("two");
            belowLeftSpike.SetState("right");
            UpdatePoints(color, 3);
        }
        else if (leftSpike && belowSpike && color == leftSpike.GetSpikeColor() && color == belowSpike.GetSpikeColor())
        {
            // LEFT DOWN
            currentSpike.SetState("two");
            leftSpike.SetState("right");
            belowSpike.SetState("up");
            UpdatePoints(color, 3);
        }
        else if (leftSpike && color == leftSpike.GetSpikeColor())
        {
            // LEFT
            currentSpike.SetState("left");
            leftSpike.SetState("right");
            UpdatePoints(color, 2);
        }
        else if (belowSpike && color == belowSpike.GetSpikeColor())
        {
            // UP
            currentSpike.SetState("down");
            belowSpike.SetState("up");
            UpdatePoints(color, 2);
        }
        else
        {
            // NONE
            currentSpike.SetState("closed");
        }

        currentSpike.SetSpikeImage("bottomleft");
        if (belowSpike)
            belowSpike.SetSpikeImage("topleft");
        if (belowLeftSpike)
            belowLeftSpike.SetSpikeImage("topright");
        if (leftSpike)
            leftSpike.SetSpikeImage("bottomright");
    }

    private void CheckAndUpdateTopRightSpike()
    {
        Spike currentSpike = attachedHackCard.GetTopRightSpike();
        Spike rightSpike = rightSquare.GetTopLeftSpike();
        Spike aboveRightSpike = aboveRightDiagonalSquare.GetBottomLeftSpike();
        Spike aboveSpike = aboveSquare.GetBottomRightSpike();

        string color = currentSpike.GetSpikeColor();
        if (rightSpike && aboveSpike && aboveRightSpike && color == rightSpike.GetSpikeColor() && color == aboveSpike.GetSpikeColor() && color == aboveRightSpike.GetSpikeColor())
        {
            // ALL FOUR CONNECTED
            currentSpike.SetState("two");
            aboveSpike.SetState("two");
            rightSpike.SetState("two");
            aboveRightSpike.SetState("two");
            UpdatePoints(color, 4);
        }
        else if (rightSpike && aboveRightSpike && color == rightSpike.GetSpikeColor() && color == aboveRightSpike.GetSpikeColor())
        {
            // RIGHT, DIAGONAL
            currentSpike.SetState("right");
            rightSpike.SetState("two");
            aboveRightSpike.SetState("down");
            UpdatePoints(color, 3);
        }
        else if (aboveSpike && aboveRightSpike && color == aboveSpike.GetSpikeColor() && color == aboveRightSpike.GetSpikeColor())
        {
            // UP DIAGONAL
            currentSpike.SetState("up");
            aboveSpike.SetState("two");
            aboveRightSpike.SetState("left");
            UpdatePoints(color, 3);
        }
        else if (rightSpike && aboveSpike && color == rightSpike.GetSpikeColor() && color == aboveSpike.GetSpikeColor())
        {
            // RIGHT UP
            currentSpike.SetState("two");
            rightSpike.SetState("left");
            aboveSpike.SetState("down");
            UpdatePoints(color, 3);
        }
        else if (rightSpike && color == rightSpike.GetSpikeColor())
        {
            // RIGHT
            currentSpike.SetState("right");
            rightSpike.SetState("left");
            UpdatePoints(color, 2);
        }
        else if (aboveSpike && color == aboveSpike.GetSpikeColor())
        {

            // UP
            currentSpike.SetState("up");
            aboveSpike.SetState("down");
            UpdatePoints(color, 2);
        }
        else
        {
            // NONE
            currentSpike.SetState("closed");
        }

        currentSpike.SetSpikeImage("topright");
        if (aboveSpike)
            aboveSpike.SetSpikeImage("bottomright");
        if (aboveRightSpike)
            aboveRightSpike.SetSpikeImage("bottomleft");
        if (rightSpike)
            rightSpike.SetSpikeImage("topleft");
    }

    private void CheckAndUpdateTopLeftSpike()
    {
        Spike currentSpike = attachedHackCard.GetTopLeftSpike();
        Spike leftSpike = leftSquare.GetTopRightSpike();
        Spike aboveLeftSpike = aboveLeftDiagonalSquare.GetBottomRightSpike();
        Spike aboveSpike = aboveSquare.GetBottomLeftSpike();

        string color = currentSpike.GetSpikeColor();
        if (leftSpike && aboveSpike && aboveLeftSpike && color == leftSpike.GetSpikeColor() && color == aboveSpike.GetSpikeColor() && color == aboveLeftSpike.GetSpikeColor())
        {
            // ALL FOUR CONNECTED
            currentSpike.SetState("two");
            aboveSpike.SetState("two");
            leftSpike.SetState("two");
            aboveLeftSpike.SetState("two");
            UpdatePoints(color, 4);
        }
        else if (leftSpike && aboveLeftSpike && color == leftSpike.GetSpikeColor() && color == aboveLeftSpike.GetSpikeColor())
        {
            // LEFT, DIAGONAL
            currentSpike.SetState("left");
            leftSpike.SetState("two");
            aboveLeftSpike.SetState("down");
            UpdatePoints(color, 3);
        }
        else if (aboveSpike && aboveLeftSpike && color == aboveSpike.GetSpikeColor() && color == aboveLeftSpike.GetSpikeColor())
        {
            // UP DIAGONAL
            currentSpike.SetState("up");
            aboveSpike.SetState("two");
            aboveLeftSpike.SetState("right");
            UpdatePoints(color, 3);
        }
        else if (leftSpike && aboveSpike && color == leftSpike.GetSpikeColor() && color == aboveSpike.GetSpikeColor())
        {
            // LEFT UP
            currentSpike.SetState("two");
            leftSpike.SetState("right");
            aboveSpike.SetState("down");
            UpdatePoints(color, 3);
        }
        else if (leftSpike && color == leftSpike.GetSpikeColor())
        {
            // LEFT
            currentSpike.SetState("left");
            leftSpike.SetState("right");
            UpdatePoints(color, 2);
        }
        else if (aboveSpike && color == aboveSpike.GetSpikeColor())
        {
            // UP
            currentSpike.SetState("up");
            aboveSpike.SetState("down");
            UpdatePoints(color, 2);
        }
        else
        {
            // NONE
            currentSpike.SetState("closed");
        }

        currentSpike.SetSpikeImage("topleft");
        if (aboveSpike)
            aboveSpike.SetSpikeImage("bottomleft");
        if (aboveLeftSpike)
            aboveLeftSpike.SetSpikeImage("bottomright");
        if (leftSpike)
            leftSpike.SetSpikeImage("topright");
    }

    private void UpdatePoints(string color, int connections)
    {
        HackBattleData hackBattleData = FindObjectOfType<HackBattleData>();
        switch(connections)
        {
            case 2:
                hackBattleData.UpdatePointValue(color, 1);
                break;
            case 3:
                hackBattleData.UpdatePointValue(color, 3);
                break;
            case 4:
                hackBattleData.UpdatePointValue(color, 9);
                break;
        }
    }

    public Spike GetTopLeftSpike()
    {
        if (attachedHackCard)
            return attachedHackCard.GetTopLeftSpike();
        return emptySpike;
    }

    public Spike GetTopRightSpike()
    {
        if (attachedHackCard)
            return attachedHackCard.GetTopRightSpike();
        return emptySpike;
    }

    public Spike GetBottomLeftSpike()
    {
        if (attachedHackCard)
            return attachedHackCard.GetBottomLeftSpike();
        return emptySpike;
    }

    public Spike GetBottomRightSpike()
    {
        if (attachedHackCard)
            return attachedHackCard.GetbottomRightSpike();
        return emptySpike;
    }

    public int GetParentRowNumber()
    {
        return parentRow.GetRowNumber();
    }

    public int GetSquareNumber()
    {
        return squareNumber;
    }

    public void SetSafe(bool isSafe)
    {
        safe = isSafe;
        if (safe)
            TurnOffSquareImage();
        else
            TurnOnSquareImage();
    }

    public void SetLegality(bool legality)
    {
        isPlacementAllowed = legality;
        if (isPlacementAllowed)
            safeSquareIndicatorBlock.color = defaultColor;
        else
            safeSquareIndicatorBlock.color = new Color(0.5f, 0.0f, 0.5f, 1);
    }

    public bool IsSafe()
    {
        return safe;
    }

    private void UpdateSecurityRating()
    {
        wasSafeWhenPlacementAttempted = safe;
        TurnOffSquareImage();
        if (!IsSafe())
        {
            FindObjectOfType<HackBattleData>().RaiseSecurityLevel();
        }
        safe = true;
    }

    public bool IsPlacementAllowed()
    {
        return isPlacementAllowed;
    }
}
