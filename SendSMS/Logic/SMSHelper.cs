using System;
using System.Collections.Generic;
using System.Linq;
using SendSMS.Models.API;
using Country = SendSMS.Models.DB.Country;
using SMS = SendSMS.Models.DB.SMS;

namespace SendSMS.Logic
{
    internal class SMSHelper
    {
        public static Country IdentifyCountry(string number, IEnumerable<Country> countries)
        {
            return countries.FirstOrDefault(c => number.StartsWith($"+{c.Code}", StringComparison.Ordinal));
        }

        public static IEnumerable<SMS> FilterSMS(DateTime from, DateTime to, int skip, int take, IEnumerable<SMS> sms)
        {
            return sms.Where(s => (s.SentTime >= from) && (s.SentTime <= to)).Skip(skip).Take(take);
        }

        public static IEnumerable<Record> GetStatistics(DateTime from, DateTime to, short[] codes,
                                                        IEnumerable<Country> countries, IEnumerable<SMS> sms)
        {
            return
                sms.Where(message => message.MobileCountryCode.HasValue
                                     && (message.SentTime.Date >= from) && (message.SentTime.Date <= to)
                                     && ((codes.Length == 0) || codes.Contains(message.MobileCountryCode.Value)))
                   .Join(countries,
                         message => message.MobileCountryCode.Value, country => country.MobileCode,
                         (message, country) => new { message.SentTime.Date, country })
                   .GroupBy(pair => new { pair.Date, pair.country }, pair => 1)
                   .Select(g => new Record(g.Key.Date, g.Key.country, g.Count()));
        }
    }
}