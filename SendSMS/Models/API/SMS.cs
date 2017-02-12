using System;
using System.Runtime.Serialization;
using SendSMS.Models.DB;

namespace SendSMS.Models.API
{
    public class SMS
    {
        /// <summary>
        /// The date and time the SMS was sent, format: "yyyy-MM-ddTHH:mm:ss", UTC.
        /// </summary>
        [DataMember(Name = "dataTime", Order = 1)]
        public string DateTime { get; set; }

        /// <summary>
        /// The sender of the SMS.
        /// </summary>
        [DataMember(Name = "from", Order = 3)]
        public string From { get; set; }

        /// <summary>
        /// The receiver of the SMS.
        /// </summary>
        [DataMember(Name = "to", Order = 4)]
        public string To { get; set; }

        /// <summary>
        /// The mobile country code of the country where the receiver of the SMS belongs to.
        /// </summary>
        [DataMember(Name = "mcc", Order = 2)]
        public string MobileCountryCode { get; set; }

        /// <summary>
        /// The price of the SMS in EUR, e.g. 0.06.
        /// </summary>
        [DataMember(Name = "price", Order = 5)]
        public decimal? Price { get; set; }

        /// <summary>
        /// Success or Failed
        /// </summary>
        [DataMember(Name = "state", Order = 6)]
        public State State { get; set; }

        public static SMS FromDB(DB.SMS sms) => new SMS(sms);

        public SMS() { }
        private SMS(DB.SMS sms)
        {
            DateTime = sms.SentTime.ToString("yyyy-MM-ddTHH:mm:ss");
            From = sms.From;
            To = sms.To;
            MobileCountryCode = sms.MobileCountryCode?.ToString() ?? "";
            Price = sms.Price.HasValue ? Math.Round(sms.Price.Value, 2) : 0m;
            State = sms.State;
        }

    }
}