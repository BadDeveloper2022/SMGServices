namespace KeywaySoft.Public.SGIP.Base
{
    using System;

    public enum ErrorCodes : byte
    {
        ConnectionFull = 3,
        ErrorLoginType = 4,
        FeeNumberError = 0x18,
        FullSequence = 0x21,
        GnsOperationError = 10,
        HandsetCanNotRecvSms = 0x1b,
        HandsetFull = 0x1a,
        HandsetReturnError = 0x1c,
        InvalidateDevice = 0x1f,
        LoginError = 1,
        MsgIDError = 7,
        NodeBusy = 11,
        NodeCanNotReachable = 0x15,
        NoDevice = 30,
        OtherError = 0x63,
        PackageLengthError = 8,
        ParameterError = 5,
        Relogon = 2,
        RouteError = 0x16,
        RoutNodeNotExisted = 0x17,
        SequenceError = 9,
        Success = 0,
        SystemError = 0x20,
        TelnumberError = 6,
        UnknownUser = 0x1d,
        UserCanNotReachable = 0x19
    }
}

