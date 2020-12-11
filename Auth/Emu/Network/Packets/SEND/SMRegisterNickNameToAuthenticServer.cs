using Auth.Emu.Enum;
using System;
using System.IO;

namespace Auth.Emu.Network.Packets.SEND
{
    [Message(MessageIds.SMRegisterNickNameToAuthenticServer, false)]
    public class SMRegisterNickNameToAuthenticServer : IMessage
    {
        public void Serialize(BinaryWriter writer)
        {
            writer.Write((uint)26403848); // ?
        }

        public void Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
