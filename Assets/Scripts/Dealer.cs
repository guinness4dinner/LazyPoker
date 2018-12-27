using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour {

       
    public enum dealStates {
        Setup,
        Start,
        Dealt,
        Flop,
        Turn,
        River,
    };

    public dealStates currentGameState;

    [SerializeField] Hand[] playerHands;
    [SerializeField] Deck mainDeck;
    [SerializeField] Hand communityCards;
    [SerializeField] PokerGameManager gameManager;

    Player[] players;

    public int numberOfPlayers;

    private void Start()
    {
        gameManager = FindObjectOfType<PokerGameManager>();
    }

    public void SetupGame()
    {
        players = gameManager.players;
        playerHands = new Hand[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            playerHands[i] = players[i].GetComponent<Hand>();
            players[i].GetComponent<ShowCards>().RpcSetup();            
        }

        communityCards.GetComponent<ShowCards>().RpcCommunitySetup();

        numberOfPlayers = playerHands.Length;
        currentGameState = dealStates.Start;
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
        currentGameState = dealStates.Start;
    }

    public void Deal()
    {
        if (currentGameState == dealStates.Start)
        {
            DealToEachPlayer(2);
            currentGameState = dealStates.Dealt;
        }
        else if (currentGameState == dealStates.Dealt)
        {
            DealCards(3, mainDeck, communityCards);
            currentGameState = dealStates.Flop;
        }
        else if (currentGameState == dealStates.Flop)
        {
            DealCards(1, mainDeck, communityCards);
            currentGameState = dealStates.Turn;
        }
        else if (currentGameState == dealStates.Turn)
        {
            DealCards(1, mainDeck, communityCards);
            currentGameState = dealStates.River;
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
