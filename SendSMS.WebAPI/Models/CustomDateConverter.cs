using Newtonsoft.Json.Converters;

namespace SendSMS.WebAPI.Models
{
    internal class CustomDateConverter : IsoDateTimeConverter
    {
        public CustomDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}