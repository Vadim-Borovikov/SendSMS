using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using SendSMS.Data;

namespace SendSMS.WebAPI.BusinessLogic
{
    internal class DummySMSSender : ISMSSender
    {
        public State SendSMS(string from, string to, short mobileCountryCode, string text)
        {
            lock (Locker)
            {
                File.AppendAllText(LogPath,
                    $"{DateTime.UtcNow}: {from} -> {to}{Environment.NewLine}{text}{Environment.NewLine}{Environment.NewLine}");
            }
            return State.Success;
        }

        public async Task<State> SendSMSAsync(string from, string to, short mobileCountryCode, string text)
        {
            return await Task<State>.Factory.StartNew(() => SendSMS(from, to, mobileCountryCode, text));
        }

        private static readonly object Locker = new object();
        private static readonly string LogPath = ConfigurationManager.AppSettings["LogFilePath"];
    }
}