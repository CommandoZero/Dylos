using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCTest : MonoBehaviour
{

    //HOW TO RPC LIKE A GOD
    //*****************************************************************************************
    /*
     * RPC FROM CLIENT TO SERVER
     * 
     * The function to call other functions on the server
     * ExampleClient.instance.clientNet.CallRPC()
     * 
     * Parameters are:
     * 
     * string FunctionName : "FunctionName"
     * - The RPC needs to know which function youre calling
     * 
     * enum RPC message type : UCNetwork.MessageReceiver.ServerOnly 
     * - Are we sending an RPC to the server, to other clients, to everyone, to your mom, whatever
     * 
     * -1 : cause you have to ¯\_(ツ)_/¯
     * 
     * long clientID : ExampleClient.instance.GetClientID() 
     * - Which client are we sending this to if we are sending it to a specific one
     * 
     * int netID : GetComponent<NetworkSync>().GetId()
     * - What object has the function we are calling
     * 
     * FINAL PRODUCT EXAMPLE:
     * ExampleClient.instance.clientNet.CallRPC("FunctionName", UCNetwork.MessageReceiver.ServerOnly, -1, ExampleClient.instance.GetClientID(), GetComponent<NetworkSync>().GetId());
     * 
     * RPC FROM SERVER TO CLIENT
     * - The same thing but replace client with server, plus you need more info if youre gonna call an RPC back, make sure the method is public
     * 
     * public void FunctionName(long clientID, int netID)
     * {
     * 
     *  ExampleServer.instance.serverNet.CallRPC("SomeFunctionNameIDK", clientID, netID, etc.);
     * 
     * }
     * 
     */
    //*****************************************************************************************

    private void Start()
    {
        //RenderSettings.skybox.SetFloat("_Exposure", 0);
        if (FindObjectsOfType(typeof(RPCTest)).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ExampleClient.instance.clientNet.CallRPC("TestFunc", UCNetwork.MessageReceiver.ServerOnly, -1);
        //}

        //RenderSettings.skybox.SetFloat("_Exposure", Time.time * .1f);
    }
}
