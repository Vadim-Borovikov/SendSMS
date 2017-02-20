using ServiceStack.DataAnnotations;

namespace SendSMS.Data.Types
{
    public class Country
    {
        public byte Code { get; set; }

        public string Name { get; set; }

        public short MobileCode { get; set; }

        [DecimalLength(3, 3)]
        public decimal PricePerSMS { get; set; }
    }
}
