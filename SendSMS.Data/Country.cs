using System.ComponentModel.DataAnnotations;
using RichardLawley.EF.AttributeConfig;

namespace SendSMS.Data
{
    public class Country
    {
        [Key]
        public byte Code { get; set; }

        public string Name { get; set; }

        public short MobileCode { get; set; }

        [DecimalPrecision(3, 3)]
        public decimal PricePerSMS { get; set; }
    }
}
