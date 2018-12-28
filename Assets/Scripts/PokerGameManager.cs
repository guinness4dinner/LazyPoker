using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] int currentPlayersTurn = 1;
    [SerializeField] int currentDealerPlayer = 0;
    [SerializeField] int currentBet;
    [SerializeField] bool roundActive = false;
    [SerializeField] bool bettingRoundActive = false;

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

        //Choose first dealer and Player.

        currentGameState = gameStates.Start;
        roundActive = true;
        StartCoroutine(RunRound());
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

    private IEnumerator RunRound()
    {
        while(roundActive)
        {
            NextGameState();
            dealer.Deal();
            MakeAllUnfoldedPlayersUncalled();
            yield return StartCoroutine(BettingRound());
        }
    }

    private void MakeAllUnfoldedPlayersUncalled()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (players[i].currentPlayerState != Player.playerState.Folded)
            {
                players[i].currentPlayerState = Player.playerState.Uncalled;
            }
            players[i].SetRaisedValue(0);
        }
    }

    private void NextGameState()
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
                NewRound();
                break;
            default:
                break;
        }
    }

    private void NewRound()
    {
        //Set Dealer & Current Player
        dealer.ResetRound();
    }

    private IEnumerator BettingRound()
    {
        bettingRoundActive = true;
        switch (currentGameState)
        {
            case gameStates.Preflop:
                PlaceBet(smallBlindValue, players[currentPlayersTurn]);
                NextPlayersTurn();
                PlaceBet(bigBlindValue, players[currentPlayersTurn]);
                players[currentPlayersTurn].currentPlayerState = Player.playerState.Called;
                currentBet = bigBlindValue;                
                break;
            case gameStates.Flop:
                currentPlayersTurn = currentDealerPlayer;
                NextPlayersTurn();
                yield return StartCoroutine(AskForCheckCallOrRaise(currentBet, minBet, players[currentPlayersTurn]));               
                break;
            case gameStates.Turn:
                currentPlayersTurn = currentDealerPlayer;
                NextPlayersTurn();
                yield return StartCoroutine(AskForCheckCallOrRaise(currentBet, minBet, players[currentPlayersTurn]));
                break;
            case gameStates.River:
                currentPlayersTurn = currentDealerPlayer;
                NextPlayersTurn();
                yield return StartCoroutine(AskForCheckCallOrRaise(currentBet, minBet, players[currentPlayersTurn]));
                break;
            case gameStates.Showdown:
                currentPlayersTurn = currentDealerPlayer;
                NextPlayersTurn();
                yield return StartCoroutine(AskForCheckCallOrRaise(currentBet, minBet, players[currentPlayersTurn]));
                break;
            default:
                break;
        }

        while (areAnyPlayersNotCalledOrFolded())
        {
            NextPlayersTurn();
            yield return StartCoroutine(AskForCheckCallOrRaise(currentBet, minBet, players[currentPlayersTurn]));
        }

        if (IsOnlyOneCalledPlayerLeft())
        {
            //End Round
        }
        currentBet = 0;
        bettingRoundActive = false;
    }

    private bool IsOnlyOneCalledPlayerLeft()
    {
        return false;
    }

    private bool areAnyPlayersNotCalledOrFolded()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (players[i].currentPlayerState == Player.playerState.Uncalled)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator AskForCheckCallOrRaise(int curBet, int minBet, Player player)
    {
        player.RpcActivateGUIButtons();
        //player.BetOtherButton.SetActive(true);
        int raisedValue = player.GetRaisedValue();
        int callValue = curBet - raisedValue;

        if (callValue > 0 )
        {           

            player.RpcSetButtonText(0, "Call " + callValue.ToString());
            player.RpcSetButtonText(1, "Raise " + minBet.ToString());
            //player.BetOtherButton.GetComponent<Text>().text = "Raise Other";
            player.RpcActivateFoldButton();
        }
        else
        {
            player.RpcSetButtonText(0, "Check");
            player.RpcSetButtonText(1, "Bet " + minBet.ToString());
            //player.BetOtherButton.GetComponent<Text>().text = "Bet Other";
        }
        Debug.Log("Waiting for Action from :" + player.netId.ToString());
        yield return StartCoroutine(WaitForAction(player));

        switch (player.action)
        {
            case "CheckOrCall":
                if (currentBet > 0)
                {
                    
                    //Show Called X Text
                    PlaceBet(callValue, player);
                    player.currentPlayerState = Player.playerState.Called;
                }
                else
                {
                    //Show Checked Text
                    player.currentPlayerState = Player.playerState.Called;
                }
                    break;
            case "Bet Min":
                //Show Raise X Text
                PlaceBet(callValue + minBet, player);
                currentBet += minBet;
                MakeAllCalledPlayersUncalled();
                player.currentPlayerState = Player.playerState.Called;
                break;
            case "Fold":
                player.currentPlayerState = Player.playerState.Folded;
                //Show Folded Text
                //Hide Their Cards
                break;
        }

        player.RpcDeactivateGUIButtons();
    }

    public IEnumerator WaitForAction(Player player)
    {
        player.RpcSetActionNull(); // clear last action, we want a new one
        while (player.action == null) { yield return null; }
    }

    private void MakeAllCalledPlayersUncalled()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (players[i].currentPlayerState == Player.playerState.Called)
            {
                players[i].currentPlayerState = Player.playerState.Uncalled;
            }
        }
    }

    private void PlaceBet(int value, Player player)
    {
        int pocketValue = player.GetPocketValue();
        int raisedValue = player.GetRaisedValue();
        if (pocketValue >= value)
        {
            player.SetPocketValue(pocketValue - value);
            player.SetRaisedValue(raisedValue + value );
            potValue += value;
        }
        else
        {
            player.SetPocketValue(0);
            player.SetRaisedValue(raisedValue + pocketValue);
            potValue += pocketValue;
        }
        //Update Pot Value Text
    }

    private void NextPlayersTurn()
    {
        currentPlayersTurn++;
        if (currentPlayersTurn > numberOfPlayers - 1)
        {
            currentPlayersTurn = 0;
        }
        while (players[currentPlayersTurn].currentPlayerState == Player.playerState.Folded)
        {
            currentPlayersTurn++;
            if (currentPlayersTurn > numberOfPlayers - 1)
            {
                currentPlayersTurn = 0;
            }
        }
        //Update Current Player Turn Text
    }
}
