using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPositions : MonoBehaviour {

    [SerializeField] Transform[] player1Positions;
    [SerializeField] Transform[] player2Positions;
    [SerializeField] Transform[] player3Positions;
    [SerializeField] Transform[] player4Positions;
    [SerializeField] Transform[] player5Positions;
    [SerializeField] Transform[] player6Positions;
    [SerializeField] Transform[] player7Positions;
    [SerializeField] Transform[] player8Positions;
    [SerializeField] Transform[] player9Positions;

    public Transform[][] playerCardPositions;

    private void Start()
    {
        playerCardPositions = new Transform[][] { 
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
    }

}
