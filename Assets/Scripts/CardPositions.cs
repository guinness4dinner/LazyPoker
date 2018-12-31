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

    Transform[][] playerCardPositions;
    Transform[][] playerTextPositions;

    public Transform[] GetPlayerCardPositions(int pos)
    {
        return playerCardPositions[pos];
    }

    public Transform[] GetPlayerTextPositions(int pos)
    {
        return playerTextPositions[pos];
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

    }

}
