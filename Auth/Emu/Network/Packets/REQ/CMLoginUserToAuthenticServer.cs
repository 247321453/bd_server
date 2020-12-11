using Auth.Emu.Enum;
using Core.Extension;
using Core.Utils;
using System;
using System.IO;

namespace Auth.Emu.Network.Packets.REQ
{
    [Message(MessageIds.CMLoginUserToAuthenticServer, true)]
    public class CMLoginUserToAuthenticServer : IMessage
    {
        private int index;
        private int zeroNumbers;
        private int cookie;
        private int screenWidth;
        private int screenHeight;
        private string gpu;
        private string cpu;
        private string pcData;
        private string osData;
        private long pin;
        private string id;

        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader)
        {
            reader.ReadBytes(261);
            this.id = reader.ReadStringL(62);
            reader.ReadBytes(162);
            this.cookie = reader.ReadInt32();
            this.cpu = reader.ReadStringL(50);
            reader.ReadByte();
            reader.ReadInt32();
            this.gpu = reader.ReadStringL(50);
            reader.ReadByte();
            this.screenWidth = reader.ReadInt32();
            this.screenHeight = reader.ReadInt32();
            reader.ReadByte();
            reader.ReadByte();
            this.pcData = reader.ReadStringL(200);
            reader.ReadByte();
            this.osData = reader.ReadStringL(200);
            reader.ReadBytes(67);
        }
    }
}
