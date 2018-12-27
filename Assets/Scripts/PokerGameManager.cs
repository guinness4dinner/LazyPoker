using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerGameManager : MonoBehaviour {


    public enum gameStates
    {
        Setup,
        Start,
        Preflop,
        Flop,
        Turn,
        River,
        Showdown
    };

    public gameStates currentGameState = gameStates.Setup;

    [SerializeField] int smallBlindValue = 5;
    [SerializeField] int bigBlindValue = 10;
    [SerializeField] int minBet = 10;
    [SerializeField] int startingPocket = 200;
    [SerializeField] Dealer dealer;
    [SerializeField] FindLocalPlayer findLocalPlayer;
    [SerializeField] int[] allPositionOrder = new int[] { 0, 5, 2, 8, 4, 1, 7, 9, 3, 6 };
    public int[] positionOrder;

    public Player[] players;

    int numberOfPlayers;
    int currentPlayersTurn = 1;
    int currentDealerPlayer = 0;
    int currentBet;

    [SerializeField] int potValue = 0;


    // Use this for initialization
    void Start()
    {
        dealer = FindObjectOfType<Dealer>();
    }

    public void SetupGame()
    {
        players = FindObjectsOfType<Player>();
        numberOfPlayers = players.Length;

        SetupPlayerOrder();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i].SetPocketValue(startingPocket);
            players[i].GetComponent<ShowCards>().playerNum = i;
        }
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i].SetPocketValue(startingPocket);
            players[i].GetComponent<ShowCards>().RpcSetLocalPlayer(i);
        }
        findLocalPlayer.RpcSetPosOrder(positionOrder);

        dealer.SetupGame();

        //Place Dealer, SB, BB buttons

        //Choose first deaker and Player.
        currentGameState = gameStates.Start;
        RunGame();
    }

    public void SetupPlayerOrder()
    {
        Player[] newPlayerOrder = new Player[numberOfPlayers];
        positionOrder = new int[numberOfPlayers];
        int posIdx = 0;

        for (int k = 0; k < allPositionOrder.Length; k++)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (allPositionOrder[k] == i)
                {
                    positionOrder[posIdx] = i;
                    posIdx++;
                }
            }
        }

        for (int i = 0; i < numberOfPlayers; i++)
        {
            newPlayerOrder[i] = players[positionOrder[i]];
        }

        players = newPlayerOrder;
    }

    private void RunGame()
    {
       while(true)
       {
            switch (currentGameState)
            {
                case gameStates.Start:
                    currentGameState = gameStates.Preflop;
                    break;
                case gameStates.Preflop:
                    currentGameState = gameStates.Flop;
                    break;
                case gameStates.Flop:
                    currentGameState = gameStates.Turn;
                    break;
                case gameStates.Turn:
                    currentGameState = gameStates.River;
                    break;
                case gameStates.River:
                    currentGameState = gameStates.Showdown;
                    break;
                case gameStates.Showdown:
                    currentGameState = gameStates.Start;
                    break;
                default:
                    break;
            }
            dealer.Deal();
            BettingRound();
        }
       
    }

    private void BettingRound()
    {
        throw new NotImplementedException();
    }

	
}
