using Auth.Emu.Enum;
using Auth.Emu.Network.Packets.REQ;

namespace Auth.Emu.Network.Packets.Handlers
{
    [MessageHandler(MessageIds.CMHearthBeat)]
    public class HeartbeatHandler : MessageHandler<CMHeartbeat>
    {
        protected override bool Process(ClientConnection connection, CMHeartbeat message)
        {
            return true;
        }
    }
}
