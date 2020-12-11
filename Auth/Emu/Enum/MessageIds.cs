namespace Auth.Emu.Enum
{
    public enum MessageIds : ushort
    {
        // Authentication - Login
        CMHearthBeat = 0x03E9,
        SMSetFrameworkInformation = 0x03EB,
        SMNak = 0x03F4,
        CMLoginUserToAuthenticServer = 0x0CB0,
        SMLoginUserToAuthenticServer = 0x0CB1,
        CMGetCreateUserInformationToAuthenticServer = 0x0CAE,
        SMGetCreateUserInformationToAuthenticServer = 0x0CAF,
        CMRegisterNickNameToAuthenticServer = 0x0CB3,
        SMRegisterNickNameToAuthenticServer = 0x0CB4,
        SMGetContentServiceInfo = 0x0CD7,
        SMGetWorldInformations = 0x0CB6,
        SMLoadChatMacro = 0x122C,
    }
}