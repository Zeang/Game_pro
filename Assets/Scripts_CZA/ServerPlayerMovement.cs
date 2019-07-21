using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using network;

public class ServerPlayerMovement : MonoBehaviour, ISerializable
{
    public Vector3 PlayerPos;
    public Vector3 PlayerForward;
    public Vector3 PlayerVelocity;

    public int PlayerBlood;
    public int PlayerIsShooting;
    public int PlayerIsJump;
    public ServerPlayerMovement()
    {
        PlayerPos = new Vector3();
        PlayerForward = new Vector3();
    }

    public void Serialize(ByteBuffer buffer)
    {
        buffer.WriteVector3(PlayerPos);
        buffer.WriteVector3(PlayerForward);
        //buffer.WriteVector3(PlayerVelocity);
        //buffer.WriteInt(PlayerBlood);
        //buffer.WriteInt(PlayerIsShooting);
        //buffer.WriteInt(PlayerIsJump);

    }

    public void Deserialize(ByteBuffer buffer)
    {
        PlayerPos = buffer.ReadVector3();
        PlayerForward = buffer.ReadVector3();
        //PlayerVelocity = buffer.ReadVector3();
        //PlayerBlood = buffer.ReadInt();
        //PlayerIsShooting = buffer.ReadInt();
        //PlayerIsJump = buffer.ReadInt();
    }

    
}
