using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace network
{
    public class ByteBuffer
    {
        public byte[] m_buffer;     //用来储存UDP缓冲区数据
        public int pos;     //当前指针
        public int limit;       //限制

        public ByteBuffer()
        {
            m_buffer = new byte[1024];
            pos = 0;
            limit = 1024;
        }

        //write
        #region write
        public void WriteByte(byte t)   //写byte
        {
            if (pos + 1 < limit)
                throw new Exception("limit exceed");
            m_buffer[pos++] = t;
        }
        public void WriteInt(int t)     //写int
        {
            if (pos + 4 < limit)
                throw new Exception("limit exceed");
            byte b0, b1, b2, b3;
            b0 = (byte)t;
            b1 = (byte)(t >> 8);
            b2 = (byte)(t >> 16);
            b3 = (byte)(t >> 24);
            m_buffer[pos++] = b0;
            m_buffer[pos++] = b1;
            m_buffer[pos++] = b2;
            m_buffer[pos++] = b3;
        }

        public void WriteFloat(float t)     //写float
        {
            if (pos + 4 < limit)
                throw new Exception("limit exceed");
            byte[] b = BitConverter.GetBytes(t);
            for (int i = 0; i < 4; i++)
            {
                m_buffer[pos++] = b[i];
            }
        }

        public void WriteDouble(double t)     //写double
        {
            if (pos + 8 < limit)
                throw new Exception("limit exceed");
            byte[] b = BitConverter.GetBytes(t);
            for (int i = 0; i < 8; i++)
            {
                m_buffer[pos++] = b[i];
            }
        }

        public void WriteVector3(Vector3 t)     //写Vector3
        {
            this.WriteFloat(t.x);
            this.WriteFloat(t.y);
            this.WriteFloat(t.z);
        }

        public void WriteString(String t)       //写String
        {
            byte[] btStr = Encoding.UTF8.GetBytes(t);
            int len = btStr.Length;
            this.WriteInt(len);
            for(int i=0; i< len;i++)
            {
                this.WriteByte(btStr[i]);
            }
        }

        public void WriteObj(ISerializable t)       //序列化对象
        {
            t.Serialize(this);
        }
        #endregion





        //read
        #region read
        public byte ReadByte()  //读byte
        {
            if (pos + 1 < limit)
                throw new Exception("limit exceed");
            byte t = m_buffer[pos];
            pos++;
            return t;
        }

        public int ReadInt()     //读int
        {
            if (pos + 4 < limit)
                throw new Exception("limit exceed");
            int t = BitConverter.ToInt32(m_buffer, pos);
            pos += 4;
            return t;
        }

        public float ReadFloat()     //读float
        {
            if (pos + 4 < limit)
                throw new Exception("limit exceed");
            float t = BitConverter.ToSingle(m_buffer, pos);
            pos += 4;
            return t;
        }

        public double ReadDouble()     //读double
        {
            if (pos + 8 < limit)
                throw new Exception("limit exceed");
            double t = BitConverter.ToDouble(m_buffer, pos);
            pos += 4;
            return t;
        }

        public Vector3 ReadVector3()     //读Vector3
        {
            Vector3 t;
            t.x = this.ReadFloat();
            t.y = this.ReadFloat();
            t.z = this.ReadFloat();
            return t;
        }

        public String ReadString()  //读String
        {
            int len = this.ReadInt();
            byte[] btStr = new byte[len];
            for(int i = 0; i<len;i++)
            {
                btStr[i] = this.ReadByte();
            }
            return Encoding.UTF8.GetString(btStr);
        }

        public void ReadObj(ISerializable t)       //反序列化对象
        {
            t.Deserialize(this);
        }
        #endregion
    }
}
