using System;
using ServiceStack.DataAnnotations;

namespace SendSMS.Data.Types
{
    public class SMS
    {
        [AutoIncrement]
        public int Id { get; set; }

        public DateTime SentTime { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public Country Country { get; set; }

        public State State { get; set; }
    }
}
