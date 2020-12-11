using Auth.Emu.Enum;
using System;
using System.IO;

namespace Auth.Emu.Network.Packets.SEND
{
    [Message(MessageIds.SMGetContentServiceInfo, true)]
    public class SMGetContentServiceInfo : IMessage
    {
        private static readonly ushort[] AccessibleRegionGroupKey =
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 31, 33, 34, 36, 37,
            38, 39, 40, 41, 42, 43, 44, 45, 46, 51, 52, 53, 54, 55, 56, 58, 61, 62, 63, 64, 101, 102, 103, 104, 105,
            106, 107, 108, 109, 110, 111, 112, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215
        };

        private static readonly byte[] PossibleClasses =
        {
            0, 4, 5, 8, 11, 12, 16, 17, 19, 20, 21, 23, 24, 25, 26, 27, 28, 31
        };

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)4);
            writer.Write((byte)6); // DEFAULT_CHARACTER_SLOT 
            writer.Write((byte)12); // CHARACTER_SLOT_LIMIT
            writer.Write((byte)3);
            writer.Write((byte)5);
            writer.Write((long)3000000);
            writer.Write(300000);
            writer.Write(0);
            writer.Write(300000); // ITEM_MARKET_REFUND_PERCENT_FOR_PREMIUM_PACKAGE
            writer.Write(0); // ITEM_MARKET_REFUND_PERCENT_FOR_PCROOM_AND_PREMIUM_PACKAGE
            writer.Write((long)60000); // BIDDING_TIME
            foreach (var possibleClass in PossibleClasses)
            {
                writer.Write((byte)possibleClass);
            }

            // POSSIBLE_CLASS
            for (var i = 0; i < 101 - PossibleClasses.Length; ++i)
            {
                writer.Write((byte)101);
            }
            writer.Write((int)0);
            writer.Write((int)1); // CHARACTER_REMOVE_TIME_CHECK_LEVEL
            writer.Write((ulong)86400); // LOW_LEVEL_CHARACTER_REMOVE_TIME
            writer.Write((ulong)86400); // CHARACTER_REMOVE_TIME
            writer.Write((ulong)86400); // NAME_REMOVE_TIME
            writer.Write((byte)0); // GUILD_WAR_TYPE - NORMAL, BOTH
            writer.Write((byte)1); // CAN_MAKE_GUILD
            writer.Write((byte)1); // CAN_REGISTER_PEARL_ITEM_ON_MARKET
            writer.Write((short)14);
            writer.Write((byte)0);
            writer.Write((int)150);
            writer.Write((byte)0); // OPEN_DESERT_PK
            writer.Write((short)0);
        }

        public void Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}