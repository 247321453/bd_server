using Auth.Emu.Enum;
using Core.Extension;
using System;
using System.IO;

namespace Auth.Emu.Network.Packets.REQ
{
    [Message(MessageIds.CMRegisterNickNameToAuthenticServer, true)]
    public class CMRegisterNickNameToAuthenticServer : IMessage
    {
        public string FamilyName { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader)
        {
            FamilyName = reader.ReadUnicodeString(62);
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
        }
    }
}