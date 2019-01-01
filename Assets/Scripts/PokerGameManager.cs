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
    [SerializeField] ShowCards communityCards;
    [SerializeField] int[] allPositionOrder = new int[] { 0, 5, 2, 8, 4, 1, 7, 9, 3, 6 };
    public int[] positionOrder;

    public Player[] players;

    int numberOfPlayers;
    [SerializeField] int currentPlayersTurn = 1;
    [SerializeField] int currentDealerPlayer = 0;
    [SerializeField] int currentBet;
    [SerializeField] int curMinBet;
    [SerializeField] bool roundActive = false;
    [SerializeField] bool bettingRoundActive = false;

    [SerializeField] int potValue = 0;
    [SerializeField] GameObject NewRoundButton;

    Player loneCaller = null;
    Player lastBetPlayer = null;

    // Use this for initialization
    void Start()
    {
        dealer = FindObjectOfType<Dealer>();
        curMinBet = minBet;
    }

    public void SetupGame()
    {
        players = FindObjectsOfType<Player>();
        numberOfPlayers = players.Length;

        SetupPlayerOrder();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i].GetComponent<PokerHandChecker>().RpcSetup();
            players[i].SetPocketValue(startingPocket);
            players[i].GetComponent<ShowCards>().playerNum = i;
        }
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i].SetPocketValue(startingPocket);
            players[i].GetComponent<ShowCards>().RpcSetLocalPlayer(i);
            players[i].GetComponent<ShowCards>().RpcChangeText(1, "Pocket: " + players[i].GetPocketValue());
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
            if (currentGameState == gameStates.Showdown)
            {
                roundActive = false;
                break;
            }
            dealer.Deal();
            MakeAllUnfoldedPlayersUncalled();
            yield return StartCoroutine(BettingRound());
        }
        ChooseWinner();
        AskForNewRound();
    }

    private void AskForNewRound()
    {
        NewRoundButton.SetActive(true);
        //Turn on Server Only button New Round
    }

    private void ChooseWinner()
    {
        List<Player> winners = new List<Player>();
        Player winner;
        if (currentGameState != gameStates.Showdown)
        {
            winner = loneCaller;
            //Update Text to show winner text.
            winner.GetComponent<ShowCards>().RpcMakeTextActive(3);
            GivePotToPlayer(winner);
        }
        else
        {
            winners = CompareRemainingPlayers(lastBetPlayer);
            //Update Text to show Hand Type and winner text.
            foreach (Player el in winners)
            {
                el.GetComponent<ShowCards>().RpcMakeTextActive(3);
                el.GetComponent<ShowCards>().RpcChangeText(4, "with " + el.GetComponent<PokerHandChecker>().handType.ToString());
                el.GetComponent<ShowCards>().RpcMakeTextActive(4);
            }
            if (winners.Count > 1)
            {
                SplitPot(winners);
            }
            else
            {
                GivePotToPlayer(winners[0]);
            }
        }  
    }

    private void SplitPot(List<Player> winners)
    {
        var splitPotValue = potValue / winners.Count;
        
        foreach (Player el in winners)
        {
            var playerPocket = el.GetPocketValue();
            el.SetPocketValue(playerPocket + splitPotValue);
            el.GetComponent<ShowCards>().RpcChangeText(1, "Pocket: " + el.GetPocketValue());
        }

        potValue = 0;
        communityCards.RpcChangePotValueText( "Pot: " + potValue.ToString());
        
    }

    private List<Player> CompareRemainingPlayers(Player lastBetPlayer)
    {
        List<Player> remainingPlayers = new List<Player>();
        List<Player> highesHandtypePlayers = new List<Player>();
        List<Card.Rank[]> highestRanks = new List<Card.Rank[]>();
        int highestHandType = 0;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (players[i].currentPlayerState == Player.playerState.Called)
            {
                remainingPlayers.Add(players[i]);
            }
        }

        foreach (Player el in remainingPlayers)
        {
            var playerHandType = el.GetComponent<PokerHandChecker>().handType;
            if ((int)playerHandType > highestHandType)
            {
                highestHandType = (int)playerHandType;
            }
        }

        foreach (Player el in remainingPlayers)
        {
            var playerHandType = el.GetComponent<PokerHandChecker>().handType;
            if (playerHandType == (PokerHandChecker.HandType)highestHandType)
            {
                highesHandtypePlayers.Add(el);
                highestRanks.Add(el.GetComponent<PokerHandChecker>().highestRanks);
            }
        }

        if (highesHandtypePlayers.Count > 1)
        {
            var removeIdx0 = CompareHighestRanks(highesHandtypePlayers, highestRanks, 0);
            foreach (int el in removeIdx0)
            {
                highesHandtypePlayers.RemoveAt(el);
                highestRanks.RemoveAt(el);
            }

            if (highesHandtypePlayers.Count > 1)
            {

                var removeIdx1 = CompareHighestRanks(highesHandtypePlayers, highestRanks, 1);
                foreach (int el in removeIdx1)
                {
                    highesHandtypePlayers.RemoveAt(el);
                    highestRanks.RemoveAt(el);
                }
                if (highesHandtypePlayers.Count > 1)
                {
                    var removeIdx2 = CompareHighestRanks(highesHandtypePlayers, highestRanks, 2);
                    foreach (int el in removeIdx2)
                    {
                        highesHandtypePlayers.RemoveAt(el);
                        highestRanks.RemoveAt(el);
                    }
                    if (highesHandtypePlayers.Count > 1)
                    {
                        var removeIdx3 = CompareHighestRanks(highesHandtypePlayers, highestRanks, 3);
                        foreach (int el in removeIdx3)
                        {
                            highesHandtypePlayers.RemoveAt(el);
                            highestRanks.RemoveAt(el);
                        }
                        if (highesHandtypePlayers.Count > 1)
                        {
                            var removeIdx4 = CompareHighestRanks(highesHandtypePlayers, highestRanks, 4);
                            foreach (int el in removeIdx4)
                            {
                                highesHandtypePlayers.RemoveAt(el);
                                highestRanks.RemoveAt(el);
                            }
                        }
                    }
                }
            }
        }

        //if (highesHandtypePlayers.Count == 1 && highesHandtypePlayers[0] == lastBetPlayer)
        //{
            //highesHandtypePlayers[0].GetComponent<ShowCards>().RpcRevealHand();
            //Ask each remainingPlayer if they would like to reveal
        //}
        //else
        //{
            foreach (Player el in remainingPlayers)
            {
                el.GetComponent<ShowCards>().RpcRevealHand();
            }
        //}

        return highesHandtypePlayers;
    }

    private static List<int> CompareHighestRanks(List<Player> highesHandtypePlayers, List<Card.Rank[]> highestRanks, int rankRow )
    {
        int[] rowOfRanks = new int[highesHandtypePlayers.Count];
        List<int> removeIdx = new List<int>();
        for (int i = 0; i < highesHandtypePlayers.Count; i++)
        {
            rowOfRanks[i] = (int)highestRanks[i][rankRow];
        }
        var highestRank = Mathf.Max(rowOfRanks);
        for (int i = 0; i < highesHandtypePlayers.Count; i++)
        {
            if ((int)highestRanks[i][rankRow] != highestRank)
            {
                removeIdx.Add(i);
            }
        }
        return removeIdx;
    }

    private void GivePotToPlayer(Player winner)
    {
        var playerPocket = winner.GetPocketValue();
        winner.SetPocketValue(playerPocket + potValue);
        potValue = 0;
        //Update Pot Value text.
        communityCards.RpcChangePotValueText("Pot: " + potValue.ToString());
        winner.GetComponent<ShowCards>().RpcChangeText(1, "Pocket: " + winner.GetPocketValue());
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

    public void NewRound()
    {
        NewRoundButton.SetActive(false);
        currentGameState = gameStates.Start;
        //Reset text about Hand Types or Winner.
        foreach (Player el in players)
        {
            el.currentPlayerState = Player.playerState.Uncalled;
            el.GetComponent<ShowCards>().RpcMakeTextInactive(3);
            el.GetComponent<ShowCards>().RpcMakeTextInactive(4);
            el.GetComponent<ShowCards>().RpcChangeText(2, "");
        }
        loneCaller = null;
        lastBetPlayer = null;
        //Set Next Dealer & Current Player
        NextDealer();
        currentPlayersTurn = currentDealerPlayer;
        NextPlayersTurn();
        currentBet = 0;
        curMinBet = minBet;
        dealer.ResetRound();
        roundActive = true;
        StartCoroutine(RunRound());
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
                break;
            default:
                break;
        }
    }



    private IEnumerator BettingRound()
    {
        bettingRoundActive = true;
        switch (currentGameState)
        {
            case gameStates.Preflop:
                PlaceBet(smallBlindValue, players[currentPlayersTurn]);
                players[currentPlayersTurn].GetComponent<ShowCards>().RpcChangeText(2,"Small Blind");
                NextPlayersTurn();
                PlaceBet(bigBlindValue, players[currentPlayersTurn]);
                players[currentPlayersTurn].GetComponent<ShowCards>().RpcChangeText(2, "Big Blind");
                currentBet = bigBlindValue;
                break;
            case gameStates.Flop:
                currentPlayersTurn = currentDealerPlayer;
                break;
            case gameStates.Turn:
                currentPlayersTurn = currentDealerPlayer;
                curMinBet *= 2;
                break;
            case gameStates.River:
                currentPlayersTurn = currentDealerPlayer;
                break;
            default:
                break;
        }

        while (areAnyPlayersNotCalledOrFolded())
        {
            NextPlayersTurn();
            yield return StartCoroutine(AskForCheckCallOrRaise(currentBet, curMinBet, players[currentPlayersTurn]));
        }

        if (IsOnlyOneCalledPlayerLeft())
        {
            roundActive = false;
        }
        currentBet = 0;
        bettingRoundActive = false;
    }


    private bool IsOnlyOneCalledPlayerLeft()
    {
        int numPlayersCalled = 0;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (players[i].currentPlayerState == Player.playerState.Called)
            {
                numPlayersCalled++;
                loneCaller = players[i];
            }

            if (numPlayersCalled > 1)
            {
                loneCaller = null;
                return false;
            }
                
        }
        return true;
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
        int raisedValue = player.GetRaisedValue();
        int callValue = curBet - raisedValue;

        //Setup Raise or Bet Other table.

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
                if (callValue > 0)
                {
                    //Show Called X Text
                    players[currentPlayersTurn].GetComponent<ShowCards>().RpcChangeText(2, "Called " + callValue.ToString());
                    PlaceBet(callValue, player);
                    player.currentPlayerState = Player.playerState.Called;
                }
                else
                {
                    //Show Checked Text
                    players[currentPlayersTurn].GetComponent<ShowCards>().RpcChangeText(2, "Checked");
                    player.currentPlayerState = Player.playerState.Called;
                }
                    break;
            case "Bet Min":
                //Show Raise X Text
                players[currentPlayersTurn].GetComponent<ShowCards>().RpcChangeText(2, "Raised " + minBet.ToString());
                PlaceBet(callValue + minBet, player);
                currentBet += minBet;
                MakeAllCalledPlayersUncalled();
                player.currentPlayerState = Player.playerState.Called;
                lastBetPlayer = player;
                break;
            case "Fold":
                player.currentPlayerState = Player.playerState.Folded;
                //Show Folded Text
                players[currentPlayersTurn].GetComponent<ShowCards>().RpcChangeText(2, "Folded");
                //Hide Their Cards
                players[currentPlayersTurn].GetComponent<Hand>().ResetHand();
                break;
        }
    }

    public IEnumerator WaitForAction(Player player)
    {
        player.RpcSetActionNull(); // clear last action, we want a new one
        player.RpcActivateGUIButtons();
        while (player.action == null)
        { yield return null; }
        player.RpcDeactivateGUIButtons();
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
        communityCards.RpcChangePotValueText("Pot: " + potValue.ToString());
        player.GetComponent<ShowCards>().RpcChangeText(1, "Pocket: " + player.GetPocketValue());
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

    private void NextDealer()
    {
        currentDealerPlayer++;
        if (currentDealerPlayer > numberOfPlayers - 1)
        {
            currentDealerPlayer = 0;
        }
        //Update Current Player Turn Text
    }
}
