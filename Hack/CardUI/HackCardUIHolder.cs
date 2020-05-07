using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackCardUIHolder : MonoBehaviour
{
    [SerializeField] HackCard parentCard;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -100);
    }

    public void TurnOffCardUIHolder()
    {
        gameObject.SetActive(false);
    }
}
