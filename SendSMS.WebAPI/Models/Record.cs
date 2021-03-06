﻿using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SendSMS.WebAPI.Models
{
    /// <summary>
    /// The statistics record
    /// </summary>
    [DataContract]
    public class Record
    {
        /// <summary>
        /// The date. Format: "yyyy-MM-dd".
        /// </summary>
        [DataMember(Name = "day", Order = 1)]
        [XmlElement(ElementName = "day", Order = 1, DataType = "date")]
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime Day { get; set; }

        /// <summary>
        /// The mobile country code.
        /// </summary>
        [DataMember(Name = "mcc", Order = 2)]
        [XmlElement(ElementName = "mcc", Order = 2)]
        public string MobileCountryCode { get; set; }

        /// <summary>
        /// The price per SMS in EUR, e.g. 0.06.
        /// </summary>
        [DataMember(Name = "pricePerSMS", Order = 3)]
        [XmlElement(ElementName = "pricePerSMS", Order = 3)]
        public decimal PricePerSMS { get; set; }

        /// <summary>
        /// The count of SMS on the day and mcc.
        /// </summary>
        [DataMember(Name = "count", Order = 4)]
        [XmlElement(ElementName = "count", Order = 4)]
        public int Count;

        /// <summary>
        /// The total price of all SMS on the day and mcc in EUR, e.g. 23.64.
        /// </summary>
        [DataMember(Name = "totalPrice", Order = 5)]
        [XmlElement(ElementName = "totalPrice", Order = 5)]
        public decimal TotalPrice { get; set; }
    }
}
