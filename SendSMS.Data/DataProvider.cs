using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using SendSMS.Data.Types;
using ServiceStack.OrmLite;

namespace SendSMS.Data
{
    public class DataProvider
    {
        private const string ConnectionName = "test";
        private readonly OrmLiteConnectionFactory _dbFactory;

        public DataProvider()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
            _dbFactory = new OrmLiteConnectionFactory(connectionString, MySqlDialect.Provider);
        }

        public List<Country> GetCountries()
        {
            using (IDbConnection db = _dbFactory.Open())
            {
                return GetCountries(db);
            }
        }

        public Country IdentifyCountry(string number)
        {
            using (IDbConnection db = _dbFactory.Open())
            {
                return IdentifyCountry(db, number);
            }
        }

        public void InsertSMS(string from, string to, Country country, DateTime sentTime, State state)
        {
            using (IDbConnection db = _dbFactory.Open())
            {
                InsertSMS(db, from, to, country, sentTime, state);
            }
        }

        public List<SMS> GetSentSMS(DateTime? from, DateTime? to, int skip, int? take)
        {
            using (IDbConnection db = _dbFactory.Open())
            {
                return GetSentSMS(db, from, to, skip, take);
            }
        }

        private static List<Country> GetCountries(IDbConnection db)
        {
            InitializeCountries(db);
            return db.Select<Country>();
        }

        private static Country IdentifyCountry(IDbConnection db, string number)
        {
            InitializeCountries(db);
            return db.Single<Country>("@number LIKE concat('+', code, '%')", new { number });
        }

        private static void InsertSMS(IDbConnection db, string from, string to, Country country, DateTime sentTime,
                                      State state)
        {
            db.CreateTableIfNotExists<SMS>();
            db.Insert(new SMS { From = from, To = to, Country = country, SentTime = sentTime, State = state });
        }

        private static List<SMS> GetSentSMS(IDbConnection db, DateTime? from, DateTime? to, int skip, int? take)
        {
            db.CreateTableIfNotExists<SMS>();
            SqlExpression<SMS> expression = db.From<SMS>();
            if (from.HasValue)
            {
                expression.Where(sms => sms.SentTime >= from.Value);
            }
            if (to.HasValue)
            {
                expression.Where(sms => sms.SentTime <= to.Value);
            }
            return db.Select(expression.Skip(skip).Take(take));
        }

        private static void InitializeCountries(IDbConnection db)
        {
            if (!db.CreateTableIfNotExists<Country>())
            {
                return;
            }

            db.Insert(new Country { Name = "Germany", MobileCode = 262, Code = 49, PricePerSMS = 0.055m });
            db.Insert(new Country { Name = "Austria", MobileCode = 232, Code = 43, PricePerSMS = 0.053m });
            db.Insert(new Country { Name = "Poland", MobileCode = 260, Code = 48, PricePerSMS = 0.032m });
        }

        /*public static IEnumerable<SMS> GetSentSMS() => DB.SentSMS;

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
        }*/
    }
}
