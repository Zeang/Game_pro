using System.Collections;
using System.Collections.Generic;
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


        public delegate void EventHandler(int event_id, ByteBuffer event_data);

        public void RegisterEvent(int event_id, EventHandler handler)
        {
            /*网络的处理以及加密我用C++来实现，几乎已经完成了，
             * 正在测试中，各种操作对于上层是透明的，
             * 测试完成后我会按照此接口与C#对接
             */
        }

        public void TriggerEvent(int event_id, ByteBuffer event_data)
        {
            //预留的接口
        }

        public void SetClient(bool client)
        {
            //预留的接口
        }
    }
}
