using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SendSMS.Data
{
    public static class DataProvider
    {
        private static readonly Context DB = new Context();

        #region Public methods
        public static IEnumerable<Country> GetCountries() => DB.Countries;

        public static Country IdentifyCountry(string number)
        {
            return GetCountries().FirstOrDefault(c => number.StartsWith($"+{c.Code}", StringComparison.Ordinal));
        }

        public static void AddSMS(string from, string to, Country country, State state, DateTime sentTime)
        {
            SMS sms = CreateSMS(from, to, country, state, sentTime);
            AddSMS(sms);
        }

        public static IEnumerable<SMS> GetSentSMS(DateTime? from, DateTime? to, int skip, int? take)
        {
            IEnumerable<SMS> sms = GetSentSMS(from, to).Skip(skip);
            if (take.HasValue)
            {
                sms = sms.Take(take.Value);
            }
            return sms;
        }

        public static IEnumerable<Tuple<DateTime, Country, int>> GetStatistics(DateTime? from, DateTime? to,
                                                                               List<short> codes)
        {
            return GetSentSMS(from, to).Where(s => s.MobileCountryCode.HasValue
                                                   && ContainsIfPresent(codes, s.MobileCountryCode.Value))
                                       .Join(GetCountries(),
                                             s => s.MobileCountryCode.Value, country => country.MobileCode,
                                             (s, c) => new { s.SentTime.Date, Country = c })
                                       .GroupBy(pair => new { pair.Date, pair.Country }, pair => true)
                                       .Select(g => new Tuple<DateTime, Country, int>(g.Key.Date, g.Key.Country, g.Count()));
        }
        #endregion Public methods

        #region Helpers
        private static void AddSMS(SMS sms)
        {
            DB.SentSMS.Add(sms);
            DB.Entry(sms).State = EntityState.Added;
            DB.SaveChanges();
        }

        private static SMS CreateSMS(string from, string to, Country country, State state, DateTime sentTime)
        {
            return new SMS
            {
                From = from,
                To = to,
                MobileCountryCode = country?.MobileCode,
                Price = country?.PricePerSMS,
                State = state,
                SentTime = sentTime
            };
        }

        private static IEnumerable<SMS> GetSentSMS(DateTime? from, DateTime? to)
        {
            IEnumerable<SMS> sms = DB.SentSMS;
            if (from.HasValue)
            {
                sms = sms.Where(s => s.SentTime >= from.Value);
            }
            if (to.HasValue)
            {
                sms = sms.Where(s => s.SentTime <= to.Value);
            }
            return sms;
        }

        private static bool ContainsIfPresent<T>(IReadOnlyCollection<T> collection, T element)
        {
            return (collection == null) || (collection.Count == 0) || collection.Contains(element);
        }
        #endregion Helpers
    }
}
