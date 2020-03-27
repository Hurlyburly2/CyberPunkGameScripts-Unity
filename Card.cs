using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    ConfigData configData;
    BattleData battleData;

    float xPos;
    float yPos;
    float zPos;
    float rotation;
    float handAdjustSpeed = 20f;

    int rememberSortingOrder = 0;
    float rememberRotation = 0;

    // where is the card, and how is it behaving?
        // draw = when card is initially drawn
        // hand = drawn card reaches hand
        // dragging = mouse down, can move it from hand
        // played = card dragged to play zone (above a certain y axis)
    string state;

    // Start is called before the first frame update
    void Awake()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
        configData = FindObjectOfType<ConfigData>();
        battleData = FindObjectOfType<BattleData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "draw" || state == "hand")
        {
            MoveTowardTarget(xPos, yPos);
            AdjustRotation();
            DoesDrawnCardReachHand();
        } else if (state == "dragging")
        {
            float mouseX = Input.mousePosition.x / Screen.width * configData.GetHalfWidth() * 2 / configData.GetCardSizeMultiplier();
            float mouseY = Input.mousePosition.y / Screen.height * configData.GetHalfHeight() * 2;

            MoveTowardTarget(mouseX, mouseY);
            AdjustRotation();
            DoesDrawnCardReachHand();
        }
    }

    private void OnMouseDown()
    {
        if (!battleData.AreActionsDisabled())
        {
            SetState("dragging");
        }
    }

    private void OnMouseUp()
    {
        if (state == "dragging")
        {
            rotation = rememberRotation;
            SetSortingOrder(rememberSortingOrder);
            float mouseY = Input.mousePosition.y / Screen.height * configData.GetHalfHeight() * 2;
            if (mouseY > configData.GetCardPlayedLine())
            {
                SetState("played");
                FindObjectOfType<PlayerHand>().RemoveCard(GetComponent<Card>());
                Destroy(gameObject);
            }
            else
            {
                SetState("draw");
            }
        }
    }

    private void DoesDrawnCardReachHand()
    {
        if (transform.position == new Vector3(xPos, yPos, transform.position.z))
        {
            SetState("hand");
        }
    }

    private void MoveTowardTarget(float targetX, float targetY)
    {
        float step = handAdjustSpeed * Time.deltaTime;
        Vector3 newPosition = new Vector3(targetX, targetY, zPos);

        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
    }

    private void AdjustRotation()
    {
        Quaternion target = Quaternion.Euler(0f, 0f, rotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * handAdjustSpeed);
    }

    public void SetSortingOrder(int newSortingOrder)
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        rememberSortingOrder = spriteRenderers[0].sortingOrder;
        int currentSortingOrder = newSortingOrder;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingOrder = currentSortingOrder;
            currentSortingOrder++;
        }
    }

    public void SetState(string state)
    {
        this.state = state;
        if (this.state == "draw")
        {
            handAdjustSpeed = configData.GetHandDrawSpeed();
        } else if (state == "hand")
        {
            handAdjustSpeed = configData.GetHandAdjustSpeed();
        } else if (state == "dragging")
        {
            rememberRotation = rotation;
            rotation = 0;
            SetSortingOrder(1000);
            handAdjustSpeed = 1000f;
        }
    }

    public void SetCardPosition(float xPos)
    {
        this.xPos = xPos;
    }

    public void SetCardPosition(float xPos, float yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
    }

    public void SetCardPosition(float xPos, float yPos, float rotation)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.rotation = rotation;
    }

    public float GetXPos()
    {
        return xPos;
    }

    public void SetZPos(float zPos)
    {
        this.zPos = zPos;
    }
}
