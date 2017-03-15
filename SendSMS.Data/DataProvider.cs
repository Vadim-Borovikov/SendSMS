using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SendSMS.Data
{
    public static class DataProvider
    {
        private static readonly Context DB = new Context();

        #region Public methods

        public static async Task<List<Country>> GetCountriesAsync() => await DB.Countries.ToListAsync();

        public static async Task<Country> IdentifyCountry(string number)
        {
            // ReSharper disable once StringStartsWithIsCultureSpecific
            return await DB.Countries.FirstOrDefaultAsync(c => number.StartsWith("+" + c.Code));
        }

        public static async Task<int> AddSMSAsync(string from, string to, Country country, State state, DateTime sentTime)
        {
            SMS sms = CreateSMS(from, to, country, state, sentTime);
            return await AddSMSAsync(sms);
        }

        public static async Task<int> GetSentSMSAmountAsync(DateTime? from, DateTime? to)
        {
            return await GetSentSMS(from, to).CountAsync();
        }

        public static async Task<List<SMS>> GetSentSMSAsync(DateTime? from, DateTime? to, int skip, int? take)
        {
            IQueryable<SMS> sms = GetSentSMS(from, to).OrderBy(s => s.SentTime).Skip(skip);
            if (take.HasValue)
            {
                sms = sms.Take(take.Value);
            }
            return await sms.ToListAsync();
        }

        public static async Task<List<Record>> GetStatisticsAsync(DateTime? from, DateTime? to, List<short> codes)
        {
            if (codes == null)
            {
                codes = new List<short>();
            }
            return await GetSentSMS(from, to).Where(s => s.MobileCountryCode.HasValue
                                                         && ((codes.Count == 0) || codes.Contains(s.MobileCountryCode.Value)))
                                             .Join(DB.Countries,
                                                   s => s.MobileCountryCode.Value, country => country.MobileCode,
                                                   (s, c) => new { Date = DbFunctions.TruncateTime(s.SentTime).Value, Country = c })
                                             .GroupBy(pair => new { pair.Date, pair.Country }, pair => true)
                                             .Select(g => new Record { Day = g.Key.Date, Country = g.Key.Country, Count = g.Count() })
                                             .ToListAsync();
        }
        #endregion Public methods

        #region Helpers
        private static Task<int> AddSMSAsync(SMS sms)
        {
            DB.SentSMS.Add(sms);
            DB.Entry(sms).State = EntityState.Added;
            return DB.SaveChangesAsync();
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

        private static IQueryable<SMS> GetSentSMS(DateTime? from, DateTime? to)
        {
            IQueryable<SMS> sms = DB.SentSMS;
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
        #endregion Helpers
    }
}
