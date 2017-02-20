using System;
using ServiceStack;

namespace SendSMS.ServiceModel
{
    [Route("/sms/sent", "GET")]
    public class GetSentSMS : IReturn<GetSentSMSResponse>
    {
        public DateTime? DateTimeFrom { get; set; }
        public DateTime? DateTimeTo { get; set; }
        public int Skip { get; set; }
        public int? Take { get; set; }
    }
}