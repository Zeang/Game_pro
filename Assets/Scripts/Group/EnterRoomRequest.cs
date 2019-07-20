using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomRequest:MonoBehaviour, network.ISerializable
{
    public int c_ip;//用户ip
    public int s_ip;//目标服务器ip
                    //序列化

    public void Serialize(network.ByteBuffer buffer)
    {
        buffer.WriteInt(c_ip);
        buffer.WriteInt(s_ip);
    }

    //反序列化
    public void Deserialize(network.ByteBuffer buffer)
    {
        c_ip = buffer.ReadInt();
        s_ip= buffer.ReadInt();
    }
};
