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

    public Card.Suit GetSuit()
    {
        return suit;
    }

    public Card.Rank GetRank()
    {
        return rank;
    }

}
