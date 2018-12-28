using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour {

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
    }

    public void Deal()
    {
        switch (gameManager.currentGameState)
        {
            case PokerGameManager.gameStates.Preflop:
                DealToEachPlayer(2);
                break;
            case PokerGameManager.gameStates.Flop:
                DealCards(3, mainDeck, communityCards);
                break;
            case PokerGameManager.gameStates.Turn:
                DealCards(1, mainDeck, communityCards);
                break;
            case PokerGameManager.gameStates.River:
                DealCards(1, mainDeck, communityCards);
                foreach (Hand el in playerHands)
                {
                    el.GetComponent<PokerHandChecker>().CheckPokerHand();
                }
                break;
            default:
                break;
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
