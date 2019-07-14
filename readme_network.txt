在整个游戏初始化时，需要调用network.NetworkManager的SetClient函数，参数为true表示是客户端，参数为false表示是服务端，此函数必须在类中所有其他函数之前调用，
如果想要用户自己选择，那么做UI的同学负责根据用户的选择调用此函数
如果需要一个单独的服务器，服务器不是其中一个玩家的话，可以给服务器的处理程序打上标签并设置ServerBuild

使用网络通信的步骤：
1. 初始化:获得network.NetworkManager对象
    在自己的对象中添加public的network.NetworkManager对象，并初始化为null
    在Unity编辑器中将NetworkManager对象拖动给自己脚本的字段进行赋值
2. 注册事件
    在自己对象的初始化函数Start中，调用NetworkManager对象的RegisterEvent函数进行事件的注册：
    public void RegisterEvent(int event_id, EventHandler handler)
    事件的注册需要一个事件id和一个回调函数，id是一个int值，而且需要保持全局唯一。各位可以自己维护一组局部不重复的id，然后在前面加上学号后4位来生成一个全局唯一id
    回调函数的格式描述在NetworkManager对象的EventHandler：
    public delegate void EventHandler(int event_id, ByteBuffer event_data);
    这里会传入event_id，因此不同的事件id可以有相同的处理函数，然后自己在函数里判断事件id，用来处理相似的事件逻辑会比较方便，比如人物的左移和右移等
3. 触发事件：
    调用NetworkManager对象的TriggerEvent函数：
    public void TriggerEvent(int event_id, ByteBuffer event_data)


关于ByteBuffer和ISerializable：
所有需要进行网络传输的类都要实现ISerializable接口，接口中定义了Serialize和
Deserialize两个函数
而ByteBuffer封装了常用的数据类型(byte, int, float, double, UnityEngine.Vector3, String)，
各位只需要在自己的类里进行宏观的序列化和反序列化，无需进行底层的字节处理

注意：虽然使用UDP通信，但我添加了确认和重传机制，因此无需来回确认事件是否成功触发，事件的触发是保证了的

一个例子：
public class CPU : MonoBehaviour , ISerializable
{
    public String name;
    public double frequency;

    //序列化
    public Serialize(ByteBuffer buffer)
    {
        buffer.WriteString(name);
        buffer.WriteDouble(frequency);
    }

    //反序列化
    public Deserialize(ByteBuffer buffer)
    {
        name = buffer.ReadString();
        name = buffer.ReadDouble();
    }
}

public class Computer : MonoBehaviour, ISerializable
{
    public String name;
    public CPU cpu;
    public float cost;

    //序列化
    public Serialize(ByteBuffer buffer)
    {
        buffer.WriteString(name);
        buffer.WriteObj(cpu);   //由于CPU类实现了序列化，所以可以直接写入缓存
        buffer.WriteFloat(cost);
    }

    //反序列化
    public Deserialize(ByteBuffer buffer)
    {
        name = buffer.ReadString();

        //注意，反序列化时首先对象不能为null
        cpu = new CPU();
        buffer.ReadObject(cpu);

        buffer.ReadFloat(cost);
    }
}


可以通过if语句来跳过事件中不需要传递的字段，但要注意反序列化的顺序和形式一定要和序列化一致

事件使用例子：

//初始化时，注册买电脑的事件
RegisterEvent(4316001, OnComputerBuy);

//在服务器上，买了一台电脑
public void BuyComputer()
{
    Computer computer = new Computer();
    ....实例字段的赋值

    //触发事件
    ByteBuffer buffer = new ByteBuffer();
    computer.Serialize(buffer);
    TriggerEvent(4316001, buffer);
}

//客户端收到事件
public void OnComputerBuy(int event_id, ByteBuffer event_data)
{
    Computer computer = new Computer();
    //从buffer中构造对象
    computer.Deserialize(event_data);

    ...事件的处理
}

使用ByteBuffer作为参数的理由是如果只是需要简单地传递几个值，那么就没必要单独写一个类
例如，鼠标单击事件：

发送端：
ByteBuffer buffer = new ByteBuffer();
buffer.WriteInt(x);
buffer.WriteInt(y);
TriggerEvent(4316002, buffer);

接收端：
ByteBuffer buffer = new ByteBuffer();
int x = buffer.ReadInt();
int y = buffer.ReadInt();


此外，在客户端触发的事件会导致服务端的事件处理函数被调用，而在服务端触发的事件会导致所有客户端的事件处理函数被调用

例如，在游戏逻辑处理时，客户端的键鼠输入应该作为事件传给服务端（不同的角色可以根据自定义传入的参数来分辨），而服务端在根据不同的输入更新角色的位置等信息后，应该触发一个位置更新的事件，可以将新位置作为参数，事件会传递给所有的客户端

要避免在客户端直接通过键鼠输入更新位置信息，因为那样其他的客户端无法得知变化
