using System;
using System.Collections.Generic;
using System.Linq;
using SendSMS.WebAPI.Models;

namespace SendSMS.WebAPI.BusinessLogic
{
    internal static class DataManager
    {
        private static readonly ISMSSender SMSSender = new DummySMSSender();

        #region CountriesController

        public static IEnumerable<Country> GetCountries()
        {
            List<Data.Country> countries = Data.DataProvider.GetCountries().ToList();
            return countries.Select(CreateInfo);
        }

        #endregion CountriesController

        #region SMSController

        public static Data.State SendSMS(string from, string to, string text)
        {
            Data.Country country = Data.DataProvider.IdentifyCountry(to);

            var state = Data.State.Failed;
            if (country != null)
            {
                state = SMSSender.SendSMS(from, to, country.MobileCode, text);
            }

            Data.DataProvider.AddSMS(from, to, country, state, DateTime.UtcNow);
            return state;
        }

        public static GetSentSMSResponse GetSentSMS(DateTime? from, DateTime? to, int skip, int? take)
        {
            List<Data.SMS> records = Data.DataProvider.GetSentSMS(from, to, skip, take).ToList();
            List<SMS> smsInfos = records.Select(CreateInfo).ToList();
            return CreateGetSentSMSResponse(smsInfos);
        }

        #endregion SMSController

        #region StatisticsController

        public static IEnumerable<Record> GetStatistics(DateTime? from, DateTime? to, string mccList)
        {
            List<short> codes = mccList?.Split(',').Select(short.Parse).ToList();
            List<Tuple<DateTime, Data.Country, int>> records =
                Data.DataProvider.GetStatistics(from, to, codes).ToList();
            return records.Select(r => CreateRecord(r.Item1, r.Item2, r.Item3));
        }

        #endregion StatisticsController

        #region Helpers

        private static SMS CreateInfo(Data.SMS sms) => new SMS
        {
            DateTime = sms.SentTime.ToString("yyyy-MM-ddTHH:mm:ss"),
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

        public static GetSentSMSResponse CreateGetSentSMSResponse(IReadOnlyCollection<SMS> items)
        {
            return new GetSentSMSResponse
            {
                TotalCount = items.Count,
                Items = items
            };
        }

        public static Record CreateRecord(DateTime day, Data.Country country, int smsCount) => new Record
        {
            Day = day.ToString("yyyy-MM-dd"),
            MobileCountryCode = country.MobileCode.ToString(),
            PricePerSMS = Math.Round(country.PricePerSMS, 2),
            Count = smsCount,
            TotalPrice = Math.Round(country.PricePerSMS * smsCount, 2)
        };

        #endregion Helpers
    }
}