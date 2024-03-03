using MessagePack;

namespace Shared.Utilities
{
public static class MsgPackExtensions
{
    public static byte[] ToMsgPack(this object data) => MessagePackSerializer.Serialize(data);
    public static Type FromMsgPack<Type>(this byte[] bytes) => MessagePackSerializer.Deserialize<Type>(bytes);
}
}
