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

        public static IEnumerable<Country> GetCountries(IEnumerable<Data.Country> countries)
        {
            return countries.Select(CreateInfo);
        }

        #endregion CountriesController

        #region SMSController

        public static Data.State SendSMS(string from, string to, string text, Data.Context db)
        {
            Data.Country country = IdentifyCountry(to, db.Countries);

            var state = Data.State.Failed;
            if (country != null)
            {
                state = SMSSender.SendSMS(from, to, country.MobileCode, text);
            }

            Data.SMS sms = CreateSMS(from, to, country, state);
            Data.ContextHelper.AddSMS(sms, db);
            return state;
        }

        public static GetSentSMSResponse GetSentSMS(DateTime? dateTimeFrom, DateTime? dateTimeTo, int skip, int? take,
                                                    IEnumerable<Data.SMS> sms)
        {
            IEnumerable<Data.SMS> records = sms.Where(s => s.SentTime.IsBetween(dateTimeFrom, dateTimeTo)).Skip(skip);
            if (take.HasValue)
            {
                records = records.Take(take.Value);
            }
            List<SMS> smsInfos = records.Select(CreateInfo).ToList();
            return CreateGetSentSMSResponse(smsInfos);
        }

        #endregion SMSController

        #region StatisticsController

        public static IEnumerable<Record> GetStatistics(DateTime? dateFrom, DateTime? dateTo, string mccList,
                                                        IEnumerable<Data.SMS> sms, IEnumerable<Data.Country> countries)
        {
            List<short> codes = mccList?.Split(',').Select(short.Parse).ToList();

            return sms.Where(message => message.MobileCountryCode.HasValue
                                        && message.SentTime.Date.IsBetween(dateFrom?.Date, dateTo?.Date)
                                        && codes.ContainsIfPresent(message.MobileCountryCode.Value))
                      .Join(countries,
                            message => message.MobileCountryCode.Value, country => country.MobileCode,
                            (message, country) => new { message.SentTime.Date, country })
                      .GroupBy(pair => new { pair.Date, pair.country }, pair => true)
                      .Select(g => CreateRecord(g.Key.Date, g.Key.country, g.Count()));
        }

        #endregion StatisticsController

        #region Helpers

        private static Data.Country IdentifyCountry(string number, IEnumerable<Data.Country> countries)
        {
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

        private static Data.SMS CreateSMS(string from, string to, Data.Country country, Data.State state)
        {
            return new Data.SMS
            {
                From = from,
                To = to,
                MobileCountryCode = country?.MobileCode,
                Price = country?.PricePerSMS,
                State = state,
                SentTime = DateTime.UtcNow
            };
        }

        public static SMS CreateInfo(Data.SMS sms) => new SMS
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