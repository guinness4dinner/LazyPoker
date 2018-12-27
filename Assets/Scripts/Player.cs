using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum playerState
    {
        StartBet,
        Uncalled,
        Checked,
        Called,
        Raised,
        Folded,
    };

    public GameObject CheckCallButton;
    public GameObject BetMinButton;
    public GameObject BetOtherButton;
    public GameObject FoldButton;

    int pocketValue = 0;
    int calledValue = 0;

    public int GetPocketValue()
    {
        return pocketValue;
    }

    public void SetPocketValue(int value)
    {
        pocketValue = value;
    }
}
