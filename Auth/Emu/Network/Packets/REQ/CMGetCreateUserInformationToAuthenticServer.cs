using Auth.Emu.Enum;
using System;
using System.IO;

namespace Auth.Emu.Network.Packets.REQ
{
    [Message(MessageIds.CMGetCreateUserInformationToAuthenticServer, true)]
    public class CMGetCreateUserInformationToAuthenticServer : IMessage
    {
        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader)
        {
            byte[] unk = reader.ReadBytes(2384);
        }
    }
}
