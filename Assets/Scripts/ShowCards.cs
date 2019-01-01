using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShowCards : NetworkBehaviour {

    [SerializeField] CardPrefabs cardPrefabs;
    [SerializeField] List<int> cardList = new List<int>();
    [SerializeField] Transform[] localCardPositions;

    [SerializeField] Transform[] cardPositions;
    [SerializeField] Transform[] textPositions;
    [SerializeField] Transform[] buttonPostions;


    [SerializeField] public TextMesh potValueText;

    [SerializeField] public TextMesh playerNameText;
    [SerializeField] public TextMesh pocketValueText;
    [SerializeField] public TextMesh statusText;
    [SerializeField] public TextMesh winnerText;
    [SerializeField] public TextMesh handTypeText;

    [SerializeField] public GameObject dealerButton;
    [SerializeField] public GameObject currentTurnButton;

    [SerializeField] FindLocalPlayer findLocalPlayer;

    TextMesh[] infoTexts = new TextMesh[] { null, null, null, null, null };

    [SyncVar]
    public int playerNum = -1;

    public void Start()
    {
        cardPrefabs = FindObjectOfType<CardPrefabs>();
        infoTexts = new TextMesh[] { playerNameText, pocketValueText, statusText, winnerText, handTypeText };
    }

    [ClientRpc]
    public void RpcSetLocalPlayer(int i)
    {
        findLocalPlayer = FindObjectOfType<FindLocalPlayer>();

        playerNum = i;

        if (isLocalPlayer)
        {
            findLocalPlayer.localPlayerNum = playerNum;
        }
    }

    [ClientRpc]
    public void RpcSetup () {

        if (isLocalPlayer)
        {
            cardPositions = localCardPositions;

            playerNameText.GetComponent<TextMesh>().text = "You";
            playerNameText.gameObject.SetActive(true);
            pocketValueText.gameObject.SetActive(true);
        }
        else
        {
            int localPlayerNum = findLocalPlayer.localPlayerNum;
            int numberOfPlayers = findLocalPlayer.numberOfPlayers;
            int posNum = 0;

            FindObjectOfType<CardPositions>().SetPlayerPositions(numberOfPlayers);

            if (playerNum < localPlayerNum )
            {
                posNum = numberOfPlayers - localPlayerNum + playerNum;
            }
            else
            {
                posNum = playerNum - localPlayerNum;
            }
            cardPositions = FindObjectOfType<CardPositions>().GetPlayerCardPositions(posNum);
            textPositions = FindObjectOfType<CardPositions>().GetPlayerTextPositions(posNum);
            buttonPostions = FindObjectOfType<CardPositions>().GetPlayerButtonPositions(posNum);

            playerNameText.transform.position = textPositions[0].position;
            pocketValueText.transform.position = textPositions[1].position;
            statusText.transform.position = textPositions[2].position;
            winnerText.transform.position = textPositions[3].position;
            handTypeText.transform.position = textPositions[4].position;

            dealerButton.transform.position = buttonPostions[0].position;
            currentTurnButton.transform.position = buttonPostions[1].position;
            currentTurnButton.transform.rotation = buttonPostions[1].rotation;

            var playerNumText = playerNum + 1;
            playerNameText.text = "Player " + playerNumText.ToString();
            playerNameText.gameObject.SetActive(true);
            pocketValueText.gameObject.SetActive(true);
        }

	}

    [ClientRpc]
    public void RpcChangeText(int infoTextNum, string text)
    {
        infoTexts[infoTextNum].text = text;
    }

    [ClientRpc]
    public void RpcMakeTextActive(int infoTextNum)
    {
        infoTexts[infoTextNum].gameObject.SetActive(true);
    }

    [ClientRpc]
    public void RpcMakeTextInactive(int infoTextNum)
    {
        infoTexts[infoTextNum].gameObject.SetActive(false);
    }

    [ClientRpc]
    public void RpcActivateDealerButton()
    {
        dealerButton.SetActive(true);
    }

    [ClientRpc]
    public void RpcDeactivateDealerButton()
    {
        dealerButton.SetActive(false);
    }

    [ClientRpc]
    public void RpcActivateTurnButton()
    {
        currentTurnButton.SetActive(true);
    }

    [ClientRpc]
    public void RpcDeactivateTurnButton()
    {
        currentTurnButton.SetActive(false);
    }

    [ClientRpc]
    public void RpcCommunitySetup()
    {
        cardPositions = localCardPositions;
        potValueText.gameObject.SetActive(true);
    }

    [ClientRpc]
    public void RpcChangePotValueText(string text)
    {
        potValueText.text = text;
    }

    [ClientRpc]
    public void RpcMakePotValueTextActive()
    {
        potValueText.gameObject.SetActive(true);
    }

    [ClientRpc]
    public void RpcMakePotValueTextInactive()
    {
        potValueText.gameObject.SetActive(false);
    }

    [ClientRpc]
    public void RpcAddCard(int card)
    {
        cardList.Add(card);
        UpdateShownCards();
    }

    [ClientRpc]
    public void RpcRemoveCard(int card)
    {
        cardList.Remove(card);
        UpdateShownCards();
    }

    [ClientRpc]
    public void RpcReset()
    {
        cardList = new List<int>();
        UpdateShownCards();
    }

    public void UpdateShownCards()
    {
        foreach (Card el in GetComponentsInChildren<Card>())
        {
            Destroy(el.gameObject);
        }

        if (cardList.Count > 0)
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                var cardPrefab = cardPrefabs.cardPrefabs[cardList[i]];
                Instantiate(cardPrefab, cardPositions[i].position, cardPositions[i].rotation, GetComponent<Transform>());
            }
        }

    }


    //Specific to LazyPoker
    [ClientRpc]
    public void RpcRevealHand()
    {
        if(!isLocalPlayer)
        {
            foreach (Card el in GetComponentsInChildren<Card>())
            {
                el.GetComponent<Transform>().Rotate(-180, 0, 0);
            }
        }
    }
}
