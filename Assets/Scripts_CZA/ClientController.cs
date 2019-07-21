using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using network;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.InteropServices;

public class ClientController : MonoBehaviour
{
    // Start is called before the first frame update
    private NetworkManager Net;

    private GameObject[] PlayerRed;
    private GameObject[] PlayerBlue;

    private GameObject[] AIRed;
    private GameObject[] AIBlue;

    private int Id;
    private int Index_Event;

    private ByteBuffer buffer;

    void Start()
    {
        Net = GetComponent<NetworkManager>();
        Id = 1010;
        Index_Event = 0;

        Net.RegisterEvent(Id * 100 + Index_Event, SyPlayerMovement);
        Index_Event++;
        Net.RegisterEvent(Id * 100 + Index_Event, SyAIMovement);
        PlayerRed = GameObject.FindGameObjectsWithTag("PlayerRed");
        PlayerBlue = GameObject.FindGameObjectsWithTag("PlayerBlue");

        buffer = new ByteBuffer();

        //Net.GetComponent<NetworkManager>().SetClient(true);
    }

    public void SyPlayerMovement(int Event_id, ByteBuffer event_data)
    {
        for(int i = 0; i < PlayerRed.Length; i++)
        {
            PlayerRed[i].GetComponent<PlayerMovement>().Deserialize(buffer);
        }
        for(int i = 0; i < PlayerBlue.Length; i++)
        {
            PlayerBlue[i].GetComponent<PlayerMovement>().Deserialize(buffer);
        }
    }

    public void SyAIMovement(int Event_id, ByteBuffer event_data)
    {
        
    }


    private void FixedUpdate()
    {
        //AIRed = GameObject.FindGameObjectsWithTag("AIRed");
        //AIBlue = GameObject.FindGameObjectsWithTag("AIBlue");
    }
}
