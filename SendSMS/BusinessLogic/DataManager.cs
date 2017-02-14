using System;
using System.Collections.Generic;
using System.Linq;
using SendSMS.Models;

namespace SendSMS.BusinessLogic
{
    internal static class DataManager
    {
        private static readonly ISMSSender SMSSender = new DummySMSSender();

        #region CountriesController

        public static IEnumerable<Country> GetCountries() => Data.DataProvider.GetCountries().Select(CreateInfo);

        #endregion CountriesController

        #region SMSController

        public static Data.State SendSMS(string from, string to, string text)
        {
            Data.Country country = IdentifyCountry(to);

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
            IEnumerable<Data.SMS> records =
                Data.DataProvider.GetSentSMS().Where(s => s.SentTime.IsBetween(from, to)).Skip(skip);
            if (take.HasValue)
            {
                records = records.Take(take.Value);
            }
            List<SMS> smsInfos = records.Select(CreateInfo).ToList();
            return CreateGetSentSMSResponse(smsInfos);
        }

        #endregion SMSController

        #region StatisticsController

        public static IEnumerable<Record> GetStatistics(DateTime? from, DateTime? to, string mccList)
        {
            List<short> codes = mccList?.Split(',').Select(short.Parse).ToList();

            return
                Data.DataProvider.GetSentSMS().Where(message => message.MobileCountryCode.HasValue
                                                                && message.SentTime.Date.IsBetween(from?.Date, to?.Date)
                                                                && codes.ContainsIfPresent(message.MobileCountryCode.Value))
                                              .Join(Data.DataProvider.GetCountries(),
                                                    message => message.MobileCountryCode.Value, country => country.MobileCode,
                                                    (message, country) => new { message.SentTime.Date, country })
                                              .GroupBy(pair => new { pair.Date, pair.country }, pair => true)
                                              .Select(g => CreateRecord(g.Key.Date, g.Key.country, g.Count()));
        }

        #endregion StatisticsController

        #region Helpers

        private static Data.Country IdentifyCountry(string number)
        {
            IEnumerable<Data.Country> countries = Data.DataProvider.GetCountries();
            return countries.FirstOrDefault(c => number.StartsWith($"+{c.Code}", StringComparison.Ordinal));
        }

        private static bool IsBetween(this DateTime dateTime, DateTime? start, DateTime? end)
        {
            return (!start.HasValue || (dateTime >= start.Value)) && (!end.HasValue || (dateTime <= end.Value));
        }

        private static bool ContainsIfPresent<T>(this IReadOnlyCollection<T> collection, T element)
        {
            return (collection == null) || (collection.Count == 0) || collection.Contains(element);
        }

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