using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FindLocalPlayer : NetworkBehaviour {

    
    public int localPlayerNum = -1;

    public int numberOfPlayers = 0;

    [ClientRpc]
    public void RpcSetNumberOfPlayers(int numPlayers)
    {
        numberOfPlayers = numPlayers;
    }
     

}
