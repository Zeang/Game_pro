using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace network
{
    public class PlayerSerialize : MonoBehaviour, ISerializable
    {
        private Vector3 Position;
        private Vector3 LookForward;
        private int IsShooting = 0;
        private float MoveX;
        private float MoveY;
        private int isJump;
        private int blood;

        public PlayerSerialize(GameObject obj)
        {
            Position = obj.transform.position;
            LookForward = obj.transform.forward;
        }

        public void Serialize(ByteBuffer buffer)
        {
            buffer.WriteVector3(Position);
            buffer.WriteVector3(LookForward);
            buffer.WriteInt(IsShooting);
            buffer.WriteFloat(MoveX);
            buffer.WriteFloat(MoveY);
            buffer.WriteInt(isJump);
            buffer.WriteInt(blood);
        }

        public void Deserialize(ByteBuffer buffer)
        {
            Position = buffer.ReadVector3();
            LookForward = buffer.ReadVector3();
            IsShooting = buffer.ReadInt();
            MoveX = buffer.ReadFloat();
            MoveY = buffer.ReadFloat();
            isJump = buffer.ReadInt();
            blood = buffer.ReadInt();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public class NetworkManager : MonoBehaviour
    {
        [SerializeField]
        private network.NetworkManager NetObj = null;

        private bool isClient;
        private int id;

        private int index = 0;

        private GameObject[] PlayerRed = new GameObject[3];
        private GameObject[] PlayerBlue = new GameObject[3];

        private GameObject[] AIRed;
        private GameObject[] AIBlue;

        ByteBuffer buffer = new ByteBuffer();

        // Start is called before the first frame update
        //void Start()
        //{
        //    SetClient(false, 174985837);
        //    isClient = true;
        //    id = 2811;
        //    PlayerRed = GameObject.FindGameObjectsWithTag("PlayerRed");
        //    PlayerBlue = GameObject.FindGameObjectsWithTag("PlayerBlue");

        //    AIRed = GameObject.FindGameObjectsWithTag("AIRed");
        //    AIBlue = GameObject.FindGameObjectsWithTag("AIBlue");

        //    RegisterEvent(12345, OnPlayerMoveInfo);
        //}

        public void OnPlayerMoveInfo(int event_id, ByteBuffer event_data)
        {
            for(int i = 0; i < 3; i++)
            {
                //PlayerRed[i].Derserialize(event_data);
                //PlayerBlue[i].Derserialize(event_data);
                //foreach (GameObject AI in AIRed)
                //{
                //    AI.Serialize(buffer);
                //}
                //foreach (GameObject AI in AIBlue)
                //{
                //    AI.Serialize(buffer);
                //}
            }
        }

        
        ////客户端收到事件
        //public void OnComputerBuy(int event_id, ByteBuffer event_data)
        //{
        //    Computer computer = new Computer();
        //    //从buffer中构造对象
        //    computer.Deserialize(event_data);


        //}

        ////在服务器上，买了一台电脑
        //public void BuyComputer()
        //{
        //    Computer computer = new Computer();
        //    //实例字段的赋值

        //    //触发事件
        //    buffer = new ByteBuffer();
        //    computer.Serialize(buffer);
        //    TriggerEvent(4316001, buffer);
        //}



        // Update is called once per frame
        void Update()
        {
            if(isClient == false)
            {
                AIRed = GameObject.FindGameObjectsWithTag("AIRed");
                AIBlue = GameObject.FindGameObjectsWithTag("AIBlue");

                PerformMoveInfo();
            }
        }

        private void PerformMoveInfo()
        {
            //for(int i = 0; i < 3; i++)
            //{
            //    PlayerRed[i].Serialize(buffer);
            //    PlayerBlue[i].Serialize(buffer);
            //}
            
            //foreach(GameObject AI in AIRed)
            //{
            //    AI.Serialize(buffer);
            //}
            //foreach(GameObject AI in AIBlue)
            //{
            //    AI.Serialize(buffer);
            //}

            //TriggerEvent(12345, buffer);
        }

        //event handler
        public delegate void EventHandler(int event_id, ByteBuffer event_data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void EventHandlerRaw(int event_id, IntPtr event_data, int length);


        //register event
        [DllImport("zztnetwork.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "RegisterEvent")]
        private static extern void RegisterEventRaw(int event_id, EventHandlerRaw handler);


        public void RegisterEvent(int event_id, EventHandler handler)
        {
            /*网络的处理以及加密我用C++来实现，几乎已经完成了，
             * 正在测试中，各种操作对于上层是透明的，
             * 测试完成后我会按照此接口与C#对接
             */
            //创建匿名方法
            EventHandlerRaw raw_handler = delegate (int event_id_raw, IntPtr event_data_raw, int length)
            {
                ByteBuffer buffer = new ByteBuffer();
                Marshal.Copy(event_data_raw, buffer.m_buffer, 0, length);
                buffer.limit = length;

                handler(event_id_raw, buffer);
            };

            RegisterEventRaw(event_id, raw_handler);
        }


        //trigger event
        [DllImport("zztnetwork.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "TriggerEvent")]
        public static extern void TriggerEventRaw(int event_id, IntPtr event_data, int length);

        public void TriggerEvent(int event_id, ByteBuffer event_data)
        {
            IntPtr data_raw = Marshal.AllocHGlobal(event_data.pos);
            Marshal.Copy(event_data.m_buffer, 0, data_raw, event_data.pos);

            TriggerEventRaw(event_id, data_raw, event_data.pos);

            Marshal.FreeHGlobal(data_raw);
        }



        //set client
        [DllImport("zztnetwork.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "SetClient")]
        public static extern void SetClientRaw(int is_client, int server_ip);
        public void SetClient(bool client, int server_ip = 0)
        {
            SetClientRaw(client ? 1 : 0, server_ip);
        }
        
    }
}
