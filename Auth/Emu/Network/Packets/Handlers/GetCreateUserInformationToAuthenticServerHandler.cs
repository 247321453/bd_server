using Auth.Emu.Enum;
using Auth.Emu.Network.Packets.REQ;
using Auth.Emu.Network.Packets.SEND;

namespace Auth.Emu.Network.Packets.Handlers
{
    [MessageHandler(MessageIds.CMGetCreateUserInformationToAuthenticServer)]
    public class GetCreateUserInformationToAuthenticServerHandler
        : MessageHandler<CMGetCreateUserInformationToAuthenticServer>
    {
        protected override bool Process(ClientConnection connection, CMGetCreateUserInformationToAuthenticServer message)
        {
            connection.Send(new SMGetCreateUserInformationToAuthenticServer());
            /*
            string[] arrstring = message.data.Trim('\0').Split(',');
            string login_text = arrstring[0];
            string password_text = arrstring[1];
            //Console.WriteLine("Test:{0} : {1}", login_text, password_text);
            connection.Account = AccountManager.loadAccount(login_text);
            connection.Player = PlayerManager.loadPlayer(connection.Account.id);
            if (connection.Account.password == password_text)
            {
                if (connection.Account.pincode == null)
                {
                    connection.Send(new SMGetCreateUserInformationToAuthenticServer
                    {
                        accountid = connection.Player.accountid,
                        pinenable = 1
                    });
                }
                else
                {
                    connection.Send(new SMGetCreateUserInformationToAuthenticServer
                    {
                        accountid = connection.Player.accountid,
                        pinenable = 0
                    });
                }
                //Console.WriteLine("Token:{0}", connection.Account.id);
            }
            else
            {
                connection.Send(new SMNak
                {
                    ErrorString = ErrorStrings.eErrNoUserNotExistOrNotEqualPassword,
                    MessageId = MessageIds.CMGetCreateUserInformationToAuthenticServer
                });
            }*/
            return true;
        }
    }
}