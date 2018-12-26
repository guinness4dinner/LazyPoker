using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighestRanksDebug : MonoBehaviour {

    [SerializeField] Hand playerHand;
    private Text highestRanksText;


    // Use this for initialization
    void Start()
    {
        highestRanksText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        var highestRanks = playerHand.GetComponent<PokerHandChecker>().highestRanks;
        highestRanksText.text =
            highestRanks[0].ToString() + ", " +
            highestRanks[1].ToString() + ", " +
            highestRanks[2].ToString() + ", " +
            highestRanks[3].ToString() + ", " +
            highestRanks[4].ToString();
    }
}
