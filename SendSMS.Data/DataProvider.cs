using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SendSMS.Data
{
    public static class DataProvider
    {
        private static readonly Context DB = new Context();

        public static IEnumerable<Country> GetCountries() => DB.Countries;
        public static IEnumerable<SMS> GetSentSMS() => DB.SentSMS;

        public static void AddSMS(string from, string to, Country country, State state, DateTime sentTime)
        {
            SMS sms = CreateSMS(from, to, country, state, sentTime);
            AddSMS(sms);
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

        private static void AddSMS(SMS sms)
        {
            DB.SentSMS.Add(sms);
            DB.Entry(sms).State = EntityState.Added;
            DB.SaveChanges();
        }
    }
}
