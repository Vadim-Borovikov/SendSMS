﻿using SendSMS.Data;

namespace SendSMS.WebAPI.BusinessLogic
{
    public interface ISMSSender
    {
        State SendSMS(string from, string to, short mobileCountryCode, string text);
    }
}