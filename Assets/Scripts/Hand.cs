using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    [SerializeField] List<Card> currentHandCards = new List<Card>();

    // Use this for initialization
    void Start () {
		
	}

    //Specific to LazyPoker
    public void RevealHand()
    {

    }

    public void FoldHand()
    {
        ResetHand();
    }


    //Generic Methods
    public void AddCard(Card card)
    {
        currentHandCards.Add(card);
        UpdateShownCards();
    }

    public Card PlayCard(int index)
    {
        Card card = currentHandCards[index];
        currentHandCards.RemoveAt(index);
        UpdateShownCards();
        return card;
    }

    public void ResetHand()
    {
        currentHandCards = new List<Card>();
        UpdateShownCards();
    }

    private void UpdateShownCards()
    {
        
    }
}
