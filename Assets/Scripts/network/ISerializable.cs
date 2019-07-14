using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    public interface ISerializable
    {
        void Serialize(ByteBuffer buffer);
        void Deserialize(ByteBuffer buffer);
    }
}
