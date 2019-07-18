using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace network
{
    public class NetworkManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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
