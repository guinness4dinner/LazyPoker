using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShowCards : NetworkBehaviour {

    [SerializeField] CardPrefabs cardPrefabs;
    [SerializeField] List<int> cardList = new List<int>();
    public Transform[] networkCardPositions;
    [SerializeField] Transform[] localCardPositions;

    [SerializeField] Transform[] cardPositions;

    int playerNumber = 0;

    private void Start()
    {
        cardPrefabs = FindObjectOfType<CardPrefabs>();
    }

    [ClientRpc]
    public void RpcSetPlayerNumber(int playerNumberToSet)
    {
        playerNumber = playerNumberToSet;
    }

    [ClientRpc]
    public void RpcSetup () {

       networkCardPositions = FindObjectOfType<CardPositions>().playerCardPositions[playerNumber];

        if (isLocalPlayer)
        {
            cardPositions = localCardPositions;
        }
        else
        {
            cardPositions = networkCardPositions;
        }
	}

    [ClientRpc]
    public void RpcCommunitySetup()
    {
            cardPositions = networkCardPositions;
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
