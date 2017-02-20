using ServiceStack;

namespace SendSMS.ServiceModel
{
    [Route("/sms/send", "GET")]
    public class SendSMS : IReturn<Data.Types.State>
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
    }
}