using Auth.Emu.Enum;
using Auth.Emu.Network.Packets.REQ;
using Auth.Emu.Network.Packets.SEND;

namespace Auth.Emu.Network.Packets.Handlers
{
    [MessageHandler(MessageIds.CMLoginUserToAuthenticServer)]
    public class LoginUserToAuthenticServerHandler : MessageHandler<CMLoginUserToAuthenticServer>
    {
        protected override bool Process(ClientConnection connection, CMLoginUserToAuthenticServer message)
        {
            connection.Send(new SMLoginUserToAuthenticServer());
            //connection.Send(new SMGetContentServiceInfo());
            /*
            if (connection.Account.pincode == null)
            {
                AccountManager.createpin(message.pincode);
                if (connection.Player.family == null)
                {
                    connection.Send(new SMLoginUserToAuthenticServer
                    {
                        newplayer = 1
                    });
                }
                else
                {
                    connection.Send(new SMLoginUserToAuthenticServer
                    {
                        newplayer = 0
                    });
                }
            }
            else
            {
                if (connection.Account.pincode == message.pincode)
                {
                    if (connection.Player.family == null)
                    {
                        connection.Send(new SMLoginUserToAuthenticServer
                        {
                            newplayer = 1
                        });
                    }
                    else
                    {
                        connection.Send(new SMLoginUserToAuthenticServer
                        {
                            newplayer = 0
                        });
                    }
                }
                else
                {
                    connection.Send(new SMLoginUserToAuthenticServerNak());
                }
            }*/
            return true;
        }
    }
}