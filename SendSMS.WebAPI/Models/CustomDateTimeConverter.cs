using Newtonsoft.Json.Converters;

namespace SendSMS.WebAPI.Models
{
    internal class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        }
    }
}