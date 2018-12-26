using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandTypeDebug : MonoBehaviour {

    [SerializeField] Hand playerHand;
    private Text handTypeText;


	// Use this for initialization
	void Start () {
        handTypeText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        handTypeText.text = playerHand.GetComponent<PokerHandChecker>().handType.ToString();
	}
}
