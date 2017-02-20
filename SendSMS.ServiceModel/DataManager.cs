using System;
using System.Collections.Generic;
using System.Linq;
using SendSMS.Data;
using SendSMS.ServiceModel.Types;

namespace SendSMS.ServiceModel
{
    public static class DataManager
    {
        private static readonly DataProvider DataProvider = new DataProvider();
        private static readonly ISMSSender SMSSender = new DummySMSSender();

        #region ServiceInterface

        public static IEnumerable<Country> GetCountries()
        {
            return DataProvider.GetCountries().Select(CreateServiceModel);
        }

        public static Data.Types.State SendSMS(string from, string to, string text)
        {
            Data.Types.Country country = DataProvider.IdentifyCountry(to);

            var state = Data.Types.State.Failed;
            if (country != null)
            {
                state = SMSSender.SendSMS(from, to, country.MobileCode, text);
            }

            DataProvider.InsertSMS(from, to, country, DateTime.UtcNow, state);
            return state;
        }

        public static GetSentSMSResponse GetSentSMS(DateTime? from, DateTime? to, int skip, int? take)
        {
            List<Data.Types.SMS> records = DataProvider.GetSentSMS(from, to, skip, take);
            List<SMS> smsInfos = records.Select(CreateInfo).ToList();
            return CreateGetSentSMSResponse(smsInfos);
        }

        #endregion

        #region Helpers

        private static Country CreateServiceModel(Data.Types.Country country) => new Country
        {
            MobileCode = country.MobileCode.ToString(),
            Code = country.Code.ToString(),
            Name = country.Name,
            PricePerSMS = Math.Round(country.PricePerSMS, 2)
        };

        private static SMS CreateInfo(Data.Types.SMS sms) => new SMS
        {
            DateTime = sms.SentTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            From = sms.From,
            To = sms.To,
            MobileCountryCode = sms.Country?.MobileCode.ToString(),
            Price = sms.Country != null ? Math.Round(sms.Country.PricePerSMS, 2) : 0m,
            State = sms.State
        };

        public static GetSentSMSResponse CreateGetSentSMSResponse(IReadOnlyCollection<SMS> items)
        {
            return new GetSentSMSResponse
            {
                TotalCount = items.Count,
                Items = items
            };
        }

        /*public static Record CreateRecord(DateTime day, Data.Country country, int smsCount) => new Record
        {
            Day = day.ToString("yyyy-MM-dd"),
            MobileCountryCode = country.MobileCode.ToString(),
            PricePerSMS = Math.Round(country.PricePerSMS, 2),
            Count = smsCount,
            TotalPrice = Math.Round(country.PricePerSMS * smsCount, 2)
        };*/

        #endregion
    }
}
