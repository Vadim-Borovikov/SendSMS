using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SendSMS.WebAPI.Models
{
    /// <summary>
    /// The SMS records information.
    /// </summary>
    [DataContract]
    public class GetSentSMSResponse
    {
        /// <summary>
        /// The total count of all items matching the filter.
        /// </summary>
        [DataMember(Name = "totalCount", Order = 1)]
        public int TotalAmount { get; set; }

        /// <summary>
        /// The items matching the filter.
        /// </summary>
        [DataMember(Name = "items", Order = 2)]
        public List<SMS> Items { get; set; }
    }
}