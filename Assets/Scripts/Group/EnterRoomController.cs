using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterRoomController : MonoBehaviour
{
    int enterRoomRequest;
    int enterRoomResponse;
    int ip;
    int serverIp;
    List<int> playerIps;

    public Text ipAsServerLabel;
    public Text ipAsClientLabel;
    public Text serverIpAsClientLabel;

    public GameObject CreateRoomPannel;
    public GameObject EnterRoomPannel;
    public Text ConnectingState;

    public int MaxPlayers;

    bool isClient;
    public network.NetworkManager networkManager;
    void Awake()
    {
        enterRoomRequest = 2536001;
        enterRoomResponse = 2536002;
        isClient = true;
        playerIps = new List<int>();
    }// Start is called before the first frame update
    void Start()
    {
        CreateRoomPannel.SetActive(false);
        EnterRoomPannel.SetActive(false);
        ConnectingState.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isClient)
        {
            int ip,serverIp;
            if(int.TryParse(ipAsClientLabel.text, out ip))
                UpdateIp(ip);
            
            if(int.TryParse(serverIpAsClientLabel.text, out serverIp))
                UpdateServerIp(serverIp);
        }
        else
        {
            int ip;
            if(int.TryParse(ipAsServerLabel.text, out ip))
                UpdateIp(ip);
        }
    }
    void OnEnterRoom(int event_id,network.ByteBuffer event_data)
    {
        if (isClient)
            return;
        EnterRoomRequest request = new EnterRoomRequest();
        //从buffer中构造对象
        request.Deserialize(event_data);
        if (request.s_ip != ip)
            return;//不是发给这台服务器的请求
        int c_ip = request.c_ip;
        if (playerIps.IndexOf(c_ip) == -1 && playerIps.Count + 1 <= MaxPlayers)
        {
            playerIps.Add(c_ip);
            if (playerIps.Count == MaxPlayers)
            {
                foreach(int ip in playerIps)
                {
                    EnterRoomResponse response = new EnterRoomResponse();
                    response.c_ip = ip;
                    response.s_ip = this.ip;
                    response.response = EnterRoomResponseType.SUCCEED;
                    network.ByteBuffer buffer = new network.ByteBuffer();
                    response.Serialize(buffer);
                    networkManager.TriggerEvent(enterRoomResponse, buffer);
                }
                SceneManager.LoadScene(9);
            }
        }
        else if(playerIps.IndexOf(c_ip) == -1)//不在里面，说明房间人满了
        {
            EnterRoomResponse response = new EnterRoomResponse();
            response.c_ip = c_ip;
            response.s_ip = this.ip;
            response.response = EnterRoomResponseType.FULL;
            network.ByteBuffer buffer = new network.ByteBuffer();
            response.Serialize(buffer);
            networkManager.TriggerEvent(enterRoomResponse, buffer);
        }
        //如果ip已经在房间内，则不做反应，也不重复加入

    }
    void OnResponse(int event_id, network.ByteBuffer event_data)
    {
        if (!isClient)
            return;
        EnterRoomResponse response = new EnterRoomResponse();
        response.Deserialize(event_data);
        if (response.c_ip != ip)
            return;//不是发给这台机器的响应
        if(response.response == EnterRoomResponseType.SUCCEED){
            SceneManager.LoadScene(9);
        }
        else
        {
            ConnectingState.gameObject.SetActive(true);
            ConnectingState.text = "Failed! This room is full...";
        }

    }
    public void CreateRoom()//若选择创建房间，则意味着是服务器，则这样初始化
    {
        isClient = false;
        networkManager.SetClient(isClient,0);
        networkManager.RegisterEvent(enterRoomRequest, OnEnterRoom);
        networkManager.RegisterEvent(enterRoomResponse, OnResponse);
        CreateRoomPannel.SetActive(true);
    }
    public void ConfirmCreateRoom()
    {
        CreateRoomPannel.SetActive(false);
        ConnectingState.gameObject.SetActive(true);
        ConnectingState.text = "waiting...";
    }
    public void EnterRoom()//若选择加入房间，则意味着是客户端，则这样初始化
    {
        networkManager.SetClient(isClient, IpToInt("127.0.0.1"));
        networkManager.RegisterEvent(enterRoomRequest, OnEnterRoom);
        networkManager.RegisterEvent(enterRoomResponse, OnResponse);
        EnterRoomPannel.SetActive(true);
    }
    public void ConfirmEnterRoom()
    {
        EnterRoomRequest request = new EnterRoomRequest();
        request.c_ip = ip;
        request.s_ip = serverIp;
        network.ByteBuffer buffer = new network.ByteBuffer();
        request.Serialize(buffer);
        networkManager.TriggerEvent(enterRoomRequest, buffer);
        EnterRoomPannel.SetActive(false);
        ConnectingState.gameObject.SetActive(true);
        ConnectingState.text = "waiting...";

    }
     int IpToInt(string ip)
    {
        char[] separator = new char[] { '.' };
        string[] items = ip.Split(separator);
        return int.Parse(items[0]) << 24
                | int.Parse(items[1]) << 16
                | int.Parse(items[2]) << 8
                | int.Parse(items[3]);
    }
    void UpdateIp(int ip)
    {
        this.ip = ip;
    }
    void UpdateServerIp(int ip)
    {
        this.serverIp = ip;
    }

}
