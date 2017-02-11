using System;
using System.Collections.Generic;
using System.Linq;
using SendSMS.Models.DB;

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
    }
}