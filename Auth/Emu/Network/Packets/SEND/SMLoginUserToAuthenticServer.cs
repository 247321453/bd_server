using Auth.Emu.Enum;
using System;
using System.IO;

namespace Auth.Emu.Network.Packets.SEND
{
    [Message(MessageIds.SMLoginUserToAuthenticServer, true)]
    public class SMLoginUserToAuthenticServer : IMessage
    {
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(new byte[] { 0x77, 0x28, 0x3F, 0x54, 0x00, 0x01, 0x00, 0x00, 0x01, 0x76, 0x15, 0x00, 0x00
        });
        }

        public void Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}