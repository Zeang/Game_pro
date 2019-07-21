using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using network;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.InteropServices;

public class ServerController : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField]
    private NetworkManager Net;

    private GameObject[] PlayerRed;
    private GameObject[] PlayerBlue;

    private GameObject[] AIRed;
    private GameObject[] AIBlue;

    private int Client_Id;
    private int Index_Event;

    private ByteBuffer buffer;

    void Start()
    {
        Net = GetComponent<NetworkManager>();
        Client_Id = 1010;
        buffer = new ByteBuffer();
        PlayerRed = GameObject.FindGameObjectsWithTag("PlayerRed");
        PlayerBlue = GameObject.FindGameObjectsWithTag("PlayerBlue");
        //Net.GetComponent<NetworkManager>().SetClient(false);

    }

    
    private void FixedUpdate()
    {
        AIRed = GameObject.FindGameObjectsWithTag("AIRed");
        AIBlue = GameObject.FindGameObjectsWithTag("AIBlue");


        Index_Event = 0;
        
        for(int i = 0; i < PlayerRed.Length; i++)
        {
            PlayerRed[i].GetComponent<PlayerMovement>().Serialize(buffer);
        }
        for(int i = 0; i < PlayerBlue.Length; i++)
        {
            PlayerBlue[i].GetComponent<PlayerMovement>().Serialize(buffer);
        }
        Net.TriggerEvent(Client_Id * 100 + Index_Event, buffer);
        Index_Event++;

        //for(int i = 0; i < AIRed.Length; i++)
        //{
        //    AIRed[i].GetComponent<PlayerMovement>().Serialize(buffer);
        //}
        //for(int i = 0; i < AIBlue.Length; i++)
        //{
        //    AIBlue[i].GetComponent<PlayerMovement>().Serialize(buffer);
        //}
        //Net.TriggerEvent(Client_Id * 100 + Index_Event, buffer);

    }
}
