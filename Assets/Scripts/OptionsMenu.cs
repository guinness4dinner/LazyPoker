using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    [SerializeField] GameObject playerPocketsSection;
    [SerializeField] GameObject p1PocketOption;
    [SerializeField] GameObject p2PocketOption;
    [SerializeField] GameObject p3PocketOption;
    [SerializeField] GameObject p4PocketOption;
    [SerializeField] GameObject p5PocketOption;
    [SerializeField] GameObject p6PocketOption;
    [SerializeField] GameObject p7PocketOption;
    [SerializeField] GameObject p8PocketOption;
    [SerializeField] GameObject p9PocketOption;
    [SerializeField] GameObject p10PocketOption;

    [SerializeField] GameObject smallBlindOption;
    [SerializeField] GameObject bigBlindOption;
    [SerializeField] GameObject minBetOption;
    [SerializeField] GameObject startingPocketOption;

    Player[] players;
    GameObject[] plPocketOptions;
    PokerGameManager gameManager;

    private void Start()
    {
        plPocketOptions = new GameObject[10] { p1PocketOption, p2PocketOption, p3PocketOption, p4PocketOption,
            p5PocketOption, p6PocketOption, p7PocketOption, p8PocketOption, p9PocketOption, p10PocketOption, };
        gameManager = FindObjectOfType<PokerGameManager>();
        gameObject.SetActive(false);
    }

    public void CloseOptionsMenu()
    {
        gameObject.SetActive(false);
    }

    public void SetPlayers(Player[] playersToSet)
    {
        startingPocketOption.SetActive(false);

        playerPocketsSection.SetActive(true);

        players = playersToSet;

        for (int i = 0; i < players.Length; i++)
        {
            plPocketOptions[i].SetActive(true);
            //Set Name
        }
    }

    public void RefreshOptionsText()
    {
        if (players != null)
        {
            for (int i = 0; i < players.Length; i++)
            {
                plPocketOptions[i].GetComponentInChildren<InputField>().text = players[i].GetPocketValue().ToString();
            }
        }

        if(startingPocketOption.activeSelf)
        {
            startingPocketOption.GetComponentInChildren<InputField>().text = gameManager.GetStartingPocket().ToString();
        }

        smallBlindOption.GetComponentInChildren<InputField>().text = gameManager.GetSmallBlind().ToString();
        bigBlindOption.GetComponentInChildren<InputField>().text = gameManager.GetBigBlind().ToString();
        minBetOption.GetComponentInChildren<InputField>().text = gameManager.GetMinimumBet().ToString();

    }

    public void SetPlayerPockets()
    {
        for (int i = 0; i < players.Length; i++)
        {
            int pocketToSet;
            if (int.TryParse(plPocketOptions[i].GetComponentInChildren<InputField>().text, out pocketToSet))
            {
                players[i].SetPocketValue(pocketToSet);
            }           
        }
    }

    public void ChangeSmallBlind()
    {
        int valueToSet;
        if (int.TryParse(smallBlindOption.GetComponentInChildren<InputField>().text, out valueToSet))
        {
            gameManager.SetSmallBlind(valueToSet);
        }       
    }

    public void ChangeBigBlind()
    {
        int valueToSet;
        if (int.TryParse(bigBlindOption.GetComponentInChildren<InputField>().text, out valueToSet))
        {
            gameManager.SetBigBlind(valueToSet);
        }
    }

    public void ChangeMinimumBet()
    {
        int valueToSet;
        if (int.TryParse(minBetOption.GetComponentInChildren<InputField>().text, out valueToSet))
        {
            gameManager.SetMinimumBet(valueToSet);
        }
    }

    public void ChangeStartingPocket()
    {
        int valueToSet;
        if (int.TryParse(startingPocketOption.GetComponentInChildren<InputField>().text, out valueToSet))
        {
            gameManager.SetStartingPocket(valueToSet);
        }
    }

    public void ForceNewRoundClick()
    {
        gameManager.ForceNewRound();
    }
}
