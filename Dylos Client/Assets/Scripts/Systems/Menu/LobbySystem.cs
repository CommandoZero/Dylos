using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySystem : MonoBehaviour
{

    private class Lobby
    {
        Scene m_Map;

        List<Player> m_PlayersInLobby;

        long lobbyID;
    }

    //If we want to have multiple lobbies, then we may have to copy scenes and link them to a Lobby object
    public void EnterGameRequest()
    {
        ExampleClient.instance.clientNet.CallRPC
           ("GameRequest", UCNetwork.MessageReceiver.ServerOnly, -1, ExampleClient.instance.GetClientID(), GetComponent<NetworkSync>().GetId());
    }

    public void EnterGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
    void JoinLobbyRequest(int currentPlayers, int maxNumberOfPlayers)
    {

    }
    void JoinLobby()
    {

    }
}
