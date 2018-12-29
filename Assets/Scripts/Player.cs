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


    [SyncVar] [SerializeField] public string action; //{ get; private set; }

    public GameObject CheckCallButton;
    public GameObject BetMinButton;
    public GameObject BetOtherButton;
    public GameObject FoldButton;

    [SerializeField] int pocketValue = 0;
    [SerializeField] int raisedValue = 0;

    GameObject[] buttons;

    private void Start()
    {
        buttons = new GameObject[4] { CheckCallButton, BetMinButton, BetOtherButton, FoldButton };
    }

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

    [ClientRpc]
    public void RpcActivateGUIButtons()
    {
        if (isLocalPlayer)
        {
            CheckCallButton.SetActive(true);
            BetMinButton.SetActive(true);
            BetOtherButton.SetActive(true);
        }
    }

    [ClientRpc]
    public void RpcActivateFoldButton()
    {
        if (isLocalPlayer)
        {
            FoldButton.SetActive(true);
        }
    }

    [ClientRpc]
    public void RpcSetButtonText(int buttonNum, string text)
    {
        if (isLocalPlayer)
        {
            buttons[buttonNum].GetComponent<Text>().text = text;
        }
    }

    [ClientRpc]
    public void RpcDeactivateGUIButtons()
    {
            CheckCallButton.SetActive(false);
            BetMinButton.SetActive(false);
            BetOtherButton.SetActive(false);
            FoldButton.SetActive(false);
    }

    [ClientRpc]
    public void RpcSetActionNull()
    {
        action = null;
    }

    public void CheckCallButtonClick()
    {
        if (isServer)
        {
            action = "CheckOrCall";
        }
        else
        {
            CmdCheckCallButtonClick();
        }
    }

    [Command]
    void CmdCheckCallButtonClick() 
    {
        CheckCallButtonClick();
    }

    public void BetMinButtonClick()
    {
        if (isServer)
        {
            action = "Bet Min";
        }
        else
        {
            CmdBetMinButtonClick();
        }
    }

    [Command]
    void CmdBetMinButtonClick()
    {
        BetMinButtonClick();
    }

    public void BetOtherButtonClick()
    {
        if (isServer)
        {
            //action = "Bet Other";
        }
        else
        {
            CmdBetOtherButtonClick();
        }
    }

    [Command]
    void CmdBetOtherButtonClick()
    {
        BetOtherButtonClick();
    }

    public void FoldButtonClick()
    {
        if (isServer)
        {
            action = "Fold";
        }
        else
        {
            CmdFoldButtonClick();
        }
    }

    [Command]
    void CmdFoldButtonClick()
    {
        FoldButtonClick();
    }
}
