using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendSMS.WebAPI.Models;

namespace SendSMS.WebAPI.BusinessLogic
{
    internal static class DataManager
    {
        #region CountriesController

        public static async Task<List<Country>> GetCountriesAsync()
        {
            List<Data.Country> countries = await Data.DataProvider.GetCountriesAsync();
            return countries.Select(CreateInfo).ToList();
        }

        #endregion CountriesController

        #region SMSController

        public static async Task<Data.State> SendSMSAsync(string from, string to, string text, ISMSSender smsSender)
        {
            Data.Country country = await Data.DataProvider.IdentifyCountry(to);

            var state = Data.State.Failed;
            if (country != null)
            {
                state = await smsSender.SendSMSAsync(from, to, country.MobileCode, text);
            }

            int saved = await Data.DataProvider.AddSMSAsync(from, to, country, state, DateTime.UtcNow);
            return saved > 0 ? state : Data.State.Failed;
        }

        public static async Task<GetSentSMSResponse> GetSentSMSAsync(DateTime? from, DateTime? to, int skip, int? take)
        {
            List<Data.SMS> records = await Data.DataProvider.GetSentSMSAsync(from, to, skip, take);
            List<SMS> smsInfos = records.Select(CreateInfo).ToList();
            return CreateGetSentSMSResponse(smsInfos);
        }

        #endregion SMSController

        #region StatisticsController

        public static async Task<List<Record>> GetStatisticsAsync(DateTime? from, DateTime? to, string mccList)
        {
            List<short> codes = mccList?.Split(',').Select(short.Parse).ToList();
            List<Data.Record> records = await Data.DataProvider.GetStatisticsAsync(from, to, codes);
            return records.Select(CreateRecord).ToList();
        }

        #endregion StatisticsController

        #region Helpers

        private static SMS CreateInfo(Data.SMS sms) => new SMS
        {
            DateTime = sms.SentTime,
            From = sms.From,
            To = sms.To,
            MobileCountryCode = sms.MobileCountryCode?.ToString() ?? "",
            Price = sms.Price.HasValue ? Math.Round(sms.Price.Value, 2) : 0m,
            State = sms.State
        };

        public static Country CreateInfo(Data.Country country) => new Country
        {
            MobileCode = country.MobileCode.ToString(),
            Code = country.Code.ToString(),
            Name = country.Name,
            PricePerSMS = Math.Round(country.PricePerSMS, 2)
        };

        public static GetSentSMSResponse CreateGetSentSMSResponse(List<SMS> items)
        {
            return new GetSentSMSResponse
            {
                TotalCount = items.Count,
                Items = items
            };
        }

        public static Record CreateRecord(Data.Record record) => new Record
        {
            Day = record.Day,
            MobileCountryCode = record.Country.MobileCode.ToString(),
            PricePerSMS = Math.Round(record.Country.PricePerSMS, 2),
            Count = record.Count,
            TotalPrice = Math.Round(record.Country.PricePerSMS * record.Count, 2)
        };

        #endregion Helpers
    }
}