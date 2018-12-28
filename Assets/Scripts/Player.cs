using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    public enum playerState
    {
        Uncalled,
        Called,
        Folded,
    };

    public playerState currentPlayerState = playerState.Uncalled;

    public string action { get; private set; }
    public GameObject CheckCallButton;
    public GameObject BetMinButton;
    public GameObject BetOtherButton;
    public GameObject FoldButton;

    [SerializeField] int pocketValue = 0;
    [SerializeField] int raisedValue = 0;

    public int GetPocketValue()
    {
        return pocketValue;
    }

    public void SetPocketValue(int value)
    {
        pocketValue = value;
    }

    public void SetRaisedValue(int value)
    {
        raisedValue = value;
    }

    public int GetRaisedValue()
    {
        return raisedValue;
    }

    public IEnumerator WaitForAction()
    {
        action = null; // clear last action, we want a new one
        while (action == null) { yield return null; }
    }

    public void CheckCallButtonClick() 
    { 
        action = "CheckOrCall"; 
    }

    public void BetMinButtonClick()
    {
        action = "Bet Min";
    }

    public void BetOtherButtonClick()
    {

    }

    public void FoldButtonClick()
    {
        action = "Fold";
    }
}
