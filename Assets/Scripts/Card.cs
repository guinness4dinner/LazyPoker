using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public enum Suit
    {
        none,
        Club,
        Diamond,
        Heart,
        Spades
    }

    public enum Rank
    {
        none,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten,
        J,
        Q,
        K,
        A
    }

    [SerializeField] Suit suit;
    [SerializeField] Rank rank;
    [SerializeField] int cardNumber;

    public Card.Suit GetSuit()
    {
        return suit;
    }

    public Card.Rank GetRank()
    {
        return rank;
    }

    public int GetCardNumber()
    {
        return cardNumber;
    }

}
