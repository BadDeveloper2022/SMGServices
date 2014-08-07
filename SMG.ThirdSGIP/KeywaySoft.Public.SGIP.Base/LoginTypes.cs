namespace KeywaySoft.Public.SGIP.Base
{
    using System;

    public enum LoginTypes : byte
    {
        GnsToGns = 6,
        GnsToSmg = 5,
        SmgToGns = 4,
        SmgToSmg = 3,
        SmgToSp = 2,
        SpToSmg = 1,
        Test = 11,
        Unknown = 0
    }
}

