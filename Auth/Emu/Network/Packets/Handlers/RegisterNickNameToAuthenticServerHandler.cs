using Auth.Emu.Enum;
using Auth.Emu.Network.Packets.REQ;
using Auth.Emu.Network.Packets.SEND;

namespace Auth.Emu.Network.Packets.Handlers
{
    [MessageHandler(MessageIds.CMRegisterNickNameToAuthenticServer)]
    public class RegisterNickNameToAuthenticServerHandler : MessageHandler<CMRegisterNickNameToAuthenticServer>
    {
        //Channel channel = ChannelSerializer.Load();
        protected override bool Process(ClientConnection connection, CMRegisterNickNameToAuthenticServer message)
        {
            connection.Send(new SMRegisterNickNameToAuthenticServer());
            connection.Send(new SMGetContentServiceInfo());
            connection.Send(new SMLoadChatMacro());
            connection.Send(new SMGetWorldInformations());
            /*
            if (connection.Player.family == null)
            {
                if (message.FamilyName.Length <= 3 || message.FamilyName.Length >= 16)
                {
                    connection.Send(new SMNak
                    {
                        ErrorString = ErrorStrings.eErrNoNameCharacterIsInvalid,
                        MessageId = MessageIds.CMRegisterNickNameToAuthenticServer
                    });
                }
                else
                {
                    PlayerManager.createfamily(message.FamilyName);
                    connection.Send(new SMRegisterNickNameToAuthenticServer());
                    Channel channel = ChannelSerializer.Load();
                    connection.Send(new SMGetContentServiceInfo());
                    connection.Send(new SMFixedCharge());
                    connection.Send(new SMGetWorldInformations { channel = channel });
                }
            }
            else
            {
                connection.Send(new SMRegisterNickNameToAuthenticServer());
                Channel channel = ChannelSerializer.Load();
                connection.Send(new SMGetContentServiceInfo());
                connection.Send(new SMFixedCharge());
                connection.Send(new SMGetWorldInformations { channel = channel });
            }*/
            return true;
        }
    }
}
