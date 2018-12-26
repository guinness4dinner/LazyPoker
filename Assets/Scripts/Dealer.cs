using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour {

       
    public enum gameStates {
        Start,
        Dealt,
        Flop,
        Turn,
        River,
    };

    public gameStates currentGameState;

    [SerializeField] Hand[] playerHands;
    [SerializeField] Deck mainDeck;
    [SerializeField] Hand communityCards;

    int numberOfPlayers;

	// Use this for initialization
	void Start () {
        numberOfPlayers = playerHands.Length;
        currentGameState = gameStates.Start;
    }

    public void ResetRound()
    {
        for (int p = 0; p < numberOfPlayers; p++)
        {
            playerHands[p].ResetHand();
            playerHands[p].GetComponent<PokerHandChecker>().ResetChecker();
        }
        communityCards.ResetHand();
        mainDeck.ResetDeck();
        currentGameState = gameStates.Start;
    }

    public void Deal()
    {
        if (currentGameState == gameStates.Start)
        {
            DealToEachPlayer(2);
            currentGameState = gameStates.Dealt;
        }
        else if (currentGameState == gameStates.Dealt)
        {
            DealCards(3, mainDeck, communityCards);
            currentGameState = gameStates.Flop;
        }
        else if (currentGameState == gameStates.Flop)
        {
            DealCards(1, mainDeck, communityCards);
            currentGameState = gameStates.Turn;
        }
        else if (currentGameState == gameStates.Turn)
        {
            DealCards(1, mainDeck, communityCards);
            currentGameState = gameStates.River;
            foreach (Hand el in playerHands)
            {
                el.GetComponent<PokerHandChecker>().CheckPokerHand();
            }
        }
    }

    private void DealToEachPlayer(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            for (int p = 0; p < numberOfPlayers; p++)
            {
                DealCards(1, mainDeck, playerHands[p]);
            }
        }
    }

    private void DealCards(int numberOfCards, Deck fromDeck, Hand toHand)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            toHand.AddCard(fromDeck.GetTopCard());
        }
    }

}
