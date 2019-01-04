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
    [SyncVar] [SerializeField] public int betOtherAction = -1;

    public GameObject CheckCallButton;
    public GameObject BetMinButton;
    public GameObject BetOtherButton;
    public GameObject FoldButton;

    public GameObject BetOtherMenu;
    public GameObject Bet2xButton;
    public GameObject Bet3xButton;
    public GameObject Bet4xButton;
    public GameObject Bet5xButton;
    public GameObject Bet10xButton;
    public GameObject Bet20xButton;
    public GameObject AllInButton;

    [SerializeField] int pocketValue = 0;
    [SerializeField] int raisedValue = 0;
    [SerializeField] GameObject buttonCanvas;

    GameObject[] buttons;
    GameObject[] betOtherButtons;

    private void Start()
    {
        if (isLocalPlayer)
        {
            buttonCanvas.SetActive(true);
        }
        buttons = new GameObject[4] { CheckCallButton, BetMinButton, BetOtherButton, FoldButton };
        betOtherButtons = new GameObject[7] { Bet2xButton, Bet3xButton, Bet4xButton, Bet5xButton, Bet10xButton, Bet20xButton, AllInButton };
    }

    public int GetPocketValue()
    {
        return pocketValue;
    }

    public void SetPocketValue(int value)
    {
        pocketValue = value;
        GetComponent<ShowCards>().pocketValueText.GetComponent<TextMesh>().text = "Pocket: " + pocketValue.ToString();
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
    public void RpcSetBetOtherButtonText(int buttonNum, string text)
    {
        if (isLocalPlayer)
        {
            betOtherButtons[buttonNum].GetComponent<Text>().text = text;
        }
    }

    [ClientRpc]
    public void RpcDeactivateGUIButtons()
    {
        if (isLocalPlayer)
        {
            DeactivateGUIButtons();
        }
    }

    public void DeactivateGUIButtons()
    {
        if (CheckCallButton.activeSelf)
        {
            CheckCallButton.SetActive(false);
        }
        if (BetMinButton.activeSelf)
        {
            BetMinButton.SetActive(false);
        }
        if (BetOtherButton.activeSelf)
        {
            BetOtherButton.SetActive(false);
        }
        if (BetOtherMenu.activeSelf)
        {
            BetOtherMenu.SetActive(false);
        }
        if (FoldButton.activeSelf)
        {
            FoldButton.SetActive(false);
        }
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
            DeactivateGUIButtons();
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
            DeactivateGUIButtons();
        }
    }

    [Command]
    void CmdBetMinButtonClick()
    {
        BetMinButtonClick();
    }

    public void BetOtherButtonClick()
    {
        if (BetOtherMenu.activeSelf)
        {
            BetOtherMenu.SetActive(false);
        }
        else
        {
            BetOtherMenu.SetActive(true);
        }
        
    }

    public void BetOtherMenuButtonClick(int betOtherButNum)
    {
        if (isServer)
        {
            action = "Bet Other";
            betOtherAction = betOtherButNum;
        }
        else
        {
            CmdBetOtherMenuButtonClick(betOtherButNum);
            DeactivateGUIButtons();
        }
    }

    [Command]
    void CmdBetOtherMenuButtonClick(int betOtherButNum)
    {
        BetOtherMenuButtonClick(betOtherButNum);
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
            DeactivateGUIButtons();
        }
    }

    [Command]
    void CmdFoldButtonClick()
    {
        FoldButtonClick();
    }
}
