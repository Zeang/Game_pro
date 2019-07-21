using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using network;

public class ServerMovement : MonoBehaviour
{
    public ServerPlayerMovement[] Player;
    public ServerPlayerMovement[] AI;

    private GameObject[] PlayerRed;
    private GameObject[] PlayerBlue;

    private GameObject[] AIRed;
    private GameObject[] AIBlue;

    private int IndexCount;

    private void Awake()
    {
        Player = new ServerPlayerMovement[6];
        AI = new ServerPlayerMovement[20];
    }

    public void Scene2Buffer()
    {
        PlayerRed = GameObject.FindGameObjectsWithTag("PlayerRed");
        PlayerBlue = GameObject.FindGameObjectsWithTag("PlayerBlue");
        

        AIRed = GameObject.FindGameObjectsWithTag("AIRed");
        AIBlue = GameObject.FindGameObjectsWithTag("AIBlue");

        IndexCount = 0;
        for (int i = 0; i < PlayerRed.Length; i++)
        {
            Debug.Log("hello");
            Debug.Log(PlayerRed[i]);
            Debug.Log(Player[i]);
            OBJ2Server(ref PlayerRed[i], ref Player[i]);
            Debug.Log("world");
            IndexCount++;
        }
        for(int i = 0; i < PlayerBlue.Length; i++)
        {
            OBJ2Server(ref PlayerBlue[i], ref Player[IndexCount + i]);
        }

        IndexCount = 0;
        for(int i = 0; i < AIRed.Length; i++)
        {
            OBJ2Server(ref AIRed[i], ref AI[i]);
            IndexCount++;
        }
        for(int i = 0; i < AIBlue.Length; i++)
        {
            OBJ2Server(ref AIBlue[i], ref AI[IndexCount + i]);
        }
    }

    public void Buffer2Scene()
    {
        IndexCount = 0;
        AIRed = GameObject.FindGameObjectsWithTag("AIRed");
        AIBlue = GameObject.FindGameObjectsWithTag("AIBlue");
        for(int i = 0; i < 3; i++)
        {
            Server2OBJ(ref Player[i], ref PlayerRed[i]);
            Server2OBJ(ref Player[2 * i + 1], ref PlayerBlue[i]);
        }
        for(int i = 0; i < AIRed.Length; i++)
        {
            Server2OBJ(ref AI[i], ref AIRed[i]);
            IndexCount++;
        }
        for(int i = 0; i < AIBlue.Length; i++)
        {
            Server2OBJ(ref AI[IndexCount + i], ref AIBlue[i]);
        }

    }

    private void OBJ2Server(ref GameObject obj, ref ServerPlayerMovement data)
    {
        data.PlayerPos = obj.transform.position;
        data.PlayerForward = obj.transform.forward;
    }

    private void Server2OBJ(ref ServerPlayerMovement data, ref GameObject obj)
    {
        obj.transform.position = data.PlayerPos;
        obj.transform.forward = data.PlayerForward;
    }

}
