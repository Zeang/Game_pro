using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnterRoomResponseType { FULL,SUCCEED};//三种状态，房间满了进不去，成功并马上可以玩
public class  EnterRoomResponse :MonoBehaviour, network.ISerializable
{
    public int c_ip;
    public int s_ip;
    public EnterRoomResponseType response;
    public void Serialize(network.ByteBuffer buffer)
    {
        buffer.WriteInt(c_ip);
        buffer.WriteInt(s_ip);
        buffer.WriteInt((int)response);
    }

    //反序列化
    public void Deserialize(network.ByteBuffer buffer)
    {
        c_ip = buffer.ReadInt();
        s_ip = buffer.ReadInt();
        response = (EnterRoomResponseType)buffer.ReadInt();
    }
};
