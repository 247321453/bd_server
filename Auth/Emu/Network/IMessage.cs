using System.IO;

namespace Auth.Emu.Network
{
    public interface IMessage
    {
        void Serialize(BinaryWriter writer);
        void Deserialize(BinaryReader reader);
    }
}
