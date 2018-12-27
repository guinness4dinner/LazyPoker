using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PokerHandChecker : MonoBehaviour {

    [SerializeField] Hand communityHand;

    public enum HandType
    {   
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
    }

    public HandType handType = HandType.HighCard;
    public Card.Rank[] highestRanks = new Card.Rank[] { Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none };
    List<Card> playerHand;

    private void Start()
    {
        var communityHandObject = GameObject.FindGameObjectWithTag("Community");
        communityHand = communityHandObject.GetComponent<Hand>();
    }

    public void ResetChecker()
    {
        handType = HandType.HighCard;
        highestRanks = new Card.Rank[] { Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none };
    }

    public void CheckPokerHand()
    {
        List<Card> pokerHand = new List<Card>();

        pokerHand.AddRange(GetComponent<Hand>().GetCardList());
        pokerHand.AddRange(communityHand.GetCardList());


        int[] numberEachRank = GetNumberEachRank(pokerHand);
        int[] numberEachSuit = GetNumberEachSuit(pokerHand);

        if (IsStraight(numberEachRank))
        {
            //Get Straight Hand
            List<Card> straightHand = GetStraightHand(pokerHand, numberEachRank);

            int[] numberEachSuitStraightHand = GetNumberEachSuit(straightHand);

            if (IsFlush(numberEachSuitStraightHand))
            {
                highestRanks = GetStraightRanks(straightHand);
                handType = HandType.StraightFlush;
            }
            else if (IsFlush(numberEachSuit))
            {
                highestRanks = GetFlushRanks(pokerHand, numberEachSuit);
                handType = HandType.Flush;
            }
            else
            {
                highestRanks = GetStraightRanks(straightHand);
                handType = HandType.Straight;
            }
        }
        else if (IsFourOfAKind(numberEachRank))
        {
            highestRanks = GetFourOfAKindRanks(pokerHand, numberEachRank);
            handType = HandType.FourOfAKind;
        }
        else if (IsFullHouse(numberEachRank))
        {
            highestRanks = GetFullHouseRanks(numberEachRank);
            handType = HandType.FullHouse;
        }
        else if (IsFlush(numberEachSuit))
        {
            highestRanks = GetFlushRanks(pokerHand, numberEachSuit);
            handType = HandType.Flush;
        }
        else if (IsThreeOfAKind(numberEachRank))
        {
            highestRanks = GetThreeOfAKindRanks(pokerHand, numberEachRank);
            handType = HandType.ThreeOfAKind;
        }
        else if (IsTwoPair(numberEachRank))
        {
            highestRanks = GetTwoPairRanks(pokerHand, numberEachRank);
            handType = HandType.TwoPair;
        }
        else if (IsOnePair(numberEachRank))
        {
            highestRanks = GetOnePairRanks(pokerHand, numberEachRank);
            handType = HandType.OnePair;
        }
        else
        {
            highestRanks = GetHighestRanks(pokerHand);
            handType = HandType.HighCard;
        }
    }

    private Card.Rank[] GetHighestRanks(List<Card> pokerHand)
    {
        var sortedHighCardHand = pokerHand.OrderByDescending(x => (int)(x.GetRank())).ToList();
        highestRanks[0] = sortedHighCardHand[0].GetRank();
        highestRanks[1] = sortedHighCardHand[1].GetRank();
        highestRanks[2] = sortedHighCardHand[2].GetRank();
        highestRanks[3] = sortedHighCardHand[3].GetRank();
        highestRanks[4] = sortedHighCardHand[4].GetRank();

        return highestRanks; 
    }

    private Card.Rank[] GetOnePairRanks(List<Card> pokerHand, int[] numberEachRank)
    {
        Card.Rank[] highestRanks = new Card.Rank[] { Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none };
        List<Card> highCardHand = new List<Card>();
        for (int i = 0; i < numberEachRank.Length; i++)
        {
            if (numberEachRank[i] >= 2)
            {
                    highestRanks[0] = (Card.Rank)i;
            }
        }

        foreach (Card el in pokerHand)
        {
            if (el.GetRank() != highestRanks[0])
            {
                highCardHand.Add(el);
            }
        }

        var sortedTwoPairHand = highCardHand.OrderByDescending(x => (int)(x.GetRank())).ToList();
        highestRanks[1] = sortedTwoPairHand[0].GetRank();
        highestRanks[2] = sortedTwoPairHand[1].GetRank();
        highestRanks[3] = sortedTwoPairHand[2].GetRank();

        return highestRanks;
    }

    private bool IsOnePair(int[] numberEachRank)
    {
        foreach (int el in numberEachRank)
        {
            if (el >= 2)
            {
                return true;
            }
        }

        return false;
    }

    private Card.Rank[] GetTwoPairRanks(List<Card> pokerHand, int[] numberEachRank)
    {
        Card.Rank[] highestRanks = new Card.Rank[] { Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none };
        List<Card> highCardHand = new List<Card>();
        bool twoOfAKindFound = false;
        for (int i = 0; i < numberEachRank.Length; i++)
        {
            if (numberEachRank[i] >= 2)
            {
                if (twoOfAKindFound)
                {
                    highestRanks[1] = highestRanks[0];
                    highestRanks[0] = (Card.Rank)i;
                }
                else
                {
                    highestRanks[0] = (Card.Rank)i;
                    twoOfAKindFound = true;
                }
            }
        }

        foreach (Card el in pokerHand)
        {
            if (el.GetRank() != highestRanks[0] && el.GetRank() != highestRanks[1])
            {
                highCardHand.Add(el);
            }
        }

        var sortedTwoPairHand = highCardHand.OrderByDescending(x => (int)(x.GetRank())).ToList();
        highestRanks[2] = sortedTwoPairHand[0].GetRank();
        return highestRanks;
    }

    private bool IsTwoPair(int[] numberEachRank)
    {
        bool twoOfAKind1 = false;
        bool twoOfAKind2 = false;
        foreach (int el in numberEachRank)
        {
            if (el >= 2)
            {
                if (!twoOfAKind1)
                {
                    twoOfAKind1 = true;
                }
                else if (!twoOfAKind2)
                {
                    twoOfAKind2 = true;
                }
            }
        }

        if (twoOfAKind1 && twoOfAKind2)
        {
            return true;
        }
        else { return false; }
    }

    private Card.Rank[] GetThreeOfAKindRanks(List<Card> pokerHand, int[] numberEachRank)
    {
        Card.Rank threesRank = Card.Rank.none;
        Card.Rank[] highestRanks = new Card.Rank[] { Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none };
        List<Card> highCardHand = new List<Card>();
        for (int i = 0; i < numberEachRank.Length; i++)
        {
            if (numberEachRank[i] >= 3)
            {
                threesRank = (Card.Rank)i;
                highestRanks[0] = threesRank;
            }
        }

        foreach (Card el in pokerHand)
        {
            if (el.GetRank() != threesRank)
            {
                highCardHand.Add(el);
            }
        }

        var sortedThreesHand = highCardHand.OrderByDescending(x => (int)(x.GetRank())).ToList();
        highestRanks[1] = sortedThreesHand[0].GetRank();
        highestRanks[2] = sortedThreesHand[1].GetRank();
        return highestRanks;
    }

    private bool IsThreeOfAKind(int[] numberEachRank)
    {
        foreach (int el in numberEachRank)
        {
            if (el >= 3)
            {
                return true;
            }
        }
        return false;
    }

    private Card.Rank[] GetFullHouseRanks(int[] numberEachRank)
    {
        Card.Rank[] highestRanks = new Card.Rank[] { Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none };
        bool threeOfAKind = false;
        bool twoOfAKind = false;
        for ( int i = 0; i < numberEachRank.Length; i++)
        {
            if (numberEachRank[i] >= 2)
            {
                if (numberEachRank[i] >= 3 && !threeOfAKind)
                {
                    highestRanks[0] = (Card.Rank)i;
                    threeOfAKind = true;
                }
                else if (!twoOfAKind || threeOfAKind)
                {
                    if (numberEachRank[i] >= 3 && threeOfAKind)
                    {
                        highestRanks[1] = highestRanks[0];
                        highestRanks[0] = (Card.Rank)i;
                    }
                    else
                    {
                        highestRanks[1] = (Card.Rank)i;
                    }
                    twoOfAKind = true;
                }
            }
        }
        return highestRanks;
    }

    private bool IsFullHouse(int[] numberEachRank)
    {
        bool threeOfAKind = false;
        bool twoOfAKind = false;
        foreach (int el in numberEachRank)
        {
            if (el >= 2)
            {
                if (el >= 3 && !threeOfAKind)
                {
                    threeOfAKind = true;
                }
                else if (!twoOfAKind || threeOfAKind) 
                {
                    twoOfAKind = true;
                }
            }
        }
        if (twoOfAKind && threeOfAKind)
        {
            return true;
        }
        else { return false; }
    }

    private Card.Rank[] GetFourOfAKindRanks(List<Card> pokerHand, int[] numberEachRank)
    {
        Card.Rank foursRank = Card.Rank.none;
        Card.Rank[] highestRanks = new Card.Rank[] { Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none };
        List<Card> highCardHand = new List<Card>();
        for (int i = 0; i < numberEachRank.Length; i++)
        {
            if (numberEachRank[i] >= 4)
            {
                foursRank = (Card.Rank)i;
                highestRanks[0] = foursRank;
            }
        }

        foreach (Card el in pokerHand)
        {
            if (el.GetRank() != foursRank)
            {
                highCardHand.Add(el);
            }
        }

        var sortedFoursHand = highCardHand.OrderByDescending(x => (int)(x.GetRank())).ToList();
        highestRanks[1] = sortedFoursHand[0].GetRank();
        return highestRanks;
    }

    private bool IsFourOfAKind(int[] numberEachRank)
    {
        foreach (int el in numberEachRank)
        {
            if (el >= 4)
            {
                return true;
            }
        }
        return false;
    }

    private Card.Rank[] GetFlushRanks(List<Card> pokerHand, int[] numberEachSuit)
    {
        Card.Suit flushSuit = Card.Suit.none;
        List<Card> flushHand = new List<Card>();
        Card.Rank[] highestRanks = new Card.Rank[] { Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none };

        for (int i = 0; i < numberEachSuit.Length; i++)
        {
            if (numberEachSuit[i] >= 5)
            {
                flushSuit = (Card.Suit)i;
            }
        }

        foreach (Card el in pokerHand)
        {
            if (el.GetSuit() == flushSuit)
            {
                flushHand.Add(el);
            }
        }

        var sortedFlushHand = flushHand.OrderByDescending(x => (int)(x.GetRank())).ToList();
        for (int i = 0; i < 5; i++)
        {
            highestRanks[i] = sortedFlushHand[i].GetRank();
        }

        return highestRanks;
    }

    private List<Card> GetStraightHand(List<Card> pokerHand, int[] numberEachRank)
    {
        int rankInRow = 0;
        Card.Rank highestRank = Card.Rank.none;
        List<Card> straightHand = new List<Card>();

        for (int i = 0; i < numberEachRank.Length; i++)
        {
            if (numberEachRank[numberEachRank.Length-1-i] > 0)
            {
                rankInRow++;
            }
            else
            {
                rankInRow = 0;
            }

            if (rankInRow == 5)
            {
                int j = numberEachRank.Length - i + 3;
                highestRank = (Card.Rank)j;
            }

            if (i == numberEachRank.Length && highestRank == Card.Rank.none)
            {
                highestRank = Card.Rank.five;
            }
        }

        foreach (Card el in pokerHand)
        {
            if ((int)el.GetRank() <= (int)highestRank && (int)el.GetRank() >= (int)highestRank - 4 )
            {
                straightHand.Add(el);
            }
        }
        return straightHand;
    }

    private Card.Rank[] GetStraightRanks(List<Card> pokerHand)
    {
        Card.Rank[] highestRanks = new Card.Rank[] { Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none, Card.Rank.none };

            var sortedHand = pokerHand.OrderByDescending(x => (int)(x.GetRank())).ToList();

        if (sortedHand[0].GetRank() == Card.Rank.A)
        {
            if (sortedHand[1].GetRank() == Card.Rank.five || sortedHand[2].GetRank() == Card.Rank.five || sortedHand[3].GetRank() == Card.Rank.five)
            {
                highestRanks[0] = Card.Rank.five;
            }
            else
            {
                highestRanks[0] = Card.Rank.A;
            }
        }
        else
        {
            highestRanks[0] = sortedHand[0].GetRank();
        }

        return highestRanks;
    }

    private bool IsStraight(int[] numberEachRank)
    {
        int rankInRow = 0;

        for (int i = 0; i < numberEachRank.Length; i++)
        {
            if (numberEachRank[i] > 0)
            {
                rankInRow++;
            }
            else
            {
                rankInRow = 0;
            }

            if (rankInRow == 4 && i == 3)
            {
                if (numberEachRank[13] > 0)
                    return true;
            }
            
            if (rankInRow == 5)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsFlush(int[] numberEachSuit)
    {
        foreach (int el in numberEachSuit)
        {
            if (el >= 5)
            {
                return true;
            }
        }

        return false;
    }

    private int[] GetNumberEachSuit(List<Card> pokerHand)
    {
        int[] numEachSuit = new int[5] { 0, 0, 0, 0, 0 };
        foreach (Card el in pokerHand)
        {
            if (el.GetSuit() == Card.Suit.Club)
            {
                numEachSuit[1]++;
            }
            else if (el.GetSuit() == Card.Suit.Diamond)
            {
                numEachSuit[2]++;
            }
            else if (el.GetSuit() == Card.Suit.Heart)
            {
                numEachSuit[3]++;
            }
            else if (el.GetSuit() == Card.Suit.Spades)
            {
                numEachSuit[4]++;
            }
        }

        return numEachSuit;
    }

    private int[] GetNumberEachRank(List<Card> pokerHand)
    {
        int[] numEachRank = new int[14] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        foreach (Card el in pokerHand)
        {
            if (el.GetRank() == Card.Rank.two)
            {
                numEachRank[1]++;
            }
            else if (el.GetRank() == Card.Rank.three)
            {
                numEachRank[2]++;
            }
            else if (el.GetRank() == Card.Rank.four)
            {
                numEachRank[3]++;
            }
            else if (el.GetRank() == Card.Rank.five)
            {
                numEachRank[4]++;
            }
            else if (el.GetRank() == Card.Rank.six)
            {
                numEachRank[5]++;
            }
            else if (el.GetRank() == Card.Rank.seven)
            {
                numEachRank[6]++;
            }
            else if (el.GetRank() == Card.Rank.eight)
            {
                numEachRank[7]++;
            }
            else if (el.GetRank() == Card.Rank.nine)
            {
                numEachRank[8]++;
            }
            else if (el.GetRank() == Card.Rank.ten)
            {
                numEachRank[9]++;
            }
            else if (el.GetRank() == Card.Rank.J)
            {
                numEachRank[10]++;
            }
            else if (el.GetRank() == Card.Rank.Q)
            {
                numEachRank[11]++;
            }
            else if (el.GetRank() == Card.Rank.K)
            {
                numEachRank[12]++;
            }
            else if (el.GetRank() == Card.Rank.A)
            {
                numEachRank[13]++;
            }

            
        }

        return numEachRank;
    }

}
