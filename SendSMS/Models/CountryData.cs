using System;
using System.Runtime.Serialization;

namespace SendSMS.Models
{
    /// <summary>
    /// The country information
    /// </summary>
    [DataContract]
    public class CountryData
    {
        /// <summary>
        /// The country code of the country.
        /// </summary>
        [DataMember(Name = "cc", Order = 2)]
        public string Code { get; set; }

        /// <summary>
        /// The name of the country.
        /// </summary>
        [DataMember(Name = "name", Order = 3)]
        public string Name { get; set; }

        /// <summary>
        /// The mobile country code of the country.
        /// </summary>
        [DataMember(Name = "mcc", Order = 1)]
        public string MobileCode { get; set; }

        /// <summary>
        /// The price per SMS sent in this country in EUR.
        /// </summary>
        [DataMember(Name = "pricePerSMS", Order = 4)]
        public decimal PricePerSMS { get; set; }

        public static CountryData FromCountry(Country country) => new CountryData(country);

        public CountryData() { }
        private CountryData(Country country)
        {
            MobileCode = country.MobileCode.ToString();
            Code = country.Code.ToString();
            Name = country.Name;
            PricePerSMS = Math.Round(country.PricePerSMS, 2);
        }
    }
}
