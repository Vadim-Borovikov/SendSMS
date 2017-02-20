using System;
using System.IO;
using SendSMS.Data.Types;
using System.Configuration;

namespace SendSMS.ServiceModel
{
    public class DummySMSSender : ISMSSender
    {
        public State SendSMS(string from, string to, short mobileCountryCode, string text)
        {
            File.AppendAllText(LogPath,
                $"{DateTime.UtcNow}: {from} -> {to}{Environment.NewLine}{text}{Environment.NewLine}{Environment.NewLine}");
            return State.Success;
        }

        private static readonly string LogPath = ConfigurationManager.AppSettings["LogFilePath"];
    }
}