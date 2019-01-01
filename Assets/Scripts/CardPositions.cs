using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPositions : MonoBehaviour {

    [SerializeField] Transform[] player0Positions;
    [SerializeField] Transform[] player1Positions;
    [SerializeField] Transform[] player2Positions;
    [SerializeField] Transform[] player3Positions;
    [SerializeField] Transform[] player4Positions;
    [SerializeField] Transform[] player5Positions;
    [SerializeField] Transform[] player6Positions;
    [SerializeField] Transform[] player7Positions;
    [SerializeField] Transform[] player8Positions;
    [SerializeField] Transform[] player9Positions;

    [SerializeField] Transform[] player0TextPos;
    [SerializeField] Transform[] player1TextPos;
    [SerializeField] Transform[] player2TextPos;
    [SerializeField] Transform[] player3TextPos;
    [SerializeField] Transform[] player4TextPos;
    [SerializeField] Transform[] player5TextPos;
    [SerializeField] Transform[] player6TextPos;
    [SerializeField] Transform[] player7TextPos;
    [SerializeField] Transform[] player8TextPos;
    [SerializeField] Transform[] player9TextPos;

    [SerializeField] Transform[] player0DealerBut;
    [SerializeField] Transform[] player1DealerBut;
    [SerializeField] Transform[] player2DealerBut;
    [SerializeField] Transform[] player3DealerBut;
    [SerializeField] Transform[] player4DealerBut;
    [SerializeField] Transform[] player5DealerBut;
    [SerializeField] Transform[] player6DealerBut;
    [SerializeField] Transform[] player7DealerBut;
    [SerializeField] Transform[] player8DealerBut;
    [SerializeField] Transform[] player9DealerBut;

    public List<int[]> playerPosList = new List<int[]>();
    public int[] playerPos;

    Transform[][] playerCardPositions;
    Transform[][] playerTextPositions;
    Transform[][] playerButPositions;

    public Transform[] GetPlayerCardPositions(int pos)
    {
        return playerCardPositions[playerPos[pos]];
    }

    public Transform[] GetPlayerTextPositions(int pos)
    {
        return playerTextPositions[playerPos[pos]];
    }

    public Transform[] GetPlayerButtonPositions(int pos)
    {
        return playerButPositions[playerPos[pos]];
    }

    public void SetPlayerPositions(int numberOfPlayers)
    {
        playerPos = playerPosList[numberOfPlayers-2];
    }


    private void Start()
    {
        playerCardPositions = new Transform[][] {
            player0Positions,
            player1Positions, 
            player2Positions, 
            player3Positions, 
            player4Positions, 
            player5Positions, 
            player6Positions,
            player7Positions,
            player8Positions,
            player9Positions
            };

        playerTextPositions = new Transform[][] {
            player0TextPos,
            player1TextPos,
            player2TextPos,
            player3TextPos,
            player4TextPos,
            player5TextPos,
            player6TextPos,
            player7TextPos,
            player8TextPos,
            player9TextPos
            };

        playerButPositions = new Transform[][] {
            player0DealerBut,
            player1DealerBut,
            player2DealerBut,
            player3DealerBut,
            player4DealerBut,
            player5DealerBut,
            player6DealerBut,
            player7DealerBut,
            player8DealerBut,
            player9DealerBut
            };

        int[] tempPlayerPos = new int[] { 0, 1 };
        playerPosList.Add(tempPlayerPos);
        tempPlayerPos = new int[] { 0, 2, 1 };
        playerPosList.Add(tempPlayerPos);
        tempPlayerPos = new int[] { 0, 2, 1, 3 };
        playerPosList.Add(tempPlayerPos);
        tempPlayerPos = new int[] { 0, 2, 4, 1, 3 };
        playerPosList.Add(tempPlayerPos);
        tempPlayerPos = new int[] { 0, 5, 2, 4, 1, 3 };
        playerPosList.Add(tempPlayerPos);
        tempPlayerPos = new int[] { 0, 5, 2, 4, 1, 3, 6 };
        playerPosList.Add(tempPlayerPos);
        tempPlayerPos = new int[] { 0, 5, 2, 4, 1, 7, 3, 6 };
        playerPosList.Add(tempPlayerPos);
        tempPlayerPos = new int[] { 0, 5, 2, 8, 4, 1, 7, 3, 6 };
        playerPosList.Add(tempPlayerPos);
        tempPlayerPos = new int[] { 0, 5, 2, 8, 4, 1, 7, 9, 3, 6 };
        playerPosList.Add(tempPlayerPos);
    }

}
