using Auth.Emu.Enum;
using System;
using System.IO;

namespace Auth.Emu.Network.Packets.SEND
{
    [Message(MessageIds.SMSetFrameworkInformation, true)]
    public class SMSetFrameworkInformation : IMessage
    {
        public byte[] FrameWorkData { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(FrameWorkData);
        }

        public void Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException(nameof(SMSetFrameworkInformation));
        }
    }
}