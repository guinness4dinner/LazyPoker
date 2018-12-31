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

    [SerializeField] public GameObject potValueText;

    [SerializeField] public GameObject playerNameText;
    [SerializeField] public GameObject pocketValueText;
    [SerializeField] public GameObject statusText;
    [SerializeField] public GameObject winnerText;
    [SerializeField] public GameObject handTypeText;

    [SerializeField] FindLocalPlayer findLocalPlayer;

    [SyncVar]
    public int playerNum = -1;

    private void Start()
    {
        cardPrefabs = FindObjectOfType<CardPrefabs>();
        findLocalPlayer = FindObjectOfType<FindLocalPlayer>();
    }

    [ClientRpc]
    public void RpcSetLocalPlayer(int i)
    {
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
            playerNameText.SetActive(true);

            pocketValueText.GetComponent<TextMesh>().text = "Pocket: " + GetComponent<Player>().GetPocketValue().ToString();
            pocketValueText.SetActive(true);
        }
        else
        {
            int localPlayerNum = findLocalPlayer.localPlayerNum;
            int numberOfPlayers = findLocalPlayer.positionOrder.Length;
            int posNum = 0;
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

            playerNameText.transform.position = textPositions[0].position;
            pocketValueText.transform.position = textPositions[1].position;
            statusText.transform.position = textPositions[2].position;
            winnerText.transform.position = textPositions[3].position;
            handTypeText.transform.position = textPositions[4].position;

            playerNameText.GetComponent<TextMesh>().text = "Player " + playerNum.ToString();
            playerNameText.SetActive(true);

            pocketValueText.GetComponent<TextMesh>().text = "Pocket: " + GetComponent<Player>().GetPocketValue().ToString();
            pocketValueText.SetActive(true);
        }

	}

    [ClientRpc]
    public void RpcCommunitySetup()
    {
        cardPositions = localCardPositions;
        potValueText.SetActive(true);
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
        foreach (Card el in GetComponentsInChildren<Card>())
        {
            el.GetComponent<Transform>().Rotate(0, 0, 0);
        }
    }
}
