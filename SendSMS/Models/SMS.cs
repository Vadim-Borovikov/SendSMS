using System;

namespace SendSMS.Models
{
    public class SMS
    {
        public int Id { get; set; }

        public DateTime? SentTime { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public short? MobileCountryCode { get; set; }

        public decimal? Price { get; set; }

        public State State { get; set; }

        public SMS() { }

        internal SMS(string from, string to, Country country, State state)
        {
            From = from;
            To = to;
            MobileCountryCode = country?.MobileCode;
            Price = country?.PricePerSMS;
            State = state;
            SentTime = state == State.Success ? DateTime.UtcNow : (DateTime?)null;
        }
    }
}
