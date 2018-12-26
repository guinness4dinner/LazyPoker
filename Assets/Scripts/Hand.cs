using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    [SerializeField] List<Card> currentHandCards = new List<Card>();
    [SerializeField] Transform[] cardPositions;

    //Specific to LazyPoker
    public void RevealHand()
    {

    }

    public void FoldHand()
    {
        ResetHand();
    }


    //Generic Methods
    public List<Card> GetCardList()
    { 
        return currentHandCards;
    }
    
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
        foreach (Card el in GetComponentsInChildren<Card>())
        {
            Destroy(el.gameObject);
        }

        if (currentHandCards.Count > 0)
        {
            for (int i = 0; i < currentHandCards.Count; i++)
            {
                Instantiate(currentHandCards[i], cardPositions[i].position, cardPositions[i].rotation, GetComponent<Transform>());
            }
        }    
            
    }
}
