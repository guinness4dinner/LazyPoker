using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FindLocalPlayer : NetworkBehaviour {

    public int[] positionOrder;
    
    public int localPlayerNum = -1;

    [ClientRpc]
    public void RpcSetPosOrder(int[] posOrder)
    {
        positionOrder = posOrder;
    }
}
