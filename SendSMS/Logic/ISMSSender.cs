using System;
using SendSMS.Models;

namespace SendSMS.Logic
{
    public interface ISMSSender
    {
        State SendSMS(string from, string to, short mobileCountryCode, string text);
    }
}