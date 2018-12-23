using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerManager : MonoBehaviour {

       
    public enum gameStates {
        Start,
        Dealt,
        Flop,
        Turn,
        River,
    };

    public gameStates currentGameState;

    [SerializeField] HandManager[] playerHands;
    [SerializeField] DeckManager deck;
    [SerializeField] HandManager communityCards;

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
        }
        communityCards.ResetHand();
        deck.ResetDeck();
        currentGameState = gameStates.Start;
    }

    public void Deal()
    {
        if (currentGameState == gameStates.Start)
        {
            DealToEachPlayer();
            currentGameState = gameStates.Dealt;
        }
        else if (currentGameState == gameStates.Dealt)
        {
            DealFlop();
            currentGameState = gameStates.Flop;
        }
        else if (currentGameState == gameStates.Flop)
        {
            DealTurnOrRiver();
            currentGameState = gameStates.Turn;
        }
        else if (currentGameState == gameStates.Turn)
        {
            DealTurnOrRiver();
            currentGameState = gameStates.River;
        }
    }


    private void DealTurnOrRiver()
    {
        communityCards.AddCard(deck.GetTopCard());
    }

    private void DealFlop()
    {
        for (int i = 0; i < 3; i++)
        {
            communityCards.AddCard(deck.GetTopCard());
        }
    }

    private void DealToEachPlayer()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int p = 0; p < numberOfPlayers; p++)
            {
                playerHands[p].AddCard(deck.GetTopCard());
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
