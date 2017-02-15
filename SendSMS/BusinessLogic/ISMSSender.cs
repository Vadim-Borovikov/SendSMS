using SendSMS.Data;

namespace SendSMS.BusinessLogic
{
    internal interface ISMSSender
    {
        State SendSMS(string from, string to, short mobileCountryCode, string text);
    }
}