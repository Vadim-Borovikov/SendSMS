using System;
using System.Collections.Generic;
using System.Linq;
using SendSMS.Models.API;
using Country = SendSMS.Models.DB.Country;
using SMS = SendSMS.Models.DB.SMS;

namespace SendSMS.Logic
{
    internal static class SMSHelper
    {
        public static Country IdentifyCountry(string number, IEnumerable<Country> countries)
        {
            return countries.FirstOrDefault(c => number.StartsWith($"+{c.Code}", StringComparison.Ordinal));
        }

        public static IEnumerable<SMS> FilterSMS(DateTime? from, DateTime? to, int skip, int? take, IEnumerable<SMS> sms)
        {
            IEnumerable<SMS> skipped = sms.Where(s => s.SentTime.IsBetween(from, to)).Skip(skip);
            return take.HasValue ? skipped.Take(take.Value) : skipped;
        }

        public static IEnumerable<Record> GetStatistics(DateTime? from, DateTime? to, IReadOnlyCollection<short> codes,
                                                        IEnumerable<Country> countries, IEnumerable<SMS> sms)
        {
            return
                sms.Where(message => message.MobileCountryCode.HasValue
                                     && message.SentTime.Date.IsBetween(from?.Date, to?.Date)
                                     && codes.ContainsIfPresent(message.MobileCountryCode.Value))
                   .Join(countries,
                         message => message.MobileCountryCode.Value, country => country.MobileCode,
                         (message, country) => new { message.SentTime.Date, country })
                   .GroupBy(pair => new { pair.Date, pair.country }, pair => true)
                   .Select(g => new Record(g.Key.Date, g.Key.country, g.Count()));
        }

        private static bool IsBetween(this DateTime dateTime, DateTime? start, DateTime? end)
        {
            return (!start.HasValue || (dateTime >= start.Value)) && (!end.HasValue || (dateTime <= end.Value));
        }

        private static bool ContainsIfPresent<T>(this IReadOnlyCollection<T> collection, T element)
        {
            return (collection == null) || (collection.Count == 0) || collection.Contains(element);
        }
    }
}