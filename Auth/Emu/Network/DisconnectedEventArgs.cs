using System;

namespace Auth.Emu.Network
{
    public class DisconnectedEventArgs : EventArgs
    {
        public string Reason { get; }

        public DisconnectedEventArgs(string reason)
        {
            Reason = reason;
        }
    }
}
