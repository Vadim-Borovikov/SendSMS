using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SendSMS.Models.API
{
    /// <summary>
    /// The SMS records information.
    /// </summary>
    public class GetSentSMSResult
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

        public GetSentSMSResult() { }

        internal GetSentSMSResult(IReadOnlyCollection<SMS> items)
        {
            TotalCount = items.Count;
            Items = items;
        }
    }
}