using SendSMS.Data.Types;

namespace SendSMS.ServiceModel
{
    public interface ISMSSender
    {
        State SendSMS(string from, string to, short mobileCountryCode, string text);
    }
}