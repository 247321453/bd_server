using System.IO;

namespace Auth.Emu.Network
{
    public interface IMessageHandler
    {
        bool Handle(ClientConnection connection, BinaryReader reader);
    }
}
