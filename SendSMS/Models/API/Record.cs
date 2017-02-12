using System;
using System.Runtime.Serialization;

namespace SendSMS.Models.API
{
    /// <summary>
    /// The country information
    /// </summary>
    [DataContract]
    public class Record
    {
        /// <summary>
        /// The date. Format: "yyyy-MM-dd".
        /// </summary>
        [DataMember(Name = "day", Order = 1)]
        public string Day { get; set; }

        /// <summary>
        /// The mobile country code.
        /// </summary>
        [DataMember(Name = "mcc", Order = 2)]
        public string MobileCountryCode { get; set; }

        /// <summary>
        /// The price per SMS in EUR, e.g. 0.06.
        /// </summary>
        [DataMember(Name = "pricePerSMS", Order = 3)]
        public decimal PricePerSMS { get; set; }

        /// <summary>
        /// The count of SMS on the day and mcc.
        /// </summary>
        [DataMember(Name = "count", Order = 4)]
        public int Count;

        /// <summary>
        /// The total price of all SMS on the day and mcc in EUR, e.g. 23.64.
        /// </summary>
        [DataMember(Name = "totalPrice", Order = 5)]
        public decimal TotalPrice { get; set; }

        public Record() { }

        internal Record(DateTime day, DB.Country country, int smsCount)
        {
            Day = day.ToString("yyyy-MM-dd");
            MobileCountryCode = country.MobileCode.ToString();
            PricePerSMS = Math.Round(country.PricePerSMS, 2);
            Count = smsCount;
            TotalPrice = Math.Round(country.PricePerSMS * Count, 2);
        }
    }
}
