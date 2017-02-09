using System;
using System.Runtime.Serialization;

namespace SendSMS.Models
{
    [DataContract]
    public class CountryData
    {
        [DataMember(Name = "cc", Order = 2)]
        public string Code { get; set; }

        [DataMember(Name = "name", Order = 3)]
        public string Name { get; set; }

        [DataMember(Name = "mcc", Order = 1)]
        public string MobileCode { get; set; }

        [DataMember(Name = "pricePerSMS", Order = 4)]
        public decimal PricePerSMS { get; set; }

        public static CountryData FromCountry(Country country) => new CountryData(country);

        private CountryData(Country country)
        {
            MobileCode = country.MobileCode.ToString();
            Code = country.Code.ToString();
            Name = country.Name;
            PricePerSMS = Math.Round(country.PricePerSMS, 2);
        }
    }
}
