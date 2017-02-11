using System;
using System.Configuration;
using System.IO;
using SendSMS.Models;

namespace SendSMS.Logic
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