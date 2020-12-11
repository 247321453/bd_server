using Auth.Emu.Enum;
using System.IO;

namespace Auth.Emu.Network.Packets.REQ
{
    [Message(MessageIds.CMHearthBeat, false)]
    public class CMHeartbeat : IMessage
    {
        public void Serialize(BinaryWriter writer)
        {
        }

        public void Deserialize(BinaryReader reader)
        {
        }
    }
}