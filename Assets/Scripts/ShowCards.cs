using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShowCards : NetworkBehaviour {

    [SerializeField] CardPrefabs cardPrefabs;
    [SerializeField] List<int> cardList = new List<int>();
    [SerializeField] Transform[] localCardPositions;

    [SerializeField] Transform[] cardPositions;

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
        }
	}

    [ClientRpc]
    public void RpcCommunitySetup()
    {
            cardPositions = localCardPositions;
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
}
