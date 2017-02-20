using System.Collections.Generic;
using System.Runtime.Serialization;
using SendSMS.ServiceModel.Types;

namespace SendSMS.ServiceModel
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
        public int TotalCount { get; set; }

        /// <summary>
        /// The items matching the filter.
        /// </summary>
        [DataMember(Name = "items", Order = 2)]
        public IEnumerable<SMS> Items { get; set; }
    }
}