using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCards : MonoBehaviour
{
    [SerializeField] Card[] cards;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card GetRandomCard()
    {
        int randomIndex = Mathf.FloorToInt(Random.Range(0, cards.Length - 1));
        return cards[randomIndex];
    }
}
