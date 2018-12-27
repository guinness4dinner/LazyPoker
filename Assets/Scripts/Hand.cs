using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    [SerializeField] List<Card> currentHandCards = new List<Card>();

    ShowCards showCards;


    private void Start()
    {
        showCards = GetComponent<ShowCards>();
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
    public List<Card> GetCardList()
    { 
        return currentHandCards;
    }
    
    public void AddCard(Card card)
    {
        currentHandCards.Add(card);
        showCards.RpcAddCard(card.GetCardNumber());
    }

    public Card PlayCard(int index)
    {
        Card card = currentHandCards[index];
        currentHandCards.RemoveAt(index);
        showCards.RpcRemoveCard(card.GetCardNumber());
        return card;
    }

    public void ResetHand()
    {
        currentHandCards = new List<Card>();
        showCards.RpcReset();
    }
}
