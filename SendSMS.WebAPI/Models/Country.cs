﻿using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SendSMS.WebAPI.Models
{
    /// <summary>
    /// The country
    /// </summary>
    [DataContract]
    public class Country
    {
        /// <summary>
        /// The country code of the country.
        /// </summary>
        [DataMember(Name = "cc", Order = 2)]
        [XmlElement(ElementName = "cc", Order = 2)]
        public string Code { get; set; }

        /// <summary>
        /// The name of the country.
        /// </summary>
        [DataMember(Name = "name", Order = 3)]
        [XmlElement(ElementName = "name", Order = 3)]
        public string Name { get; set; }

        /// <summary>
        /// The mobile country code of the country.
        /// </summary>
        [DataMember(Name = "mcc", Order = 1)]
        [XmlElement(ElementName = "mcc", Order = 1)]
        public string MobileCode { get; set; }

        /// <summary>
        /// The price per SMS sent in this country in EUR.
        /// </summary>
        [DataMember(Name = "pricePerSMS", Order = 4)]
        [XmlElement(ElementName = "pricePerSMS", Order = 4)]
        public decimal PricePerSMS { get; set; }
    }
}
