using System.ComponentModel.DataAnnotations;

namespace SendSMS.Models
{
    public class Country
    {
        [Key]
        public byte Code { get; set; }

        public string Name { get; set; }
        public ushort MobileCode { get; set; }
        public decimal PricePerSMS { get; set; }
    }
}
