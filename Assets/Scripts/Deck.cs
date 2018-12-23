using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

    [SerializeField] List<Card> deckCards;
    [SerializeField] List<Card> currentDeckCards = new List<Card>();

    // Use this for initialization
    void Start()
    {
        ResetDeck();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCard(Card card)
    {

    }

    public void RemoveCard(Card card)
    {

    }

    public Card GetTopCard()
    {
        Card card = currentDeckCards[0];
        currentDeckCards.RemoveAt(0);
        return card;
    }

    public void GetSpecificCard(Card card)
    {

    }

    public void ShuffleDeck()
    {
        List<Card> shuffledCards = new List<Card>();
        int totalCards = currentDeckCards.Count;

        for (int i = 0; i < totalCards; i++)
        {
            int randomCardIndex = Random.Range(0, totalCards - i);
            shuffledCards.Add(currentDeckCards[randomCardIndex]);
            currentDeckCards.RemoveAt(randomCardIndex);
        }

        currentDeckCards = shuffledCards;
    }

    public void ResetDeck()
    {
        currentDeckCards = new List<Card>();
        currentDeckCards.AddRange(deckCards);
        ShuffleDeck();
    }
}
