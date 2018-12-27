using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    [SerializeField] GameObject gameButtons;

    public void StartGameButton()
    {
        //gameButtons.SetActive(true);
        FindObjectOfType<PokerGameManager>().SetupGame();
        gameObject.SetActive(false);
    }
}
