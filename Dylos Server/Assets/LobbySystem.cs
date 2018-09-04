using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySystem : MonoBehaviour
{
    //The client enters the game 
    public void GameRequest(long clientID, int netID)
    {
        ExampleServer.instance.serverNet.CallRPC("EnterGame", clientID, netID);
    }
}
